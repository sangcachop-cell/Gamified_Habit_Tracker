using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HabitTracker.Services.Implementations;

public class PartyService : IPartyService
{
    private readonly AppDbContext _context;
    private readonly INotificationService _notifications;
    private const int PAGE_SIZE = 20;

    public PartyService(AppDbContext context, INotificationService notifications)
    {
        _context = context;
        _notifications = notifications;
    }

    public async Task<(bool Success, string? Error, Party? Party)> CreateAsync(int leaderId, string name)
    {
        name = name.Trim();
        if (string.IsNullOrWhiteSpace(name)) return (false, "Party name is required.", null);
        if (name.Length > 100) return (false, "Name exceeds 100 characters.", null);

        // 1 party per user constraint
        if (await GetMyPartyAsync(leaderId) != null)
            return (false, "You are already in a party. Leave it first.", null);

        var party = new Party
        {
            Name      = name,
            LeaderId  = leaderId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Parties.Add(party);
        await _context.SaveChangesAsync();

        _context.PartyMembers.Add(new PartyMember
        {
            PartyId  = party.Id,
            UserId   = leaderId,
            Role     = "Leader",
            JoinedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        return (true, null, party);
    }

    public async Task<Party?> GetMyPartyAsync(int userId)
    {
        var membership = await _context.PartyMembers
            .AsNoTracking()
            .Include(m => m.Party)
            .FirstOrDefaultAsync(m => m.UserId == userId);
        return membership?.Party;
    }

    public async Task<PartyViewModel?> GetPartyViewAsync(int partyId, int userId, int page = 0)
    {
        var party = await _context.Parties.AsNoTracking().FirstOrDefaultAsync(p => p.Id == partyId);
        if (party == null) return null;

        var members = await _context.PartyMembers
            .AsNoTracking()
            .Where(m => m.PartyId == partyId)
            .Include(m => m.User)
            .OrderBy(m => m.Role == "Leader" ? 0 : 1)
            .ThenBy(m => m.JoinedAt)
            .ToListAsync();

        var messages  = await GetMessagesAsync(partyId, userId, page);
        var myMember  = members.FirstOrDefault(m => m.UserId == userId);
        var isLeader  = myMember?.Role == "Leader";

        var pendingInvites = isLeader
            ? await GetPendingInvitesAsync(userId, partyId)
            : new List<PartyInvite>();

        // Phase 8: quest status
        var activeQuest = await _context.PartyQuests
            .AsNoTracking()
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members).ThenInclude(m => m.User)
            .Include(pq => pq.Leader)
            .FirstOrDefaultAsync(pq => pq.PartyId == partyId &&
                                       (pq.Status == "Pending" || pq.Status == "Active"));

        PartyQuestStatusDto? questStatus = null;
        if (activeQuest is not null)
        {
            questStatus = new PartyQuestStatusDto
            {
                Status       = activeQuest.Status,
                QuestId      = activeQuest.Id,
                QuestKey     = activeQuest.BossQuest!.Key,
                QuestName    = activeQuest.BossQuest.Text,
                IsBossQuest  = activeQuest.BossQuest.IsBossQuest,
                LeaderName   = activeQuest.Leader?.Username,
                LeaderUserId = activeQuest.LeaderUserId,
                BossHpMax       = activeQuest.BossQuest.BossHp,
                BossHpRemaining = Math.Max(0, activeQuest.BossHpRemaining),
                RageMax         = activeQuest.BossQuest.RageValue,
                RageCurrent     = activeQuest.RageMeter,
                Members = activeQuest.Members.Select(m => new QuestMemberRsvp
                {
                    UserId   = m.UserId,
                    UserName = m.User?.Username ?? "?",
                    Avatar   = m.User?.Avatar,
                    Response = m.Response,
                }).ToList()
            };

            if (!activeQuest.BossQuest.IsBossQuest && activeQuest.BossQuest.CollectJson is not null)
            {
                questStatus.CollectRequired = System.Text.Json.JsonSerializer
                    .Deserialize<Dictionary<string, int>>(activeQuest.BossQuest.CollectJson);
                questStatus.CollectProgress = activeQuest.CollectProgressJson is not null
                    ? System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(activeQuest.CollectProgressJson)
                    : new();
            }
        }

        // Owned quest scrolls for the current user
        var ownedScrolls = await _context.UserInventoryItems
            .AsNoTracking()
            .Where(i => i.UserId == userId && i.GameItem!.Type == ItemType.QuestScroll && i.Quantity > 0)
            .Include(i => i.GameItem)
            .Select(i => i.GameItem!)
            .ToListAsync();

        return new PartyViewModel
        {
            Party          = party,
            Members        = members.Select(m => new PartyMemberEntry { Member = m, User = m.User! }).ToList(),
            Messages       = messages,
            PendingInvites = pendingInvites,
            IsLeader       = isLeader,
            Page           = page,
            HasMore        = messages.Count == PAGE_SIZE,
            QuestStatus    = questStatus,
            OwnedScrolls   = ownedScrolls,
        };
    }

    public async Task<(bool Success, string? Error)> InviteAsync(int inviterId, int targetUserId)
    {
        var myParty = await GetMyPartyAsync(inviterId);
        if (myParty == null) return (false, "You are not in a party.");

        var leaderMember = await _context.PartyMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.PartyId == myParty.Id && m.UserId == inviterId);
        if (leaderMember?.Role != "Leader")
            return (false, "Only the party leader can invite.");

        var target = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == targetUserId && !u.IsAdmin);
        if (target == null) return (false, "User not found.");

        if (await GetMyPartyAsync(target.Id) != null)
            return (false, "That user is already in a party.");

        var pendingExists = await _context.PartyInvites.AnyAsync(
            i => i.PartyId == myParty.Id && i.InviteeId == target.Id && i.Status == "Pending");
        if (pendingExists) return (false, "User already has a pending invite.");

        var inviter = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == inviterId);

