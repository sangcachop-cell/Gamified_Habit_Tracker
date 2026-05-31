using HabitTracker.Constants;
using HabitTracker.Services;

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

        // Skills bar
        public User? User { get; set; }
        public EffectiveStats? EffectiveStats { get; set; }
        public IReadOnlyList<SpellDefinition> Skills { get; set; } = Array.Empty<SpellDefinition>();
        public bool CanUseSkills => User != null && !string.IsNullOrEmpty(User.Class) && User.Level >= 11;
    }
}
