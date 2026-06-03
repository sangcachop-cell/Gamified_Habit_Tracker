using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class Badge
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Key { get; set; } = "";

        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(10)]
        public string? Icon { get; set; }

        // "Streak" | "TaskMilestone" | "PerfectDay" | "UltimateGear" | "Quest" | "Stable" | "Social"
        [StringLength(50)]
        public string TriggerType { get; set; } = "";

        // Threshold for counter-based achievements (0 = one-time / no threshold)
        public int TriggerValue { get; set; } = 0;

        [StringLength(20)]
        public string Rarity { get; set; } = "Common";

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<UserBadge>? UserBadges { get; set; }

        public string GetRarityColor() => Rarity switch
        {
            "Common"    => "#808080",
            "Rare"      => "#0070DD",
            "Epic"      => "#A335EE",
            "Legendary" => "#FF8000",
            _           => "#808080"
        };
    }
}
