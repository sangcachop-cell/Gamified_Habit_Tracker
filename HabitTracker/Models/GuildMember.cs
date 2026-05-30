using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class GuildMember
{
    public int Id { get; set; }
    public int GuildId { get; set; }
    public int UserId { get; set; }

    [MaxLength(20)]
    public string Role { get; set; } = "Member"; // "Leader" | "Manager" | "Member"

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public virtual Guild? Guild { get; set; }
    public virtual User? User { get; set; }
}
