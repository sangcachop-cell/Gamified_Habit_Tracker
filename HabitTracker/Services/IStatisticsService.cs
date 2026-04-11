using HabitTracker.Models;

namespace HabitTracker.Services
{
    public interface IStatisticsService
    {
        // Dashboard stats
        Task<(int Total, int Completed, int Pending)> GetQuestStatsAsync(int userId);

        // Category breakdown
        Task<List<(string Category, int Count)>> GetCompletionByCategoryAsync(int userId, int days = 30);

        // XP trend
        Task<List<(DateTime Date, int XP)>> GetXPTrendAsync(int userId, int days = 30);

        // Difficulty breakdown
        Task<List<(string Difficulty, int Count)>> GetCompletionByDifficultyAsync(int userId, int days = 30);

        // Frequency breakdown
        Task<List<(string Frequency, int Count)>> GetCompletionByFrequencyAsync(int userId, int days = 30);

        // Activity heatmap (by day of week)
        Task<List<(int DayOfWeek, int Count)>> GetActivityByDayOfWeekAsync(int userId, int weeks = 4);

        // Top achievements
        Task<List<(string AchievementName, DateTime AchievedDate)>> GetRecentAchievementsAsync(int userId, int limit = 10);

        // Performance metrics
        Task<(double CompletionRate, double AverageXPPerDay, int ConsistencyScore)> GetPerformanceMetricsAsync(int userId, int days = 30);

        // Comparison with average
        Task<(int UserCompletion, int AverageCompletion)> CompareWithAverageAsync(int userId, int days = 7);
    }
}