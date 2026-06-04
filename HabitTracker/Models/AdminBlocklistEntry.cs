using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class AdminBlocklistEntry
    {
        [Key]
        public int Id { get; set; }

        /// <summary>"email" | "ip"</summary>
        [Required]
        [StringLength(10)]
        public string Type { get; set; } = "email";

        [Required]
        [StringLength(255)]
        public string Value { get; set; } = "";

        [StringLength(500)]
        public string? Note { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public int? AddedByAdminId { get; set; }
    }
}
