using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models;

public class Message
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SenderId { get; set; }

    [Required]
    public int ReceiverId { get; set; }

    [Required]
    [StringLength(2000)]
    public string Body { get; set; } = "";

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public bool IsRead { get; set; } = false;

    public DateTime? ReadAt { get; set; }

    public bool DeletedBySender { get; set; } = false;

    public bool DeletedByReceiver { get; set; } = false;

    public User? Sender { get; set; }
    public User? Receiver { get; set; }
    public ICollection<MessageLike> Likes { get; set; } = new List<MessageLike>();

    [NotMapped]
    public int LikeCount => Likes.Count;
}
