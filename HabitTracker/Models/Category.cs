using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Quest Categories - Admin có thể tạo category custom
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(10)]
        public string? Color { get; set; } = "#007bff"; // Bootstrap primary blue

        [StringLength(10)]
        public string? Icon { get; set; } = "📁"; // Emoji icon

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int CreatedByUserId { get; set; } // Admin who created it

        // Relationships
        public virtual User? CreatedByUser { get; set; }
        public virtual List<Quest>? Quests { get; set; }
    }
}