using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class UserInventoryItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GameItemId { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime ObtainedAt { get; set; } = DateTime.UtcNow;

        public virtual User?     User     { get; set; }
        public virtual GameItem? GameItem { get; set; }
    }
}
