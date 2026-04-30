using HabitTracker.Constants;
using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Mô hình Quest (nhiệm vụ)
    /// </summary>
    public class Quest
    {
        [Key]
        public int Id { get; set; }

        // ===== BASIC INFO =====
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(10)]
        public string? Icon { get; set; }

        // ===== CLASSIFICATION =====
        // Dùng string Category cho system categories (từ AppConstants)
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = AppConstants.Categories.HEALTH;

        // Nếu muốn dùng custom category từ database, dùng CategoryId
        public int? CategoryId { get; set; } // Foreign key tới Category table

        // Navigation property cho custom categories
        public virtual Category? CustomCategory { get; set; }

        [Required]
        [StringLength(50)]
        public string Difficulty { get; set; } = AppConstants.Difficulty.EASY;

        [Required]
        [StringLength(50)]
        public string Frequency { get; set; } = AppConstants.Frequency.DAILY;

        // ===== REWARDS =====
        [Range(1, 1000)]
        public int XPReward { get; set; } = AppConstants.XPRewards.EASY;

        // ===== STATUS =====
        public bool IsActive { get; set; } = true;

        // ===== AUDIT =====
        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // ===== STATISTICS =====
        public int TimesCompleted { get; set; } = 0; // Track trending

        // ===== RELATIONSHIPS =====
        public virtual List<UserQuest>? UserQuests { get; set; }

        // Which hideout facility this quest is completed at (null = no specific facility)
        public int? FacilityId { get; set; }
        public virtual Facility? AssignedFacility { get; set; }

        // ===== METHODS =====
        /// <summary>
        /// Tính XP reward dựa vào difficulty
        /// </summary>
        public static int CalculateXPByDifficulty(string difficulty)
        {
            return difficulty switch
            {
                AppConstants.Difficulty.MEDIUM => AppConstants.XPRewards.MEDIUM,
                AppConstants.Difficulty.HARD => AppConstants.XPRewards.HARD,
                _ => AppConstants.XPRewards.EASY
            };
        }

        /// <summary>
        /// Get display name (icon + name)
        /// </summary>
        public string GetDisplayName()
        {
            return string.IsNullOrEmpty(Icon) ? Name : $"{Icon} {Name}";
        }

        /// <summary>
        /// Check if quest is completed by user today
        /// </summary>
        public bool IsCompletedByUserToday(int userId)
        {
            return UserQuests?.Any(uq =>
                uq.UserId == userId &&
                uq.CompletedDate == DateTime.Today) ?? false;
        }

        /// <summary>
        /// Get category name (system or custom)
        /// </summary>
        public string GetCategoryName()
        {
            return CustomCategory?.Name ?? Category;
        }
    }
}