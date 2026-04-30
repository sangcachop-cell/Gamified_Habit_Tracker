using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class UserFacility
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int FacilityId { get; set; }

        public int Level { get; set; } = 1;

        public DateTime UnlockedAt { get; set; }

        // Set when upgrade begins; null = idle
        public DateTime? UpgradeStartedAt { get; set; }

        public virtual User Facility_User { get; set; }
        public virtual Facility Facility { get; set; }
    }
}
