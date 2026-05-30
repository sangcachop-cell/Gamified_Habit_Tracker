using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HabitTracker.Services.Implementations;

public class GuildService : IGuildService
{
    private readonly AppDbContext _context;
    private readonly INotificationService _notifications;
    private const int PAGE_SIZE = 20;

    public GuildService(AppDbContext context, INotificationService notifications)
    {
        _context = context;
        _notifications = notifications;
    }

    public async Task<(bool Success, string? Error, Guild? Guild)> CreateAsync(
        int leaderId, string name, string? description, string? summary, string privacy)
    {
        name = name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return (false, "Guild name is required.", null);
        if (name.Length > 100)
            return (false, "Name exceeds 100 characters.", null);

        var exists = await _context.Guilds.AsNoTracking().AnyAsync(g => g.Name == name);
        if (exists)
            return (false, "A guild with that name already exists.", null);

        var guild = new Guild
        {
            Name        = name,
            Description = description?.Trim(),
            Summary     = summary?.Trim(),
            Privacy     = privacy == "private" ? "private" : "public",
            LeaderId    = leaderId,
            CreatedAt   = DateTime.UtcNow
        };

        _context.Guilds.Add(guild);
        await _context.SaveChangesAsync();

        _context.GuildMembers.Add(new GuildMember
        {
            GuildId  = guild.Id,
            UserId   = leaderId,
            Role     = "Leader",
            JoinedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        return (true, null, guild);
    }

    public async Task<List<Guild>> GetPublicGuildsAsync(string? search = null)
    {
        var query = _context.Guilds.AsNoTracking()
            .Where(g => g.Privacy == "public");

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(g => g.Name.Contains(search) || (g.Summary != null && g.Summary.Contains(search)));

        return await query.OrderBy(g => g.Name).ToListAsync();
    }

    public async Task<List<GuildCardModel>> GetMyGuildsAsync(int userId)
    {
        var memberships = await _context.GuildMembers
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .Include(m => m.Guild)
            .ToListAsync();

        var guildIds = memberships.Select(m => m.GuildId).ToList();
        var counts = await _context.GuildMembers
            .AsNoTracking()
            .Where(m => guildIds.Contains(m.GuildId))
            .GroupBy(m => m.GuildId)
            .Select(g => new { GuildId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.GuildId, x => x.Count);

        return memberships
            .Where(m => m.Guild != null)
            .Select(m => new GuildCardModel
            {
                Guild       = m.Guild!,
                MemberCount = counts.TryGetValue(m.GuildId, out var c) ? c : 1,
                IsMember    = true,
                MyRole      = m.Role
            })
            .ToList();
    }

    public async Task<GuildViewModel?> GetGuildViewAsync(int guildId, int userId, int page = 0)
    {
        var guild = await _context.Guilds.AsNoTracking().FirstOrDefaultAsync(g => g.Id == guildId);
        if (guild == null) return null;

        var members = await _context.GuildMembers
            .AsNoTracking()
            .Where(m => m.GuildId == guildId)
            .Include(m => m.User)
            .OrderBy(m => m.Role == "Leader" ? 0 : m.Role == "Manager" ? 1 : 2)
            .ThenBy(m => m.JoinedAt)
            .ToListAsync();

        var messages = await GetMessagesAsync(guildId, userId, page);
        var myRole   = members.FirstOrDefault(m => m.UserId == userId)?.Role;

        return new GuildViewModel
        {
            Guild    = guild,
            Members  = members.Select(m => new GuildMemberEntry { Member = m, User = m.User! }).ToList(),
            Messages = messages,
            MyRole   = myRole,
            IsMember = myRole != null,
            Page     = page,
            HasMore  = messages.Count == PAGE_SIZE
        };
    }

    public async Task<(bool Success, string? Error)> JoinPublicAsync(int userId, int guildId)
    {
        var guild = await _context.Guilds.AsNoTracking().FirstOrDefaultAsync(g => g.Id == guildId);
        if (guild == null) return (false, "Guild not found.");
        if (guild.Privacy != "public") return (false, "This guild is private. You need an invitation.");

        if (await _context.GuildMembers.AnyAsync(m => m.GuildId == guildId && m.UserId == userId))
            return (false, "Already a member.");

        _context.GuildMembers.Add(new GuildMember
        {
            GuildId  = guildId,
            UserId   = userId,
            Role     = "Member",
            JoinedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> LeaveAsync(int userId, int guildId)
    {
        var member = await _context.GuildMembers
            .FirstOrDefaultAsync(m => m.GuildId == guildId && m.UserId == userId);
        if (member == null) return (false, "Not a member.");

        if (member.Role == "Leader")
        {
            // Transfer leadership to oldest manager, then oldest member
            var next = await _context.GuildMembers
                .Where(m => m.GuildId == guildId && m.UserId != userId)
                .OrderBy(m => m.Role == "Manager" ? 0 : 1)
                .ThenBy(m => m.JoinedAt)
                .FirstOrDefaultAsync();

            if (next == null)
            {
                // Last member — delete the guild
                var guild = await _context.Guilds.FindAsync(guildId);
                if (guild != null) _context.Guilds.Remove(guild);
                await _context.SaveChangesAsync();
                return (true, null);
            }

            next.Role = "Leader";
            var g = await _context.Guilds.FindAsync(guildId);
            if (g != null) { g.LeaderId = next.UserId; g.UpdatedAt = DateTime.UtcNow; }
        }

        _context.GuildMembers.Remove(member);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> InviteAsync(int inviterId, int guildId, int targetUserId)
    {
        var inviterRole = await GetRoleAsync(inviterId, guildId);
        if (inviterRole is not ("Leader" or "Manager"))
            return (false, "Only leaders or managers can invite.");

        var target = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == targetUserId && !u.IsAdmin);
        if (target == null) return (false, "User not found.");

        if (await IsMemberAsync(target.Id, guildId))
            return (false, "User is already a member.");

        var pendingExists = await _context.GuildInvites.AnyAsync(
            i => i.GuildId == guildId && i.InviteeId == target.Id && i.Status == "Pending");
        if (pendingExists) return (false, "User already has a pending invite.");

        var guild = await _context.Guilds.AsNoTracking().FirstOrDefaultAsync(g => g.Id == guildId);
        var inviter = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == inviterId);

        _context.GuildInvites.Add(new GuildInvite
        {
            GuildId   = guildId,
            InviterId = inviterId,
            InviteeId = target.Id,
            Status    = "Pending",
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        await _notifications.CreateNotificationAsync(
            target.Id,
            $"Guild invite from {inviter?.Username ?? "someone"}",
            $"You've been invited to join {guild?.Name ?? "a guild"}.",
            "Guild",
            "/Guild",
            "⚔️"
        );

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> AcceptInviteAsync(int userId, int inviteId)
    {
        var invite = await _context.GuildInvites
            .Include(i => i.Guild)
            .FirstOrDefaultAsync(i => i.Id == inviteId && i.InviteeId == userId && i.Status == "Pending");
        if (invite == null) return (false, "Invite not found.");

        if (await IsMemberAsync(userId, invite.GuildId))
        {
            invite.Status = "Accepted";
            await _context.SaveChangesAsync();
            return (false, "Already a member.");
        }

        invite.Status = "Accepted";
        _context.GuildMembers.Add(new GuildMember
        {
            GuildId  = invite.GuildId,
            UserId   = userId,
            Role     = "Member",
            JoinedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeclineInviteAsync(int userId, int inviteId)
    {
        var invite = await _context.GuildInvites
            .FirstOrDefaultAsync(i => i.Id == inviteId && i.InviteeId == userId && i.Status == "Pending");
        if (invite == null) return (false, "Invite not found.");

        invite.Status = "Declined";
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> KickMemberAsync(int actorId, int guildId, int memberId)
    {
        var actorRole  = await GetRoleAsync(actorId, guildId);
        var targetRole = await GetRoleAsync(memberId, guildId);

        if (actorRole is not ("Leader" or "Manager"))
            return (false, "Only leaders or managers can kick members.");
        if (targetRole == null)
            return (false, "User is not a member.");
        if (targetRole == "Leader")
            return (false, "Cannot kick the guild leader.");
        if (targetRole == "Manager" && actorRole != "Leader")
            return (false, "Only the leader can kick managers.");

        var member = await _context.GuildMembers
            .FirstOrDefaultAsync(m => m.GuildId == guildId && m.UserId == memberId);
        if (member != null)
        {
            _context.GuildMembers.Remove(member);
            await _context.SaveChangesAsync();
        }
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> PromoteManagerAsync(int leaderId, int guildId, int memberId)
    {
        if (await GetRoleAsync(leaderId, guildId) != "Leader")
            return (false, "Only the leader can promote members.");

        var member = await _context.GuildMembers
            .FirstOrDefaultAsync(m => m.GuildId == guildId && m.UserId == memberId);
        if (member == null) return (false, "User is not a member.");
        if (member.Role == "Leader") return (false, "Cannot promote the leader.");

        member.Role = "Manager";
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DemoteManagerAsync(int leaderId, int guildId, int memberId)
    {
        if (await GetRoleAsync(leaderId, guildId) != "Leader")
            return (false, "Only the leader can demote managers.");

        var member = await _context.GuildMembers
            .FirstOrDefaultAsync(m => m.GuildId == guildId && m.UserId == memberId);
        if (member == null) return (false, "User is not a member.");
        if (member.Role != "Manager") return (false, "User is not a manager.");

        member.Role = "Member";
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error, GuildMessage? Msg)> SendMessageAsync(
        int userId, int guildId, string body)
    {
        if (!await IsMemberAsync(userId, guildId))
            return (false, "You must be a member to chat.", null);

        body = body.Trim();
        if (string.IsNullOrWhiteSpace(body)) return (false, "Message is empty.", null);
        if (body.Length > 2000) return (false, "Message exceeds 2000 characters.", null);

        var msg = new GuildMessage
        {
            GuildId  = guildId,
            AuthorId = userId,
            Body     = body,
            SentAt   = DateTime.UtcNow
        };

        _context.GuildMessages.Add(msg);
        await _context.SaveChangesAsync();

        var guild = await _context.Guilds.AsNoTracking().FirstOrDefaultAsync(g => g.Id == guildId);
        await ProcessMentionsAsync(body, guild?.Name ?? "a guild", $"/Guild/View/{guildId}", userId);

        return (true, null, msg);
    }

    public async Task<List<GuildMessageEntry>> GetMessagesAsync(int guildId, int userId, int page = 0)
    {
        var messages = await _context.GuildMessages
            .AsNoTracking()
            .Where(m => m.GuildId == guildId && !m.IsDeleted)
            .Include(m => m.Author)
            .Include(m => m.Likes)
            .OrderByDescending(m => m.SentAt)
            .Skip(page * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .ToListAsync();

        messages.Reverse();

        // Collect all @mentioned usernames across bodies to batch-resolve IDs
        var mentionedNames = messages
            .SelectMany(m => Regex.Matches(m.Body, @"@(\w+)").Select(x => x.Groups[1].Value))
            .Distinct()
            .ToList();

        var mentionLookup = mentionedNames.Any()
            ? (await _context.Users.AsNoTracking()
                .Where(u => mentionedNames.Contains(u.Username))
                .Select(u => new { u.Username, u.Id })
                .ToListAsync())
                .GroupBy(u => u.Username, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        return messages.Select(m => new GuildMessageEntry
        {
            Message      = m,
            LikedByMe    = m.Likes.Any(l => l.LikerUserId == userId),
            LikeCount    = m.Likes.Count,
            RenderedBody = RenderMentions(m.Body, mentionLookup)
        }).ToList();
    }

    public async Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId)
    {
        var existing = await _context.GuildMessageLikes
            .FirstOrDefaultAsync(l => l.GuildMessageId == messageId && l.LikerUserId == userId);

        if (existing != null)
            _context.GuildMessageLikes.Remove(existing);
        else
            _context.GuildMessageLikes.Add(new GuildMessageLike
            {
                GuildMessageId = messageId,
                LikerUserId    = userId,
                LikedAt        = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
        var count = await _context.GuildMessageLikes.CountAsync(l => l.GuildMessageId == messageId);
        return (existing == null, count);
    }

    public async Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId)
    {
        var msg = await _context.GuildMessages
            .Include(m => m.Guild)
            .FirstOrDefaultAsync(m => m.Id == messageId);
        if (msg == null) return (false, "Message not found.");

        var role = await GetRoleAsync(userId, msg.GuildId);
        if (msg.AuthorId != userId && role is not ("Leader" or "Manager"))
            return (false, "Not authorised.");

        msg.IsDeleted = true;
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<List<GuildInvite>> GetPendingInvitesAsync(int userId)
    {
        return await _context.GuildInvites
            .AsNoTracking()
            .Where(i => i.InviteeId == userId && i.Status == "Pending")
            .Include(i => i.Guild)
            .Include(i => i.Inviter)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IsMemberAsync(int userId, int guildId)
    {
        return await _context.GuildMembers
            .AsNoTracking()
            .AnyAsync(m => m.GuildId == guildId && m.UserId == userId);
    }

    public async Task<string?> GetRoleAsync(int userId, int guildId)
    {
        var member = await _context.GuildMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.GuildId == guildId && m.UserId == userId);
        return member?.Role;
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

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task ProcessMentionsAsync(string body, string groupName, string groupLink, int authorId)
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
                $"You were mentioned in {groupName}",
                body.Length > 80 ? body[..80] + "…" : body,
                "Social",
                groupLink,
                "@"
            );
        }
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
