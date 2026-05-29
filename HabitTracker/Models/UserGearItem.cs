using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Join table — gear items owned by a user.
    /// Equipped slots are stored as string Keys on User, not FKs here.
    /// </summary>
    public class UserGearItem
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int GearItemId { get; set; }
        public virtual GearItem GearItem { get; set; } = null!;

        public DateTime AcquiredAt { get; set; } = DateTime.UtcNow;
    }
}
