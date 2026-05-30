namespace HabitTracker.Models.ViewModels;

public class PartyViewModel
{
    public Party? Party { get; set; }
    public List<PartyMemberEntry> Members { get; set; } = new();
    public List<PartyMessageEntry> Messages { get; set; } = new();
    public List<PartyInvite> PendingInvites { get; set; } = new();
    public bool IsLeader { get; set; }
    public int Page { get; set; }
    public bool HasMore { get; set; }

    // Phase 8: Boss Quests
    public PartyQuestStatusDto? QuestStatus { get; set; }
    public List<GameItem> OwnedScrolls { get; set; } = new();  // scrolls current user owns
}

public class PartyMemberEntry
{
    public PartyMember Member { get; set; } = null!;
    public User User { get; set; } = null!;
}

public class PartyMessageEntry
{
    public PartyMessage Message { get; set; } = null!;
    public bool LikedByMe { get; set; }
    public int LikeCount { get; set; }
    public string RenderedBody { get; set; } = string.Empty; // @mention links applied
    public bool IsSystem { get; set; }
}
