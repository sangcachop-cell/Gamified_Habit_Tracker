using HabitTracker.Models;

namespace HabitTracker.Models.ViewModels;

public class QuestShopViewModel
{
    public List<BossQuest> Quests { get; set; } = new();
    public Dictionary<string, int> OwnedScrolls { get; set; } = new();  // questKey → quantity
    public int UserGold { get; set; }
    public int UserGems { get; set; }
    public int UserLevel { get; set; }
    public HashSet<string> CompletedQuestKeys { get; set; } = new();
}

public class PartyQuestStatusDto
{
    public string Status { get; set; } = "None";  // "None"|"Pending"|"Active"|"Complete"|"Aborted"
    public int? QuestId { get; set; }
    public string? QuestKey { get; set; }
    public string? QuestName { get; set; }
    public bool IsBossQuest { get; set; }

    // Boss quest
    public double? BossHpMax { get; set; }
    public double? BossHpRemaining { get; set; }
    public double? RageMax { get; set; }
    public double? RageCurrent { get; set; }

    // Collection quest
    public Dictionary<string, int>? CollectRequired { get; set; }
    public Dictionary<string, int>? CollectProgress { get; set; }

    // Members RSVP (pending state)
    public List<QuestMemberRsvp> Members { get; set; } = new();

    public string? LeaderName { get; set; }
    public int? LeaderUserId { get; set; }
}

public class QuestMemberRsvp
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Response { get; set; }  // null=pending, "accepted", "rejected"
}
