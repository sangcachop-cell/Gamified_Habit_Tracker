namespace HabitTracker.Models
{
    public class TaskLog
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public DateTime Date { get; set; }

        public bool IsCompleted { get; set; }
    }
}