using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Mô hình biểu thị badge được kiếm bởi user
    /// </summary>
    public class UserBadge
    {
        [Key]
        public int Id { get; set; }

        // ===== FOREIGN KEYS =====
        [Required]
        public int UserId { get; set; }

        [Required]
        public int BadgeId { get; set; }

        // ===== DATE INFO =====
        [Required]
        public DateTime EarnedDate { get; set; }

        // ===== NAVIGATION PROPERTIES =====
        [Required]
        public User? User { get; set; }

        [Required]
        public Badge? Badge { get; set; }

        // ===== METHODS =====
        /// <summary>
        /// Get display name
        /// </summary>
        public string GetDisplayName()
        {
            return Badge?.GetDisplayName() ?? "Unknown Badge";
        }

        /// <summary>
        /// Check nếu badge vừa được kiếm (trong 24h)
        /// </summary>
        public bool IsNewlyEarned()
        {
            return DateTime.UtcNow - EarnedDate < TimeSpan.FromHours(24);
        }
    }
}