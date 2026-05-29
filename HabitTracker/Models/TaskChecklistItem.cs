using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class TaskChecklistItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HabitTaskId { get; set; }

        [Required]
        [StringLength(300)]
        public string Text { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public virtual HabitTask? HabitTask { get; set; }
    }
}
