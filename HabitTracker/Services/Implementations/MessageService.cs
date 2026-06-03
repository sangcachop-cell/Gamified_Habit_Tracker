using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;
    private readonly INotificationService _notifications;
    private const int PAGE_SIZE = 20;

    public MessageService(AppDbContext context, INotificationService notifications)
    {
        _context = context;
        _notifications = notifications;
    }

    public async Task<(bool Success, string? Error, Message? Msg)> SendAsync(int senderId, int receiverId, string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return (false, "Message body cannot be empty.", null);

        body = body.Trim();
        if (body.Length > 2000)
            return (false, "Message exceeds 2000 characters.", null);

        if (senderId == receiverId)
            return (false, "Cannot message yourself.", null);

        var receiver = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == receiverId);
        if (receiver == null)
            return (false, "User not found.", null);

        var sender = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == senderId);
        if (sender == null)
            return (false, "Sender not found.", null);

        if (sender.IsBanned)
            return (false, "Your account has been banned.", null);

        if (sender.IsMuted)
            return (false, "You have been muted and cannot send messages.", null);

        if (await IsBlockedAsync(senderId, receiverId))
            return (false, "Cannot send message — user is blocked.", null);

        if (receiver.PMPermission == "nobody")
            return (false, "This user is not accepting private messages.", null);

        var message = new Message
        {
            SenderId    = senderId,
            ReceiverId  = receiverId,
            Body        = body,
            SentAt      = DateTime.UtcNow,
            IsRead      = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        // Notify receiver
        await _notifications.CreateNotificationAsync(
            receiverId,
            $"New message from {sender?.Username ?? "someone"}",
            body.Length > 80 ? body[..80] + "…" : body,
            "Social",
            $"/Message/Conversation/{senderId}",
            "✉️"
        );

        return (true, null, message);
    }

    public async Task<List<ConversationSummary>> GetConversationsAsync(int userId)
    {
        // Pull all messages where user is sender or receiver, not deleted on their side
        var sent = await _context.Messages
            .AsNoTracking()
            .Where(m => m.SenderId == userId && !m.DeletedBySender)
            .Include(m => m.Receiver)
            .ToListAsync();

        var received = await _context.Messages
            .AsNoTracking()
            .Where(m => m.ReceiverId == userId && !m.DeletedByReceiver)
            .Include(m => m.Sender)
            .ToListAsync();

        // Build conversation partners dict: partnerId → (lastMessage, unreadCount)
        var conversations = new Dictionary<int, (Message Last, int Unread)>();

        foreach (var m in sent)
        {
            var partnerId = m.ReceiverId;
            if (!conversations.TryGetValue(partnerId, out var existing) || m.SentAt > existing.Last.SentAt)
                conversations[partnerId] = (m, conversations.TryGetValue(partnerId, out var ex2) ? ex2.Unread : 0);
        }

        foreach (var m in received)
        {
            var partnerId = m.SenderId;
            var unread = !m.IsRead ? 1 : 0;
            if (!conversations.TryGetValue(partnerId, out var existing))
            {
                conversations[partnerId] = (m, unread);
            }
            else
            {
                var newLast = m.SentAt > existing.Last.SentAt ? m : existing.Last;
                conversations[partnerId] = (newLast, existing.Unread + unread);
            }
        }

        // Resolve unread counts properly per partner
        var unreadCounts = await _context.Messages
            .AsNoTracking()
            .Where(m => m.ReceiverId == userId && !m.IsRead && !m.DeletedByReceiver)
            .GroupBy(m => m.SenderId)
            .Select(g => new { SenderId = g.Key, Count = g.Count() })
            .ToListAsync();

        var unreadByPartner = unreadCounts.ToDictionary(x => x.SenderId, x => x.Count);

        // Load user records for all partners
        var partnerIds = conversations.Keys.ToList();
        var partnerUsers = await _context.Users
            .AsNoTracking()
            .Where(u => partnerIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        // Load blocks
        var blocks = await _context.UserBlocks
            .AsNoTracking()
            .Where(b => b.BlockerId == userId && partnerIds.Contains(b.BlockedId))
            .Select(b => b.BlockedId)
            .ToListAsync();

        var result = conversations
            .Where(kv => partnerUsers.ContainsKey(kv.Key))
            .Select(kv => new ConversationSummary
            {
                OtherUser   = partnerUsers[kv.Key],
                LastMessage = kv.Value.Last,
                UnreadCount = unreadByPartner.TryGetValue(kv.Key, out var uc) ? uc : 0,
                IsBlocked   = blocks.Contains(kv.Key)
            })
            .OrderByDescending(c => c.LastMessage.SentAt)
            .ToList();

        return result;
    }

    public async Task<List<MessageEntry>> GetConversationAsync(int userId, int otherId, int page = 0)
    {
        var messages = await _context.Messages
            .AsNoTracking()
            .Where(m =>
                (m.SenderId == userId && m.ReceiverId == otherId && !m.DeletedBySender) ||
                (m.SenderId == otherId && m.ReceiverId == userId && !m.DeletedByReceiver))
            .Include(m => m.Likes)
            .OrderByDescending(m => m.SentAt)
            .Skip(page * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .ToListAsync();

        messages.Reverse(); // oldest-first for display

        return messages.Select(m => new MessageEntry
        {
            Message    = m,
            LikedByMe  = m.Likes.Any(l => l.LikerUserId == userId),
            LikeCount  = m.Likes.Count
        }).ToList();
    }

    public async Task MarkReadAsync(int userId, int otherId)
    {
        var unread = await _context.Messages
            .Where(m => m.ReceiverId == userId && m.SenderId == otherId && !m.IsRead)
            .ToListAsync();

        if (unread.Count == 0) return;

        var now = DateTime.UtcNow;
        foreach (var m in unread)
        {
            m.IsRead  = true;
            m.ReadAt  = now;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null) return (false, "Message not found.");

        if (message.SenderId != userId && message.ReceiverId != userId)
            return (false, "Not authorised.");

        if (message.SenderId == userId)
            message.DeletedBySender = true;
        else
            message.DeletedByReceiver = true;

        // Hard-delete if both sides deleted
        if (message.DeletedBySender && message.DeletedByReceiver)
            _context.Messages.Remove(message);

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _context.Messages
            .AsNoTracking()
            .CountAsync(m => m.ReceiverId == userId && !m.IsRead && !m.DeletedByReceiver);
    }

    public async Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId)
    {
        var existing = await _context.MessageLikes
            .FirstOrDefaultAsync(l => l.MessageId == messageId && l.LikerUserId == userId);

        if (existing != null)
        {
            _context.MessageLikes.Remove(existing);
        }
        else
        {
            _context.MessageLikes.Add(new MessageLike
            {
                MessageId   = messageId,
                LikerUserId = userId,
                LikedAt     = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();

        var count = await _context.MessageLikes.CountAsync(l => l.MessageId == messageId);
        return (existing == null, count);
    }

    public async Task<bool> IsBlockedAsync(int userA, int userB)
    {
        return await _context.UserBlocks
            .AsNoTracking()
            .AnyAsync(b =>
                (b.BlockerId == userA && b.BlockedId == userB) ||
                (b.BlockerId == userB && b.BlockedId == userA));
    }

    public async Task<bool> IsBlockerAsync(int blockerId, int blockedId)
    {
        return await _context.UserBlocks
            .AsNoTracking()
            .AnyAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
    }

    public async Task<bool> ToggleBlockAsync(int blockerId, int blockedId)
    {
        var existing = await _context.UserBlocks
            .FirstOrDefaultAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);

        if (existing != null)
        {
            _context.UserBlocks.Remove(existing);
            await _context.SaveChangesAsync();
            return false; // now unblocked
        }

        _context.UserBlocks.Add(new UserBlock
        {
            BlockerId  = blockerId,
            BlockedId  = blockedId,
            CreatedAt  = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return true; // now blocked
    }

    public async Task<(bool Success, string? Error)> ReportUserAsync(int reporterId, int reportedUserId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return (false, "Reason is required.");

        if (reporterId == reportedUserId)
            return (false, "Cannot report yourself.");

        // Prevent duplicate report within 24h
        var cutoff = DateTime.UtcNow.AddHours(-24);
        var duplicate = await _context.Reports
            .AsNoTracking()
            .AnyAsync(r => r.ReporterId == reporterId && r.ReportedUserId == reportedUserId
                        && r.ReportedMessageId == null && r.CreatedAt > cutoff);

        if (duplicate)
            return (false, "You have already reported this user recently.");

        _context.Reports.Add(new Report
        {
            ReporterId      = reporterId,
            ReportedUserId  = reportedUserId,
            Reason          = reason.Trim()[..Math.Min(reason.Trim().Length, 500)],
            CreatedAt       = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> ReportMessageAsync(int reporterId, int messageId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return (false, "Reason is required.");

        var message = await _context.Messages.AsNoTracking().FirstOrDefaultAsync(m => m.Id == messageId);
        if (message == null)
            return (false, "Message not found.");

        if (message.SenderId == reporterId)
            return (false, "Cannot report your own message.");

        // Prevent duplicate
        var cutoff = DateTime.UtcNow.AddHours(-24);
        var duplicate = await _context.Reports
            .AsNoTracking()
            .AnyAsync(r => r.ReporterId == reporterId && r.ReportedMessageId == messageId && r.CreatedAt > cutoff);

        if (duplicate)
            return (false, "You have already reported this message recently.");

        _context.Reports.Add(new Report
        {
            ReporterId         = reporterId,
            ReportedUserId     = message.SenderId,
            ReportedMessageId  = messageId,
            Reason             = reason.Trim()[..Math.Min(reason.Trim().Length, 500)],
            CreatedAt          = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return (true, null);
    }
}
