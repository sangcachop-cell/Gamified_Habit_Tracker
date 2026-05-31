using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class Party
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int LeaderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? Leader { get; set; }
    public virtual ICollection<PartyMember> Members { get; set; } = new List<PartyMember>();
    public virtual ICollection<PartyMessage> Messages { get; set; } = new List<PartyMessage>();
    public virtual ICollection<PartyInvite> Invites { get; set; } = new List<PartyInvite>();
}
