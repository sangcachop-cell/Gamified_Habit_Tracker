using HabitTracker.Constants;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services
{
    public interface ITaskService
    {
        Task<TaskBoardViewModel> GetUserTasksAsync(int userId);
        Task<HabitTask> CreateTaskAsync(int userId, CreateTaskViewModel model);
        Task UpdateTaskAsync(int taskId, int userId, EditTaskViewModel model);
        Task DeleteTaskAsync(int taskId, int userId);
        Task<ScoreResultViewModel> ScoreTaskAsync(int taskId, int userId, string direction);
        bool IsDue(HabitTask task, DateTime today);
        Task RunCronAsync(int userId);
        Task<bool> ToggleChecklistItemAsync(int itemId, int userId);
        Task ReorderTasksAsync(int userId, TaskType type, List<int> orderedIds);
        Task ClearCompletedTodosAsync(int userId);
    }
}
