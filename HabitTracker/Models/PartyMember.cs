using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class PartyMember
{
    public int Id { get; set; }
    public int PartyId { get; set; }
    public int UserId { get; set; }

    [MaxLength(20)]
    public string Role { get; set; } = "Member"; // "Leader" | "Member"

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public virtual Party? Party { get; set; }
    public virtual User? User { get; set; }
}
