using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Mô hình biểu thị quest được hoàn thành bởi user
    /// </summary>
    public class UserQuest
    {
        [Key]
        public int Id { get; set; }

        // ===== FOREIGN KEYS =====
        [Required]
        public int UserId { get; set; }

        [Required]
        public int QuestId { get; set; }

        // ===== COMPLETION INFO =====
        [Required]
        public DateTime CompletedDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Confirmed"; // "Confirmed" hoặc "Pending"

        // ===== XP EARNED (snapshot của XP lúc hoàn thành) =====
        public int XPEarned { get; set; } = 0;

        // ===== AUDIT =====
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ===== NAVIGATION PROPERTIES =====
        [Required]
        public User? User { get; set; }

        [Required]
        public Quest? Quest { get; set; }

        // ===== METHODS =====
        /// <summary>
        /// Check nếu quest hoàn thành hôm nay
        /// </summary>
        public bool IsCompletedToday()
        {
            return CompletedDate == DateTime.Today && Status == "Confirmed";
        }

        /// <summary>
        /// Get display name
        /// </summary>
        public string GetDisplayName()
        {
            return Quest?.GetDisplayName() ?? "Unknown Quest";
        }
    }
}