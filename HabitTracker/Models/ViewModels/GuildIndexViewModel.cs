namespace HabitTracker.Models.ViewModels;

public class GuildIndexViewModel
{
    public List<GuildCardModel> PublicGuilds { get; set; } = new();
    public List<GuildCardModel> MyGuilds { get; set; } = new();
    public List<GuildInvite> PendingInvites { get; set; } = new();
    public string? Search { get; set; }
    public string ActiveTab { get; set; } = "discover"; // "discover" | "my-guilds"
}

public class GuildCardModel
{
    public Guild Guild { get; set; } = null!;
    public int MemberCount { get; set; }
    public bool IsMember { get; set; }
    public string? MyRole { get; set; }
}
