using HabitTracker.Models;

namespace HabitTracker.Models.ViewModels
{
    /// <summary>
    /// ViewModel for advanced statistics dashboard
    /// </summary>
    public class StatisticsViewModel
    {
        // ===== BASIC STATS =====
        /// <summary>
        /// (Total quests, Completed today, Pending)
        /// </summary>
        public (int Total, int Completed, int Pending) QuestStats { get; set; }

        // ===== BREAKDOWNS =====
        /// <summary>
        /// Completion count by category (30 days)
        /// </summary>
        public List<(string Category, int Count)> CategoryBreakdown { get; set; } = new();

        /// <summary>
        /// Daily XP trend (30 days)
        /// </summary>
        public List<(DateTime Date, int XP)> XPTrend { get; set; } = new();

        /// <summary>
        /// Completion count by difficulty (30 days)
        /// </summary>
        public List<(string Difficulty, int Count)> DifficultyBreakdown { get; set; } = new();

        /// <summary>
        /// Completion count by frequency (30 days)
        /// </summary>
        public List<(string Frequency, int Count)> FrequencyBreakdown { get; set; } = new();

        /// <summary>
        /// Activity by day of week (Monday=1, Sunday=0)
        /// </summary>
        public List<(int DayOfWeek, int Count)> ActivityByDay { get; set; } = new();

        // ===== PERFORMANCE METRICS =====
        /// <summary>
        /// (CompletionRate, AverageXPPerDay, ConsistencyScore)
        /// </summary>
        public (double CompletionRate, double AverageXPPerDay, int ConsistencyScore) PerformanceMetrics { get; set; }

        // ===== ACHIEVEMENTS =====
        /// <summary>
        /// Recent earned badges
        /// </summary>
        public List<(string AchievementName, DateTime AchievedDate)> RecentAchievements { get; set; } = new();

        // ===== COMPARISON =====
        /// <summary>
        /// (UserCompletion, AverageCompletion) - last 7 days
        /// </summary>
        public (int UserCompletion, int AverageCompletion) Comparison { get; set; }

        // ===== HELPERS =====
        /// <summary>
        /// Get category with most completions
        /// </summary>
        public string GetTopCategory()
        {
            return CategoryBreakdown?.FirstOrDefault().Category ?? "N/A";
        }

        /// <summary>
        /// Get total completions across all categories
        /// </summary>
        public int GetTotalCompletions()
        {
            return CategoryBreakdown?.Sum(x => x.Count) ?? 0;
        }

        /// <summary>
        /// Get total XP earned from trend
        /// </summary>
        public int GetTotalXP()
        {
            return XPTrend?.Sum(x => x.XP) ?? 0;
        }

        /// <summary>
        /// Check if user is above average
        /// </summary>
        public bool IsAboveAverage()
        {
            return Comparison.UserCompletion > Comparison.AverageCompletion;
        }

        /// <summary>
        /// Get consistency status (High/Medium/Low)
        /// </summary>
        public string GetConsistencyStatus()
        {
            return PerformanceMetrics.ConsistencyScore switch
            {
                >= 80 => "🟢 Xuất sắc",
                >= 60 => "🟡 Tốt",
                >= 40 => "🟠 Trung bình",
                _ => "🔴 Cần cố gắng"
            };
        }

        /// <summary>
        /// Get performance rating
        /// </summary>
        public string GetPerformanceRating()
        {
            var rate = PerformanceMetrics.CompletionRate;
            return rate switch
            {
                >= 90 => "⭐⭐⭐⭐⭐ Siêu xuất sắc",
                >= 75 => "⭐⭐⭐⭐ Rất tốt",
                >= 60 => "⭐⭐⭐ Tốt",
                >= 40 => "⭐⭐ Trung bình",
                _ => "⭐ Cần cố gắng"
            };
        }
    }
}