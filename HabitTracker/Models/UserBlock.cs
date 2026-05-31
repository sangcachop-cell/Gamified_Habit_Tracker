using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class UserBlock
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int BlockerId { get; set; }

    [Required]
    public int BlockedId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Blocker { get; set; }
    public User? Blocked { get; set; }
}
