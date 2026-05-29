using HabitTracker.Constants;
using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models.ViewModels
{
    public class CreateTaskViewModel
    {
        [Required]
        public TaskType Type { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Task name")]
        public string Text { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Easy;

        // Habit
        public bool Up   { get; set; } = true;
        public bool Down { get; set; } = true;
        public HabitResetFrequency HabitFrequency { get; set; } = HabitResetFrequency.Daily;

        // Daily
        public DailyFrequency DailyFrequency { get; set; } = DailyFrequency.Weekly;
        public int EveryX { get; set; } = 1;
        public DateTime? StartDate { get; set; }
        public bool RepeatMonday    { get; set; } = true;
        public bool RepeatTuesday   { get; set; } = true;
        public bool RepeatWednesday { get; set; } = true;
        public bool RepeatThursday  { get; set; } = true;
        public bool RepeatFriday    { get; set; } = true;
        public bool RepeatSaturday  { get; set; } = false;
        public bool RepeatSunday    { get; set; } = false;

        // Todo
        public DateTime? DueDate { get; set; }

        // Reward
        [Range(1, 9999)]
        public double GoldCost { get; set; } = 10.0;

        // Checklist items (Daily + Todo)
        public List<string> ChecklistItems { get; set; } = new();
    }
}
