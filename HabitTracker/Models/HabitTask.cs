using HabitTracker.Constants;
using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class HabitTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        // ===== COMMON =====
        public TaskType Type { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Easy;

        public double Value { get; set; } = 0.0;

        public int SortOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // ===== HABIT-SPECIFIC =====
        public bool? Up { get; set; }
        public bool? Down { get; set; }
        public int CounterUp { get; set; } = 0;
        public int CounterDown { get; set; } = 0;
        public HabitResetFrequency? HabitFrequency { get; set; }

        // ===== DAILY-SPECIFIC =====
        public DailyFrequency? DailyFrequency { get; set; }
        public int EveryX { get; set; } = 1;
        public DateTime? StartDate { get; set; }
        public bool RepeatMonday    { get; set; } = true;
        public bool RepeatTuesday   { get; set; } = true;
        public bool RepeatWednesday { get; set; } = true;
        public bool RepeatThursday  { get; set; } = true;
        public bool RepeatFriday    { get; set; } = true;
        public bool RepeatSaturday  { get; set; } = false;
        public bool RepeatSunday    { get; set; } = false;
        public int Streak { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;

        // ===== TODO-SPECIFIC =====
        public DateTime? DueDate { get; set; }
        public DateTime? DateCompleted { get; set; }

        // ===== REWARD-SPECIFIC =====
        public double GoldCost { get; set; } = 10.0;

        // ===== NAVIGATION =====
        public virtual User? User { get; set; }
        public virtual List<TaskChecklistItem>? ChecklistItems { get; set; }
        public virtual List<TaskTagAssignment>? TagAssignments { get; set; }
    }
}
