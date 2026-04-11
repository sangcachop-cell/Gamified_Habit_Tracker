using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(AppDbContext context, ILogger<StatisticsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(int Total, int Completed, int Pending)> GetQuestStatsAsync(int userId)
        {
            try
            {
                var total = await _context.Quests.CountAsync(q => q.IsActive);

                var completedToday = await _context.UserQuests
                    .CountAsync(uq => uq.UserId == userId && uq.CompletedDate == DateTime.Today && uq.Status == "Confirmed");

                var pending = total - completedToday;

                _logger.LogInformation($"User {userId} quest stats: {completedToday}/{total}");
                return (total, completedToday, pending);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Quest stats error: {ex.Message}");
                return (0, 0, 0);
            }
        }

        public async Task<List<(string Category, int Count)>> GetCompletionByCategoryAsync(int userId, int days = 30)
        {
            try
            {
                var dateFrom = DateTime.Today.AddDays(-days);

                // Use anonymous object for LINQ translation
                var results = await _context.UserQuests
                    .Where(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed")
                    .Include(uq => uq.Quest)
                    .GroupBy(uq => uq.Quest.Category)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync();

                // Convert to tuple in memory
                var tuples = results.Select(r => (r.Category, r.Count)).ToList();

                _logger.LogInformation($"User {userId} category stats: {tuples.Count} categories");
                return tuples;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Category stats error: {ex.Message}");
                return new List<(string, int)>();
            }
        }

        public async Task<List<(DateTime Date, int XP)>> GetXPTrendAsync(int userId, int days = 30)
        {
            try
            {
                var dateFrom = DateTime.Today.AddDays(-days);

                // Use anonymous object for LINQ translation
                var results = await _context.UserQuests
                    .Where(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed")
                    .Include(uq => uq.Quest)
                    .GroupBy(uq => uq.CompletedDate)
                    .Select(g => new { Date = g.Key, XP = g.Sum(uq => uq.Quest.XPReward) })
                    .OrderBy(x => x.Date)
                    .ToListAsync();

                // Convert to tuple in memory
                var tuples = results.Select(r => (r.Date, r.XP)).ToList();

                _logger.LogInformation($"User {userId} XP trend: {tuples.Count} days");
                return tuples;
            }
            catch (Exception ex)
            {
                _logger.LogError($"XP trend error: {ex.Message}");
                return new List<(DateTime, int)>();
            }
        }

        public async Task<List<(string Difficulty, int Count)>> GetCompletionByDifficultyAsync(int userId, int days = 30)
        {
            try
            {
                var dateFrom = DateTime.Today.AddDays(-days);

                // Use anonymous object for LINQ translation
                var results = await _context.UserQuests
                    .Where(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed")
                    .Include(uq => uq.Quest)
                    .GroupBy(uq => uq.Quest.Difficulty)
                    .Select(g => new { Difficulty = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync();

                // Convert to tuple in memory
                var tuples = results.Select(r => (r.Difficulty, r.Count)).ToList();

                _logger.LogInformation($"User {userId} difficulty stats: {tuples.Count} difficulties");
                return tuples;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Difficulty stats error: {ex.Message}");
                return new List<(string, int)>();
            }
        }

        public async Task<List<(string Frequency, int Count)>> GetCompletionByFrequencyAsync(int userId, int days = 30)
        {
            try
            {
                var dateFrom = DateTime.Today.AddDays(-days);

                // Use anonymous object for LINQ translation
                var results = await _context.UserQuests
                    .Where(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed")
                    .Include(uq => uq.Quest)
                    .GroupBy(uq => uq.Quest.Frequency)
                    .Select(g => new { Frequency = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync();

                // Convert to tuple in memory
                var tuples = results.Select(r => (r.Frequency, r.Count)).ToList();

                _logger.LogInformation($"User {userId} frequency stats: {tuples.Count} frequencies");
                return tuples;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Frequency stats error: {ex.Message}");
                return new List<(string, int)>();
            }
        }

        public async Task<List<(int DayOfWeek, int Count)>> GetActivityByDayOfWeekAsync(int userId, int weeks = 4)
        {
            try
            {
                var dateFrom = DateTime.Today.AddDays(-weeks * 7);

                // Use anonymous object for LINQ translation
                var results = await _context.UserQuests
                    .Where(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed")
                    .GroupBy(uq => (int)uq.CompletedDate.DayOfWeek)
                    .Select(g => new { DayOfWeek = g.Key, Count = g.Count() })
                    .OrderBy(x => x.DayOfWeek)
                    .ToListAsync();

                // Convert to tuple in memory
                var tuples = results.Select(r => (r.DayOfWeek, r.Count)).ToList();

                _logger.LogInformation($"User {userId} activity heatmap: {tuples.Count} days");
                return tuples;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Activity heatmap error: {ex.Message}");
                return new List<(int, int)>();
            }
        }

        public async Task<List<(string AchievementName, DateTime AchievedDate)>> GetRecentAchievementsAsync(int userId, int limit = 10)
        {
            try
            {
                // Use anonymous object for LINQ translation
                var results = await _context.UserBadges
                    .Where(ub => ub.UserId == userId)
                    .Include(ub => ub.Badge)
                    .OrderByDescending(ub => ub.EarnedDate)
                    .Take(limit)
                    .Select(ub => new { Name = ub.Badge.Name, Date = ub.EarnedDate })
                    .ToListAsync();

                // Convert to tuple in memory
                var tuples = results.Select(r => (r.Name, r.Date)).ToList();

                _logger.LogInformation($"User {userId} achievements: {tuples.Count} recent");
                return tuples;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Achievements error: {ex.Message}");
                return new List<(string, DateTime)>();
            }
        }

        public async Task<(double CompletionRate, double AverageXPPerDay, int ConsistencyScore)> GetPerformanceMetricsAsync(int userId, int days = 30)
        {
            try
            {
                var dateFrom = DateTime.Today.AddDays(-days);

                // Completion rate
                var totalQuests = await _context.Quests.CountAsync(q => q.IsActive);
                var completedCount = await _context.UserQuests
                    .CountAsync(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed");

                var completionRate = totalQuests > 0 ? (completedCount * 100.0) / totalQuests : 0;

                // Average XP per day
                var totalXP = await _context.UserQuests
                    .Where(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed")
                    .Include(uq => uq.Quest)
                    .SumAsync(uq => uq.Quest.XPReward);

                var averageXPPerDay = days > 0 ? totalXP / (double)days : 0;

                // Consistency score (% of days with at least 1 quest)
                var activeDays = await _context.UserQuests
                    .Where(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed")
                    .Select(uq => uq.CompletedDate)
                    .Distinct()
                    .CountAsync();

                var consistencyScore = days > 0 ? (activeDays * 100) / days : 0;

                _logger.LogInformation($"User {userId} metrics: rate={completionRate:F1}%, xp/day={averageXPPerDay:F1}, consistency={consistencyScore}%");
                return (completionRate, averageXPPerDay, consistencyScore);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Performance metrics error: {ex.Message}");
                return (0, 0, 0);
            }
        }

        public async Task<(int UserCompletion, int AverageCompletion)> CompareWithAverageAsync(int userId, int days = 7)
        {
            try
            {
                var dateFrom = DateTime.Today.AddDays(-days);

                // User's completion count
                var userCompletion = await _context.UserQuests
                    .CountAsync(uq => uq.UserId == userId && uq.CompletedDate >= dateFrom && uq.Status == "Confirmed");

                // All users' total completions
                var allUsersTotal = await _context.UserQuests
                    .CountAsync(uq => uq.CompletedDate >= dateFrom && uq.Status == "Confirmed");

                // Count of active users
                var allUsersCount = await _context.Users.CountAsync();
                var averageCompletion = allUsersCount > 0 ? allUsersTotal / allUsersCount : 0;

                _logger.LogInformation($"User {userId} vs average: {userCompletion} vs {averageCompletion}");
                return (userCompletion, averageCompletion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Compare average error: {ex.Message}");
                return (0, 0);
            }
        }
    }
}