        _context.PartyInvites.Add(new PartyInvite
        {
            PartyId   = myParty.Id,
            InviterId = inviterId,
            InviteeId = target.Id,
            Status    = "Pending",
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        await _notifications.CreateNotificationAsync(
            target.Id,
            $"Party invite from {inviter?.Username ?? "someone"}",
            $"You've been invited to join {myParty.Name}.",
            "Party",
            "/Party",
            "🛡️"
        );

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> AcceptInviteAsync(int userId, int inviteId)
    {
        var invite = await _context.PartyInvites
            .Include(i => i.Party)
            .FirstOrDefaultAsync(i => i.Id == inviteId && i.InviteeId == userId && i.Status == "Pending");
        if (invite == null) return (false, "Invite not found.");

        if (await GetMyPartyAsync(userId) != null)
        {
            invite.Status = "Declined";
            await _context.SaveChangesAsync();
            return (false, "You are already in a party.");
        }

        invite.Status = "Accepted";
        _context.PartyMembers.Add(new PartyMember
        {
            PartyId  = invite.PartyId,
            UserId   = userId,
            Role     = "Member",
            JoinedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeclineInviteAsync(int userId, int inviteId)
    {
        var invite = await _context.PartyInvites
            .FirstOrDefaultAsync(i => i.Id == inviteId && i.InviteeId == userId && i.Status == "Pending");
        if (invite == null) return (false, "Invite not found.");

        invite.Status = "Declined";
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> LeaveAsync(int userId)
    {
        var myParty = await GetMyPartyAsync(userId);
        if (myParty == null) return (false, "You are not in a party.");

        var member = await _context.PartyMembers
            .FirstOrDefaultAsync(m => m.PartyId == myParty.Id && m.UserId == userId);
        if (member == null) return (false, "Not a member.");

        if (member.Role == "Leader")
        {
            var next = await _context.PartyMembers
                .Where(m => m.PartyId == myParty.Id && m.UserId != userId)
                .OrderBy(m => m.JoinedAt)
                .FirstOrDefaultAsync();

            if (next == null)
            {
                // Last member — disband party
                var party = await _context.Parties.FindAsync(myParty.Id);
                if (party != null) _context.Parties.Remove(party);
                await _context.SaveChangesAsync();
                return (true, null);
            }

            next.Role = "Leader";
            var p = await _context.Parties.FindAsync(myParty.Id);
            if (p != null) p.LeaderId = next.UserId;
        }

        _context.PartyMembers.Remove(member);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> KickMemberAsync(int leaderId, int memberId)
    {
        var myParty = await GetMyPartyAsync(leaderId);
        if (myParty == null) return (false, "You are not in a party.");

        var leaderMember = await _context.PartyMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.PartyId == myParty.Id && m.UserId == leaderId);
        if (leaderMember?.Role != "Leader") return (false, "Only the leader can kick members.");

        var target = await _context.PartyMembers
            .FirstOrDefaultAsync(m => m.PartyId == myParty.Id && m.UserId == memberId);
        if (target == null) return (false, "User is not in your party.");
        if (target.Role == "Leader") return (false, "Cannot kick yourself.");

        _context.PartyMembers.Remove(target);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error, PartyMessage? Msg)> SendMessageAsync(int userId, string body)
    {
        var myParty = await GetMyPartyAsync(userId);
        if (myParty == null) return (false, "You are not in a party.", null);

        body = body.Trim();
        if (string.IsNullOrWhiteSpace(body)) return (false, "Message is empty.", null);
        if (body.Length > 2000) return (false, "Message exceeds 2000 characters.", null);

        var msg = new PartyMessage
        {
            PartyId  = myParty.Id,
            AuthorId = userId,
            Body     = body,
            SentAt   = DateTime.UtcNow
        };

        _context.PartyMessages.Add(msg);
        await _context.SaveChangesAsync();

        await ProcessMentionsAsync(body, myParty.Name, $"/Party", userId);

        return (true, null, msg);
    }

    public async Task<List<PartyMessageEntry>> GetMessagesAsync(int partyId, int userId, int page = 0)
    {
        var messages = await _context.PartyMessages
            .AsNoTracking()
            .Where(m => m.PartyId == partyId && !m.IsDeleted)
            .Include(m => m.Author)
            .Include(m => m.Likes)
            .OrderByDescending(m => m.SentAt)
            .Skip(page * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .ToListAsync();

        messages.Reverse();

        var mentionedNames = messages
            .SelectMany(m => Regex.Matches(m.Body, @"@(\w+)").Select(x => x.Groups[1].Value))
            .Distinct()
            .ToList();

        var mentionLookup = mentionedNames.Any()
            ? await _context.Users.AsNoTracking()
                .Where(u => mentionedNames.Contains(u.Username))
                .ToDictionaryAsync(u => u.Username, u => u.Id)
            : new Dictionary<string, int>();

        return messages.Select(m =>
        {
            bool isSys = m.Body.StartsWith("[SYS]");
            string displayBody = isSys ? m.Body[5..] : m.Body;
            return new PartyMessageEntry
            {
                Message      = m,
                LikedByMe    = m.Likes.Any(l => l.LikerUserId == userId),
                LikeCount    = m.Likes.Count,
                RenderedBody = RenderMentions(displayBody, mentionLookup),
                IsSystem     = isSys
            };
        }).ToList();
    }

    public async Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId)
    {
        var existing = await _context.PartyMessageLikes
            .FirstOrDefaultAsync(l => l.PartyMessageId == messageId && l.LikerUserId == userId);

        if (existing != null)
            _context.PartyMessageLikes.Remove(existing);
        else
            _context.PartyMessageLikes.Add(new PartyMessageLike
            {
                PartyMessageId = messageId,
                LikerUserId    = userId,
                LikedAt        = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
        var count = await _context.PartyMessageLikes.CountAsync(l => l.PartyMessageId == messageId);
        return (existing == null, count);
    }

    public async Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId)
    {
        var msg = await _context.PartyMessages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (msg == null) return (false, "Message not found.");

        // Only author or party leader can delete
        if (msg.AuthorId != userId)
        {
            var myParty = await GetMyPartyAsync(userId);
            var isLeader = myParty != null &&
                await _context.PartyMembers.AnyAsync(m => m.PartyId == myParty.Id && m.UserId == userId && m.Role == "Leader");
            if (!isLeader) return (false, "Not authorised.");
        }

        msg.IsDeleted = true;
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<List<PartyInvite>> GetPendingInvitesAsync(int userId)
    {
        return await _context.PartyInvites
            .AsNoTracking()
            .Where(i => i.InviteeId == userId && i.Status == "Pending")
            .Include(i => i.Party)
            .Include(i => i.Inviter)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task<List<PartyInvite>> GetPendingInvitesAsync(int leaderId, int partyId)
    {
        return await _context.PartyInvites
            .AsNoTracking()
            .Where(i => i.PartyId == partyId && i.Status == "Pending")
            .Include(i => i.Invitee)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    private async Task ProcessMentionsAsync(string body, string partyName, string link, int authorId)
    {
        var mentions = Regex.Matches(body, @"@(\w+)");
        foreach (Match match in mentions)
        {
            var username = match.Groups[1].Value;
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || user.Id == authorId) continue;

            await _notifications.CreateNotificationAsync(
                user.Id,
                $"You were mentioned in {partyName}",
                body.Length > 80 ? body[..80] + "…" : body,
                "Social",
                link,
                "@"
            );
        }
    }

    public async Task<string> RenderBodyAsync(string body)
    {
        var names = Regex.Matches(body, @"@(\w+)").Select(m => m.Groups[1].Value).Distinct().ToList();
        if (!names.Any()) return body;

        var lookup = (await _context.Users.AsNoTracking()
            .Where(u => names.Contains(u.Username))
            .Select(u => new { u.Username, u.Id })
            .ToListAsync())
            .GroupBy(u => u.Username, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

        return RenderMentions(body, lookup);
    }

    private static string RenderMentions(string body, Dictionary<string, int> userLookup)
    {
        return Regex.Replace(body, @"@(\w+)", m =>
        {
            var name = m.Groups[1].Value;
            return userLookup.TryGetValue(name, out var id)
                ? $"<a href=\"/Friend/ViewProfile/{id}\" class=\"mention fw-semibold\">@{name}</a>"
                : $"@{name}";
        });
    }
}
