using HabitTracker.Constants;

namespace HabitTracker.Models.ViewModels
{
    public class TaskBoardViewModel
    {
        public List<HabitTask> Habits  { get; set; } = new();
        public List<HabitTask> Dailies { get; set; } = new();
        public List<HabitTask> Todos   { get; set; } = new();
        public List<HabitTask> Rewards { get; set; } = new();

        public int DueDailiesCount    => Dailies.Count(t => !t.IsCompleted);
        public int PendingTodosCount  => Todos.Count(t => t.DateCompleted == null);
    }
}
