using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// In-app notifications
    /// </summary>
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Message { get; set; }

        [StringLength(50)]
        public string Type { get; set; } = "Info"; // Badge, Achievement, Streak, Quest, System

        [StringLength(200)]
        public string? Link { get; set; } // Link khi click notification

        [StringLength(10)]
        public string? Icon { get; set; } = "ℹ️";

        public bool IsRead { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ReadAt { get; set; }

        // Relationships
        public virtual User? User { get; set; }
    }
}