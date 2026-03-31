using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class Task
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        // Daily / Weekly / Monthly
        public string Type { get; set; }

        public int XP { get; set; }

        public DateTime? Deadline { get; set; }

        public bool IsActive { get; set; } = true;
    }
}