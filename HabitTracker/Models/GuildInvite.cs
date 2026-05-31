using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class GuildInvite
{
    public int Id { get; set; }
    public int GuildId { get; set; }
    public int InviterId { get; set; }
    public int InviteeId { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // "Pending" | "Accepted" | "Declined"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Guild? Guild { get; set; }
    public virtual User? Inviter { get; set; }
    public virtual User? Invitee { get; set; }
}
