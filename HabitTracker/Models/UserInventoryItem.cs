using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class UserInventoryItem
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemId { get; set; } = ""; // key into ItemCatalogue.Items

        [Required]
        [StringLength(20)]
        public string ContainerType { get; set; } = "Storage"; // "Storage" | "Backpack"

        public int GridX { get; set; } // 0-based column of top-left corner
        public int GridY { get; set; } // 0-based row of top-left corner

        public bool IsRotated { get; set; } = false;

        public DateTime AcquiredAt { get; set; }
    }
}
