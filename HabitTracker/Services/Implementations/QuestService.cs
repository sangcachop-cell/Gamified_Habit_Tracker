using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class QuestService : IQuestService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<QuestService> _logger;

        public QuestService(AppDbContext context, ILogger<QuestService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int GetXPRewardByDifficulty(string difficulty)
        {
            return AppConstants.XPRewards.GetRewardByDifficulty(difficulty);
        }

        public void UpdateStreak(User user)
        {
            var today = DateTime.Today;

            // Nếu đã check-in hôm nay, không update
            if (user.LastCheckInDate == today)
                return;

            // Nếu check-in ngày hôm qua, tăng streak
            if (user.LastCheckInDate == today.AddDays(-1))
            {
                user.CurrentStreak++;
            }
            else
            {
                // Nếu không liên tiếp, reset về 1
                user.CurrentStreak = 1;
            }

            // Update longest streak nếu cần
            if (user.CurrentStreak > user.LongestStreak)
                user.LongestStreak = user.CurrentStreak;

            // Update last check-in date
            user.LastCheckInDate = today;

            _logger.LogInformation($"User {user.Id} streak updated to {user.CurrentStreak}");
        }

        public async Task<List<string>> AwardBadgesAsync(User user, int oldXP)
        {
            var newBadgeNames = new List<string>();

            try
            {
                // Lấy tất cả badges
                var allBadges = await _context.Badges.ToListAsync();

                // Lấy những badge user đã có
                var earnedBadgeIds = user.UserBadges?
                    .Select(ub => ub.BadgeId)
                    .ToHashSet() ?? new HashSet<int>();

                // Kiểm tra từng badge
                foreach (var badge in allBadges)
                {
                    // Nếu user vừa đạt được badge
                    if (user.XP >= badge.RequiredXP && !earnedBadgeIds.Contains(badge.Id))
                    {
                        _context.UserBadges.Add(new UserBadge
                        {
                            UserId = user.Id,
                            BadgeId = badge.Id,
                            EarnedDate = DateTime.Now
                        });

                        newBadgeNames.Add($"{badge.Icon} {badge.Name}");
                    }
                }

                if (newBadgeNames.Any())
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation(
                        $"User {user.Id} earned {newBadgeNames.Count} new badges");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error awarding badges: {ex.Message}");
            }

            return newBadgeNames;
        }

        public int CalculateLevel(int xp)
        {
            return (xp / AppConstants.XP_PER_LEVEL) + 1;
        }

        public async Task<List<int>> GetCompletedTodayAsync(int userId)
        {
            return await _context.UserQuests
                .Where(uq => uq.UserId == userId && uq.CompletedDate == DateTime.Today)
                .Select(uq => uq.QuestId)
                .ToListAsync();
        }

        public async Task<bool> IsQuestCompletedTodayAsync(int userId, int questId)
        {
            return await _context.UserQuests
                .AnyAsync(uq => uq.UserId == userId &&
                               uq.QuestId == questId &&
                               uq.CompletedDate == DateTime.Today);
        }
    }
}