namespace HabitTracker.Models;

public class PartyMessageLike
{
    public int PartyMessageId { get; set; }
    public int LikerUserId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    public virtual PartyMessage? PartyMessage { get; set; }
    public virtual User? LikerUser { get; set; }
}
