namespace HabitTracker.Models.ViewModels;

public class GuildViewModel
{
    public Guild Guild { get; set; } = null!;
    public List<GuildMemberEntry> Members { get; set; } = new();
    public List<GuildMessageEntry> Messages { get; set; } = new();
    public string? MyRole { get; set; } // null = not a member
    public bool IsMember { get; set; }
    public int Page { get; set; }
    public bool HasMore { get; set; }
}

public class GuildMemberEntry
{
    public GuildMember Member { get; set; } = null!;
    public User User { get; set; } = null!;
}

public class GuildMessageEntry
{
    public GuildMessage Message { get; set; } = null!;
    public bool LikedByMe { get; set; }
    public int LikeCount { get; set; }
    public string RenderedBody { get; set; } = string.Empty; // @mention links applied
}
