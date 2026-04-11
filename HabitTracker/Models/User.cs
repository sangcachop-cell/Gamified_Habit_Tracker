using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Mô hình người dùng
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        // ===== ACCOUNT INFO =====
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        // ===== AVATAR =====
        [StringLength(255)]
        public string? Avatar { get; set; }

        // ===== GAMIFICATION =====
        public int Level { get; set; } = 1;

        public int XP { get; set; } = 0;

        public int CurrentStreak { get; set; } = 0;

        public int LongestStreak { get; set; } = 0;

        public DateTime? LastCheckInDate { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        // ===== PROFILE INFORMATION =====
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        // ===== SOCIAL LINKS =====
        [StringLength(255)]
        public string? FacebookLink { get; set; }

        [StringLength(255)]
        public string? LinkedInLink { get; set; }

        [StringLength(255)]
        public string? InstagramLink { get; set; }

        // ===== ADMIN & AUDIT =====
        public bool IsAdmin { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        // ===== RELATIONSHIPS =====
        public List<UserBadge>? UserBadges { get; set; }

        public List<UserQuest>? UserQuests { get; set; }

        // ===== METHODS =====
        /// <summary>
        /// Check if user has a specific badge
        /// </summary>
        public bool HasBadge(int badgeId)
        {
            return UserBadges?.Any(ub => ub.BadgeId == badgeId) ?? false;
        }

        /// <summary>
        /// Get user's earned badges
        /// </summary>
        public List<Badge> GetEarnedBadges()
        {
            return UserBadges?
                .Select(ub => ub.Badge)
                .Where(b => b != null)
                .ToList() ?? new List<Badge>();
        }

        /// <summary>
        /// Get number of quests completed today
        /// </summary>
        public int GetCompletedQuestsTodayCount()
        {
            return UserQuests?
                .Count(uq => uq.CompletedDate == DateTime.Today) ?? 0;
        }

        [StringLength(255)]
        public string? CoverImage { get; set; }

        [StringLength(255)]
        public string? FacebookUrl { get; set; }

        [StringLength(255)]
        public string? InstagramUrl { get; set; }

        [StringLength(255)]
        public string? LinkedInUrl { get; set; }

        [StringLength(255)]
        public string? TwitterUrl { get; set; }

        // Gamification
        public int TotalQuestsCompleted { get; set; } = 0;
        public int TotalXPEarned { get; set; } = 0;
        public DateTime? LastActiveDate { get; set; }

        // Relationships
        public virtual List<Notification>? Notifications { get; set; }
        public virtual List<Category>? CreatedCategories { get; set; }
    }
}