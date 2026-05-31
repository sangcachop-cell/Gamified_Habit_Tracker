namespace HabitTracker.Models;

public class GuildMessageLike
{
    public int GuildMessageId { get; set; }
    public int LikerUserId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    public virtual GuildMessage? GuildMessage { get; set; }
    public virtual User? LikerUser { get; set; }
}
