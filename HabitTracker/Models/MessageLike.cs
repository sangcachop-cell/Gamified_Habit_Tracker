using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class MessageLike
{
    [Required]
    public int MessageId { get; set; }

    [Required]
    public int LikerUserId { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    public Message? Message { get; set; }
    public User? LikerUser { get; set; }
}
