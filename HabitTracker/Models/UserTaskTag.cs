using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class UserTaskTag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(10)]
        public string? Color { get; set; }

        public virtual User? User { get; set; }
        public virtual List<TaskTagAssignment>? TaskAssignments { get; set; }
    }
}
