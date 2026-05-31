using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class Report
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ReporterId { get; set; }

    [Required]
    public int ReportedUserId { get; set; }

    public int? ReportedMessageId { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsResolved { get; set; } = false;

    public DateTime? ResolvedAt { get; set; }

    public int? ResolvedByAdminId { get; set; }

    public User? Reporter { get; set; }
    public User? ReportedUser { get; set; }
    public Message? ReportedMessage { get; set; }
}
