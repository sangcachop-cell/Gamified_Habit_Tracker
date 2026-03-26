using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class Habit
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int XPReward { get; set; } = 10;

        public int UserId { get; set; }
        public User? User { get; set; }

        public string? Icon { get; set; }

        public string Frequency { get; set; } = "Daily";

        public TimeSpan? ReminderTime { get; set; }

        public DateTime? Deadline { get; set; }

        public int Streak { get; set; } = 0;

        public DateTime? LastCompletedDate { get; set; }

        public string? Category { get; set; }
    }
}