using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class Guild
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? Summary { get; set; }

    [MaxLength(255)]
    public string? Logo { get; set; }

    [MaxLength(10)]
    public string Privacy { get; set; } = "public"; // "public" | "private"

    public int LeaderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual User? Leader { get; set; }
    public virtual ICollection<GuildMember> Members { get; set; } = new List<GuildMember>();
    public virtual ICollection<GuildMessage> Messages { get; set; } = new List<GuildMessage>();
    public virtual ICollection<GuildInvite> Invites { get; set; } = new List<GuildInvite>();
}
