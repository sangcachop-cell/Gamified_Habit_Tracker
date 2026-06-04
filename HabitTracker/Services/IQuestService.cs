using HabitTracker.Models;

namespace HabitTracker.Services
{
    /// <summary>
    /// Quản lý logic liên quan đến Quest
    /// </summary>
    public interface IQuestService
    {
        /// <summary>
        /// Lấy XP reward dựa vào Difficulty
        /// </summary>
        int GetXPRewardByDifficulty(string difficulty);

        /// <summary>
        /// Update streak cho user
        /// </summary>
        void UpdateStreak(User user);

        /// <summary>
        /// Tính level dựa vào cumulative XP (Habitica quadratic formula).
        /// </summary>
        int CalculateLevel(int cumulativeXp);

        /// <summary>
        /// XP required to advance FROM level N to N+1.
        /// </summary>
        int XpToNextLevel(int level);

        /// <summary>
        /// XP accumulated within the current level (for progress bar display).
        /// </summary>
        int XpWithinLevel(int cumulativeXp);

        /// <summary>
        /// Lấy danh sách quest đã hoàn thành hôm nay
        /// </summary>
        Task<List<int>> GetCompletedTodayAsync(int userId);

        /// <summary>
        /// Kiểm tra quest có được hoàn thành hôm nay chưa
        /// </summary>
        Task<bool> IsQuestCompletedTodayAsync(int userId, int questId);
    }
}