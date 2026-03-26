using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class HabitLog
    {
        public int Id { get; set; }

        public int HabitId { get; set; }
        public Habit Habit { get; set; }

        public DateTime Date { get; set; } = DateTime.Today;

        public bool IsCompleted { get; set; }
    }
}