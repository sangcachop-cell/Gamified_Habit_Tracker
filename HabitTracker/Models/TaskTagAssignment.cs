using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class TaskTagAssignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HabitTaskId { get; set; }

        [Required]
        public int UserTaskTagId { get; set; }

        public virtual HabitTask? HabitTask { get; set; }
        public virtual UserTaskTag? UserTaskTag { get; set; }
    }
}
