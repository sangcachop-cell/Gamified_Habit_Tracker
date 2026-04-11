using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Mô hình Badge (huy hiệu)
    /// </summary>
    public class Badge
    {
        [Key]
        public int Id { get; set; }

        // ===== BASIC INFO =====
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(10)]
        public string Icon { get; set; }

        // ===== REQUIREMENT =====
        [Range(0, 100000)]
        public int RequiredXP { get; set; }

        // ===== RARITY =====
        [StringLength(20)]
        public string Rarity { get; set; } = "Common"; // "Common", "Rare", "Epic", "Legendary"

        // ===== STATUS =====
        public bool IsActive { get; set; } = true;

        // ===== AUDIT =====
        [Required]
        public DateTime CreatedAt { get; set; }

        // ===== RELATIONSHIPS =====
        public List<UserBadge>? UserBadges { get; set; }

        // ===== METHODS =====
        /// <summary>
        /// Check nếu user đủ điều kiện nhận badge
        /// </summary>
        public bool IsEarnedByUserXP(int userXP)
        {
            return userXP >= RequiredXP;
        }

        /// <summary>
        /// Get display name
        /// </summary>
        public string GetDisplayName()
        {
            return $"{Icon} {Name}";
        }

        /// <summary>
        /// Get rarity color
        /// </summary>
        public string GetRarityColor()
        {
            return Rarity switch
            {
                "Common" => "#808080",      // Gray
                "Rare" => "#0070DD",        // Blue
                "Epic" => "#A335EE",        // Purple
                "Legendary" => "#FF8000",   // Orange
                _ => "#808080"
            };
        }
    }
}