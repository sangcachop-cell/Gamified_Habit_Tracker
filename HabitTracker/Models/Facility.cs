using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class Facility
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Icon { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Which derived stat this facility buffs: "ATK", "HP", "Armor", "XPGain", "Stamina"
        [StringLength(20)]
        public string StatAffected { get; set; }

        // Human-readable buff line shown on the card, e.g. "+5 ATK per level"
        [StringLength(100)]
        public string BuffDescription { get; set; }

        // Flat buff added per facility level
        public int BuffPerLevel { get; set; }

        public int MaxLevel { get; set; } = 5;

        public bool IsActive { get; set; } = true;

        public virtual List<UserFacility>? UserFacilities { get; set; }
    }
}
