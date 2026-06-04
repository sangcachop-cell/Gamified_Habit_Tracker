using System.Text.Json;
using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations;

public class BossQuestService : IBossQuestService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notify;
    private readonly IAchievementService _achievements;
    private readonly ILogger<BossQuestService> _logger;

    public BossQuestService(AppDbContext db, INotificationService notify,
                            IAchievementService achievements, ILogger<BossQuestService> logger)
    {
        _db           = db;
        _notify       = notify;
        _achievements = achievements;
        _logger       = logger;
    }

    // ─── Shop ────────────────────────────────────────────────────────────────

    public async Task<List<BossQuest>> GetShopQuestsAsync(int userId)
        => await _db.BossQuests.AsNoTracking().OrderBy(q => q.Category).ThenBy(q => q.Text).ToListAsync();

    public async Task<(bool Success, string? Error)> BuyScrollAsync(int userId, string questKey)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return (false, "User not found.");

        var quest = await _db.BossQuests.AsNoTracking().FirstOrDefaultAsync(q => q.Key == questKey);
        if (quest is null) return (false, "Quest not found.");
        if (quest.GemCost <= 0) return (false, "This scroll cannot be purchased.");
        if (user.Gems < quest.GemCost) return (false, $"Not enough gems. Need {quest.GemCost} 💎.");
        if (quest.GoldCost > 0 && user.Gold < quest.GoldCost) return (false, $"Not enough gold. Need {quest.GoldCost} GP.");

        user.Gems -= quest.GemCost;
        if (quest.GoldCost > 0) user.Gold -= quest.GoldCost;

        // Find the GameItem for this scroll
        var scrollItem = await _db.GameItems.AsNoTracking()
            .FirstOrDefaultAsync(i => i.Key == $"quest_{questKey}" && i.Type == ItemType.QuestScroll);
        if (scrollItem is null) return (false, "Scroll item not found in catalog.");

        var inv = await _db.UserInventoryItems
            .FirstOrDefaultAsync(i => i.UserId == userId && i.GameItemId == scrollItem.Id);
        if (inv is null)
            _db.UserInventoryItems.Add(new UserInventoryItem { UserId = userId, GameItemId = scrollItem.Id, Quantity = 1 });
        else
            inv.Quantity++;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    // ─── Lifecycle ───────────────────────────────────────────────────────────

    public async Task<(bool Success, string? Error)> InvitePartyAsync(int leaderId, string questKey)
    {
        var leader = await _db.Users.FindAsync(leaderId);
        if (leader is null) return (false, "User not found.");

        // Leader must be in a party and be the leader
        var membership = await _db.Set<PartyMember>()
            .Include(m => m.Party)
            .FirstOrDefaultAsync(m => m.UserId == leaderId);
        if (membership is null) return (false, "You are not in a party.");

        var party = membership.Party!;
        if (party.LeaderId != leaderId) return (false, "Only the party leader can start a quest.");

        // No active/pending quest already
        bool hasActive = await _db.PartyQuests
            .AnyAsync(pq => pq.PartyId == party.Id && (pq.Status == "Pending" || pq.Status == "Active"));
        if (hasActive) return (false, "Party already has an active or pending quest.");

        // Find quest definition
        var quest = await _db.BossQuests.AsNoTracking().FirstOrDefaultAsync(q => q.Key == questKey);
        if (quest is null) return (false, "Quest not found.");

        // Level check
        if (leader.Level < quest.LevelRequired)
            return (false, $"You must be level {quest.LevelRequired} to start this quest.");

        // Prereq check
        if (quest.PrerequisiteQuestKey is not null)
        {
            bool prereqDone = await _db.PartyQuests.AnyAsync(pq =>
                pq.LeaderUserId == leaderId &&
                pq.BossQuest.Key == quest.PrerequisiteQuestKey &&
                pq.Status == "Complete");
            if (!prereqDone)
                return (false, $"You must complete the prerequisite quest first.");
        }

        // Leader must own the scroll
        var scrollItem = await _db.GameItems.AsNoTracking()
            .FirstOrDefaultAsync(i => i.Key == $"quest_{questKey}" && i.Type == ItemType.QuestScroll);
        if (scrollItem is null) return (false, "Quest scroll not found.");

        var scrollInv = await _db.UserInventoryItems
            .FirstOrDefaultAsync(i => i.UserId == leaderId && i.GameItemId == scrollItem.Id && i.Quantity > 0);
        if (scrollInv is null) return (false, "You do not own this quest scroll.");

        // Consume scroll
        scrollInv.Quantity--;
        if (scrollInv.Quantity <= 0) _db.UserInventoryItems.Remove(scrollInv);

        // Create PartyQuest
        var partyQuest = new PartyQuest
        {
            PartyId          = party.Id,
            BossQuestId      = quest.Id,
            LeaderUserId     = leaderId,
            Status           = "Pending",
            CreatedAt        = DateTime.UtcNow,
            BossHpRemaining  = quest.BossHp ?? 0,
            RageMeter        = 0,
        };
        _db.PartyQuests.Add(partyQuest);
        await _db.SaveChangesAsync();

        // Create member rows for all party members
        var allMembers = await _db.Set<PartyMember>()
            .Where(m => m.PartyId == party.Id)
            .Select(m => m.UserId)
            .ToListAsync();

        foreach (var uid in allMembers)
        {
            _db.PartyQuestMembers.Add(new PartyQuestMember
            {
                PartyQuestId = partyQuest.Id,
                UserId       = uid,
                Response     = uid == leaderId ? "accepted" : null,
            });
        }
        await _db.SaveChangesAsync();

        // Notify all non-leader members
        foreach (var uid in allMembers.Where(u => u != leaderId))
        {
            await _notify.CreateNotificationAsync(uid,
                "Quest Invitation",
                $"{leader.Username} invited the party to: {quest.Text}",
                "quest_invite", "/Party", "⚔️");
        }

        // Auto-activate if solo party (all members already responded)
        await TryAutoActivateAsync(partyQuest.Id);

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> AcceptQuestAsync(int userId)
    {
        var member = await _db.PartyQuestMembers
            .Include(m => m.PartyQuest)
            .FirstOrDefaultAsync(m => m.UserId == userId && m.PartyQuest!.Status == "Pending");
        if (member is null) return (false, "No pending quest invitation found.");

        member.Response = "accepted";
        await _db.SaveChangesAsync();

        await TryAutoActivateAsync(member.PartyQuestId);
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> RejectQuestAsync(int userId)
    {
        var member = await _db.PartyQuestMembers
            .Include(m => m.PartyQuest)
            .FirstOrDefaultAsync(m => m.UserId == userId && m.PartyQuest!.Status == "Pending");
        if (member is null) return (false, "No pending quest invitation found.");

        member.Response = "rejected";
        await _db.SaveChangesAsync();

        await TryAutoActivateAsync(member.PartyQuestId);
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> ForceStartAsync(int leaderId)
    {
        var partyQuest = await _db.PartyQuests
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.LeaderUserId == leaderId && pq.Status == "Pending");
        if (partyQuest is null) return (false, "No pending quest found.");

        foreach (var m in partyQuest.Members.Where(m => m.Response is null))
            m.Response = "accepted";
        await _db.SaveChangesAsync();

        await ActivateQuestAsync(partyQuest.Id);
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> CancelQuestAsync(int leaderId)
    {
        var partyQuest = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.LeaderUserId == leaderId && pq.Status == "Pending");
        if (partyQuest is null) return (false, "No pending quest to cancel.");

        partyQuest.Status = "Aborted";

        // Return scroll to leader
        await ReturnScrollToLeaderAsync(leaderId, partyQuest.BossQuest!.Key);

        await _db.SaveChangesAsync();

        var memberIds = partyQuest.Members.Select(m => m.UserId).ToList();
        foreach (var uid in memberIds)
        {
            await _notify.CreateNotificationAsync(uid, "Quest Cancelled",
                $"The quest '{partyQuest.BossQuest.Text}' was cancelled.", "quest", "/Party", "🗑️");
        }
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> AbortQuestAsync(int leaderId)
    {
        var partyQuest = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.LeaderUserId == leaderId && pq.Status == "Active");
        if (partyQuest is null) return (false, "No active quest to abort.");

        partyQuest.Status       = "Aborted";
        partyQuest.CompletedAt  = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        var memberIds = partyQuest.Members.Select(m => m.UserId).ToList();
        foreach (var uid in memberIds)
        {
            await _notify.CreateNotificationAsync(uid, "Quest Aborted",
                $"The quest '{partyQuest.BossQuest!.Text}' was aborted. No rewards.", "quest", "/Party", "⚠️");
        }
        return (true, null);
    }

    // ─── Combat ──────────────────────────────────────────────────────────────

    public async Task ApplyTaskDamageAsync(int userId, double rawDelta, TaskType taskType, double critMult)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return;

        var partyMembership = await _db.Set<PartyMember>().FirstOrDefaultAsync(m => m.UserId == userId);
        if (partyMembership is null) return;

        var partyQuest = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.PartyId == partyMembership.PartyId && pq.Status == "Active");
        if (partyQuest is null) return;

        var questMember = partyQuest.Members.FirstOrDefault(m => m.UserId == userId && m.Response == "accepted");
        if (questMember is null) return;

        var questDef = partyQuest.BossQuest!;

        if (questDef.IsBossQuest)
        {
            double strBonus = taskType == TaskType.Habit
                ? 0.5 + user.STR / 400.0
                : 1.0 + user.STR / 200.0;
            double damage = rawDelta * critMult * strBonus / questDef.BossDef;

            partyQuest.BossHpRemaining -= damage;
            questMember.TotalDamageDealt += damage;

            _db.Set<PartyMessage>().Add(new PartyMessage
            {
                PartyId  = partyMembership.PartyId,
                AuthorId = userId,
                Body     = $"[SYS]⚔️ {user.Username} dealt {damage:F1} damage to {questDef.Text}! (HP: {Math.Max(0, partyQuest.BossHpRemaining):F0}/{questDef.BossHp:F0})",
                SentAt   = DateTime.UtcNow
            });

            if (partyQuest.BossHpRemaining <= 0)
            {
                await _db.SaveChangesAsync();
                await FinishQuestAsync(partyQuest.Id);
                return;
            }
        }
        else
        {
            // Collection quest: roll for item drop
            if (partyQuest.CollectProgressJson is null && questDef.CollectJson is not null)
                partyQuest.CollectProgressJson = "{}";

            if (questDef.CollectJson is not null)
            {
                var required = JsonSerializer.Deserialize<Dictionary<string, int>>(questDef.CollectJson)!;
                var progress = JsonSerializer.Deserialize<Dictionary<string, int>>(partyQuest.CollectProgressJson ?? "{}") ?? new();

                // Drop chance formula (Habitica approximation)
                double dropChance = Math.Min(0.75, (Math.Abs(rawDelta) / 10.0 + 0.02) * critMult);

                if (Random.Shared.NextDouble() < dropChance)
                {
                    // Pick a random item that isn't yet complete
                    var incomplete = required.Where(kv => progress.GetValueOrDefault(kv.Key, 0) < kv.Value).ToList();
                    if (incomplete.Count > 0)
                    {
                        var chosen = incomplete[Random.Shared.Next(incomplete.Count)].Key;
                        progress[chosen] = progress.GetValueOrDefault(chosen, 0) + 1;
                        partyQuest.CollectProgressJson = JsonSerializer.Serialize(progress);

                        // Check completion
                        bool allDone = required.All(kv => progress.GetValueOrDefault(kv.Key, 0) >= kv.Value);
                        if (allDone)
                        {
                            await _db.SaveChangesAsync();
                            await FinishQuestAsync(partyQuest.Id);
                            return;
                        }
                    }
                }
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task ApplyMissedDailyRageAsync(int userId, double rawDelta, double taskPriority)
    {
        var partyMembership = await _db.Set<PartyMember>().FirstOrDefaultAsync(m => m.UserId == userId);
        if (partyMembership is null) return;

        var partyQuest = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.PartyId == partyMembership.PartyId && pq.Status == "Active");
        if (partyQuest is null || partyQuest.BossQuest!.RageValue is null) return;

        var questMember = partyQuest.Members.FirstOrDefault(m => m.UserId == userId && m.Response == "accepted");
        if (questMember is null) return;

        double rage = Math.Abs(rawDelta) * taskPriority;
        partyQuest.RageMeter          += rage;
        questMember.TotalRageGiven    += rage;

        if (partyQuest.RageMeter >= partyQuest.BossQuest.RageValue)
        {
            await _db.SaveChangesAsync();
            await TriggerRageAsync(partyQuest.Id);
            return;
        }

        await _db.SaveChangesAsync();
    }

    // ─── View data ───────────────────────────────────────────────────────────

    public async Task<PartyQuest?> GetActiveQuestAsync(int partyId)
        => await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.PartyId == partyId &&
                                       (pq.Status == "Pending" || pq.Status == "Active"));

    public async Task<PartyQuestStatusDto?> GetQuestStatusAsync(int partyId)
    {
        var pq = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members).ThenInclude(m => m.User)
            .Include(pq => pq.Leader)
            .FirstOrDefaultAsync(pq => pq.PartyId == partyId &&
                                       (pq.Status == "Pending" || pq.Status == "Active"));

        if (pq is null) return new PartyQuestStatusDto { Status = "None" };

        var dto = new PartyQuestStatusDto
        {
            Status       = pq.Status,
            QuestId      = pq.Id,
            QuestKey     = pq.BossQuest!.Key,
            QuestName    = pq.BossQuest.Text,
            IsBossQuest  = pq.BossQuest.IsBossQuest,
            LeaderName   = pq.Leader?.Username,
            LeaderUserId = pq.LeaderUserId,
        };

        if (pq.BossQuest.IsBossQuest)
        {
            dto.BossHpMax       = pq.BossQuest.BossHp;
            dto.BossHpRemaining = Math.Max(0, pq.BossHpRemaining);
            if (pq.BossQuest.RageValue.HasValue)
            {
                dto.RageMax     = pq.BossQuest.RageValue;
                dto.RageCurrent = pq.RageMeter;
            }
        }
        else if (pq.BossQuest.CollectJson is not null)
        {
            dto.CollectRequired = JsonSerializer.Deserialize<Dictionary<string, int>>(pq.BossQuest.CollectJson);
            dto.CollectProgress = pq.CollectProgressJson is not null
                ? JsonSerializer.Deserialize<Dictionary<string, int>>(pq.CollectProgressJson)
                : new();
        }

        dto.Members = pq.Members.Select(m => new QuestMemberRsvp
        {
            UserId   = m.UserId,
            UserName = m.User?.Username ?? "?",
            Avatar   = m.User?.Avatar,
            Response = m.Response,
        }).ToList();

        return dto;
    }

    // ─── Private helpers ─────────────────────────────────────────────────────

    private async Task TryAutoActivateAsync(int partyQuestId)
    {
        var pq = await _db.PartyQuests
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.Id == partyQuestId && pq.Status == "Pending");
        if (pq is null) return;

        bool allResponded = pq.Members.All(m => m.Response is not null);
        if (!allResponded) return;

        bool anyRejected = pq.Members.Any(m => m.Response == "rejected");
        if (anyRejected)
        {
            // If EVERYONE rejected, cancel; otherwise still start with acceptors
            bool allRejected = pq.Members.All(m => m.Response == "rejected");
            if (allRejected)
            {
                pq.Status = "Aborted";
                await _db.SaveChangesAsync();
                return;
            }
        }

        await ActivateQuestAsync(partyQuestId);
    }

    private async Task ActivateQuestAsync(int partyQuestId)
    {
        var pq = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members).ThenInclude(m => m.User)
            .FirstOrDefaultAsync(pq => pq.Id == partyQuestId);
        if (pq is null) return;

        pq.Status   = "Active";
        pq.ActiveAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        foreach (var m in pq.Members.Where(m => m.Response == "accepted"))
        {
            await _notify.CreateNotificationAsync(m.UserId, "Quest Started!",
                $"Your party has begun: {pq.BossQuest!.Text}", "quest", "/Party", "⚔️");
        }
    }

    private async Task TriggerRageAsync(int partyQuestId)
    {
        var pq = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members).ThenInclude(m => m.User)
            .FirstOrDefaultAsync(pq => pq.Id == partyQuestId);
        if (pq is null) return;

        var questDef   = pq.BossQuest!;
        var acceptedIds = pq.Members.Where(m => m.Response == "accepted").Select(m => m.UserId).ToList();

        // Boss heals
        if (questDef.RageHealing.HasValue)
        {
            double heal = questDef.BossHp!.Value * questDef.RageHealing.Value;
            pq.BossHpRemaining = Math.Min(questDef.BossHp.Value, pq.BossHpRemaining + heal);
        }

        // Progress drain: boss "regains" HP proportional to damage already dealt
        if (questDef.RageProgressDrain.HasValue)
        {
            double alreadyDealt = questDef.BossHp!.Value - pq.BossHpRemaining;
            double drain        = alreadyDealt * questDef.RageProgressDrain.Value;
            pq.BossHpRemaining  = Math.Min(questDef.BossHp.Value, pq.BossHpRemaining + drain);
        }

        // Mana drain on accepted members
        if (questDef.RageMpDrain.HasValue)
        {
            var users = await _db.Users
                .Where(u => acceptedIds.Contains(u.Id))
                .ToListAsync();
            foreach (var u in users)
                u.Mana = Math.Max(0, u.Mana * (1.0 - questDef.RageMpDrain.Value));
        }

        pq.RageMeter = 0;
        await _db.SaveChangesAsync();

        // Notify party
        string rageText = questDef.RageEffect ?? $"{questDef.RageName} triggered!";
        foreach (var uid in acceptedIds)
        {
            await _notify.CreateNotificationAsync(uid, $"⚠️ {questDef.RageName}",
                rageText, "quest_rage", "/Party", "⚠️");
        }
    }

    public async Task FinishQuestAsync(int partyQuestId)
    {
        var pq = await _db.PartyQuests
            .Include(pq => pq.BossQuest)
            .Include(pq => pq.Members)
            .FirstOrDefaultAsync(pq => pq.Id == partyQuestId);
        if (pq is null) return;

        pq.Status      = "Complete";
        pq.CompletedAt = DateTime.UtcNow;

        var questDef      = pq.BossQuest!;
        var acceptedIds   = pq.Members.Where(m => m.Response == "accepted").Select(m => m.UserId).ToList();
        var acceptedUsers = await _db.Users.Where(u => acceptedIds.Contains(u.Id)).ToListAsync();

        // Parse drop items
        List<DropItem> drops = new();
        if (questDef.DropItemsJson is not null)
        {
            try { drops = JsonSerializer.Deserialize<List<DropItem>>(questDef.DropItemsJson) ?? new(); }
            catch { /* malformed JSON — skip drops */ }
        }

        foreach (var user in acceptedUsers)
        {
            user.Gold += questDef.DropGold;
            user.XP   += questDef.DropExp;
            // Level-up check (simple)
            while (user.XP >= user.Level * AppConstants.XP_PER_LEVEL && user.Level < AppConstants.MAX_LEVEL)
            {
                user.XP    -= user.Level * AppConstants.XP_PER_LEVEL;
                user.Level++;
                await _notify.CreateNotificationAsync(user.Id, "Level Up!",
                    $"You are now level {user.Level}!", "level_up", "/Character", "🎉");
            }

            // Non-owner drops go to all accepted members
            foreach (var drop in drops.Where(d => !d.OnlyOwner))
                await GrantDropItemAsync(user.Id, drop);
        }

        // Owner-only drops go to quest leader
        foreach (var drop in drops.Where(d => d.OnlyOwner))
            await GrantDropItemAsync(pq.LeaderUserId, drop);

        await _db.SaveChangesAsync();

        foreach (var user in acceptedUsers)
        {
            await _notify.CreateNotificationAsync(user.Id, "Quest Complete!",
                $"'{questDef.Text}' complete! +{questDef.DropGold}gp, +{questDef.DropExp}xp",
                "quest_complete", "/Party", "🏆");
            await _achievements.CheckQuestAsync(user.Id);
        }
    }

    private async Task GrantDropItemAsync(int userId, DropItem drop)
    {
        // Find the matching GameItem by type+key
        ItemType? itemType = drop.Type switch
        {
            "eggs"           => ItemType.Egg,
            "hatchingPotions"=> ItemType.HatchingPotion,
            "food"           => ItemType.Food,
            "quests"         => ItemType.QuestScroll,
            _                => null
        };
        if (itemType is null) return;

        string gameItemKey = itemType switch
        {
            ItemType.Egg            => $"egg_{drop.Key}",
            ItemType.HatchingPotion => $"potion_{drop.Key}",
            ItemType.Food           => $"food_{drop.Key}",
            ItemType.QuestScroll    => $"quest_{drop.Key}",
            _                       => drop.Key
        };

        var gameItem = await _db.GameItems.AsNoTracking()
            .FirstOrDefaultAsync(i => i.Key == gameItemKey && i.Type == itemType);
        if (gameItem is null) return;

        var inv = await _db.UserInventoryItems
            .FirstOrDefaultAsync(i => i.UserId == userId && i.GameItemId == gameItem.Id);
        if (inv is null)
            _db.UserInventoryItems.Add(new UserInventoryItem { UserId = userId, GameItemId = gameItem.Id, Quantity = 1 });
        else
            inv.Quantity++;
    }

    private async Task ReturnScrollToLeaderAsync(int leaderId, string questKey)
    {
        var scrollItem = await _db.GameItems.AsNoTracking()
            .FirstOrDefaultAsync(i => i.Key == $"quest_{questKey}" && i.Type == ItemType.QuestScroll);
        if (scrollItem is null) return;

        var inv = await _db.UserInventoryItems
            .FirstOrDefaultAsync(i => i.UserId == leaderId && i.GameItemId == scrollItem.Id);
        if (inv is null)
            _db.UserInventoryItems.Add(new UserInventoryItem { UserId = leaderId, GameItemId = scrollItem.Id, Quantity = 1 });
        else
            inv.Quantity++;
    }

    // ─── Internal DTOs for JSON deserialization ───────────────────────────────

    private sealed class DropItem
    {
        public string Type      { get; set; } = string.Empty;
        public string Key       { get; set; } = string.Empty;
        public int    Count     { get; set; } = 1;
        public bool   OnlyOwner { get; set; } = false;
    }
}
