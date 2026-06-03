using HabitTracker.Models;

namespace HabitTracker.Services
{
    public interface IAchievementService
    {
        // Called on task score (isUp)
        Task<List<string>> CheckStreakAsync(User user);
        Task<List<string>> CheckTaskMilestoneAsync(User user);

        // Called in cron after perfect day detected
        Task<List<string>> CheckPerfectDayAsync(User user);

        // Called after gear equip
        Task<List<string>> CheckUltimateGearAsync(int userId, string userClass);

        // Called after pet evolution or stable change
        Task<List<string>> CheckStableAsync(int userId);

        // Called after party quest completion
        Task<List<string>> CheckQuestAsync(int userId);

        // Called after joining a guild
        Task<List<string>> CheckGuildJoinAsync(int userId);
    }
}
