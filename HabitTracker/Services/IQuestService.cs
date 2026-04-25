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
        /// Trao badge nếu đạt XP requirement
        /// </summary>
        Task<List<string>> AwardBadgesAsync(User user, int oldXP);

        /// <summary>
        /// Tính level dựa vào XP
        /// </summary>
        int CalculateLevel(int xp);

        /// <summary>
        /// Lấy danh sách quest đã hoàn thành hôm nay
        /// </summary>
        Task<List<int>> GetCompletedTodayAsync(int userId);

        /// <summary>
        /// Kiểm tra quest có được hoàn thành hôm nay chưa
        /// </summary>
        Task<bool> IsQuestCompletedTodayAsync(int userId, int questId);

        /// <summary>
        /// Update RPG base stats (STR/WILL/INT/AGL/END) based on completed quests
        /// </summary>
        void UpdateRpgStats(User user, IEnumerable<Quest> completedQuests);

        /// <summary>
        /// Grant +1 to every base stat for each level gained
        /// </summary>
        void GrantLevelUpStats(User user, int levelsGained);

        /// <summary>
        /// XP gap between level N and N+1
        /// </summary>
        int XPToNextLevel(int level);
    }
}