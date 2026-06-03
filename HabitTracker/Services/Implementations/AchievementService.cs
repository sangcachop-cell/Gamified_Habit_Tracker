using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class AchievementService : IAchievementService
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notify;
        private readonly ILogger<AchievementService> _logger;

        public AchievementService(AppDbContext context, INotificationService notify, ILogger<AchievementService> logger)
        {
            _context = context;
            _notify  = notify;
            _logger  = logger;
        }

        // ─── Core award ────────────────────────────────────────────────────────

        private async Task<List<string>> AwardBatchAsync(int userId, IEnumerable<Badge> candidates)
        {
            var earned = await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .Select(ub => ub.BadgeId)
                .ToListAsync();

            var earnedSet = earned.ToHashSet();
            var awarded   = new List<string>();

            foreach (var badge in candidates.Where(b => !earnedSet.Contains(b.Id)))
            {
                _context.UserBadges.Add(new UserBadge
                {
                    UserId     = userId,
                    BadgeId    = badge.Id,
                    EarnedDate = DateTime.UtcNow
                });

                var label = $"{badge.Icon} {badge.Name}";
                awarded.Add(label);

                await _notify.CreateNotificationAsync(
                    userId,
                    "Achievement Unlocked!",
                    badge.Name,
                    "achievement",
                    "/Dashboard/Badges",
                    badge.Icon ?? "🏅");
            }

            if (awarded.Count > 0)
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User {UserId} earned {Count} achievement(s)", userId, awarded.Count);
            }

            return awarded;
        }

        // ─── Streak ────────────────────────────────────────────────────────────

        public async Task<List<string>> CheckStreakAsync(User user)
        {
            int streak     = user.CurrentStreak;
            int[] thresholds = { 7, 21, 90, 180, 365 };

            var keys = thresholds
                .Where(t => streak >= t)
                .Select(t => $"streak_{t}")
                .ToHashSet();

            if (!keys.Any()) return new();

            var badges = await _context.Badges
                .Where(b => b.TriggerType == "Streak" && keys.Contains(b.Key))
                .ToListAsync();

            return await AwardBatchAsync(user.Id, badges);
        }

        // ─── Task milestones ───────────────────────────────────────────────────

        public async Task<List<string>> CheckTaskMilestoneAsync(User user)
        {
            int total      = user.TotalTasksCompleted;
            int[] thresholds = { 1, 10, 50, 100, 500 };

            var keys = thresholds
                .Where(t => total >= t)
                .Select(t => $"tasks_{t}")
                .ToHashSet();

            if (!keys.Any()) return new();

            var badges = await _context.Badges
                .Where(b => b.TriggerType == "TaskMilestone" && keys.Contains(b.Key))
                .ToListAsync();

            return await AwardBatchAsync(user.Id, badges);
        }

        // ─── Perfect Day ───────────────────────────────────────────────────────

        public async Task<List<string>> CheckPerfectDayAsync(User user)
        {
            int count      = user.PerfectDayCount;
            int[] thresholds = { 1, 7, 30 };

            var keys = thresholds
                .Where(t => count >= t)
                .Select(t => $"perfect_{t}")
                .ToHashSet();

            if (!keys.Any()) return new();

            var badges = await _context.Badges
                .Where(b => b.TriggerType == "PerfectDay" && keys.Contains(b.Key))
                .ToListAsync();

            return await AwardBatchAsync(user.Id, badges);
        }

        // ─── Ultimate Gear ─────────────────────────────────────────────────────

        public async Task<List<string>> CheckUltimateGearAsync(int userId, string userClass)
        {
            if (string.IsNullOrEmpty(userClass)) return new();

            var key   = $"ultimate_{userClass}";
            var badge = await _context.Badges
                .FirstOrDefaultAsync(b => b.TriggerType == "UltimateGear" && b.Key == key);
            if (badge == null) return new();

            // Check all class gear items are owned
            var classGearIds = await _context.GearItems
                .Where(g => g.GearClass == userClass)
                .Select(g => g.Id)
                .ToListAsync();

            if (!classGearIds.Any()) return new();

            var ownedIds = (await _context.UserGearItems
                .Where(ug => ug.UserId == userId)
                .Select(ug => ug.GearItemId)
                .ToListAsync())
                .ToHashSet();

            if (!classGearIds.All(id => ownedIds.Contains(id))) return new();

            return await AwardBatchAsync(userId, new[] { badge });
        }

        // ─── Stable ────────────────────────────────────────────────────────────

        public async Task<List<string>> CheckStableAsync(int userId)
        {
            var pets = await _context.UserPets
                .Where(p => p.UserId == userId)
                .ToListAsync();

            int petCount   = pets.Count(p => !p.IsMount);
            int mountCount = pets.Count(p => p.IsMount);
            int total      = petCount + mountCount;

            var allBadges = await _context.Badges
                .Where(b => b.TriggerType == "Stable")
                .ToListAsync();

            var petAnimals   = pets.Where(p => !p.IsMount)
                                   .Select(p => p.PetKey.Split('-')[0])
                                   .ToHashSet();
            var mountAnimals = pets.Where(p => p.IsMount)
                                   .Select(p => p.PetKey.Split('-')[0])
                                   .ToHashSet();
            bool hasTriad    = petAnimals.Intersect(mountAnimals).Any();

            var candidates = allBadges.Where(b => b.Key switch
            {
                "stable_10"    => total      >= 10,
                "beast_master" => petCount   >= b.TriggerValue,
                "mount_master" => mountCount >= b.TriggerValue,
                "triad_bingo"  => hasTriad,
                _              => false
            }).ToList();

            return await AwardBatchAsync(userId, candidates);
        }

        // ─── Quest ─────────────────────────────────────────────────────────────

        public async Task<List<string>> CheckQuestAsync(int userId)
        {
            int questCount = await _context.PartyQuestMembers
                .Include(m => m.PartyQuest)
                .CountAsync(m => m.UserId == userId
                              && m.Response == "accepted"
                              && m.PartyQuest.Status == "Complete");

            int[] thresholds = { 1, 10, 50 };
            var keys = thresholds
                .Where(t => questCount >= t)
                .Select(t => $"quests_{t}")
                .ToHashSet();

            if (!keys.Any()) return new();

            var badges = await _context.Badges
                .Where(b => b.TriggerType == "Quest" && keys.Contains(b.Key))
                .ToListAsync();

            return await AwardBatchAsync(userId, badges);
        }

        // ─── Social ────────────────────────────────────────────────────────────

        public async Task<List<string>> CheckGuildJoinAsync(int userId)
        {
            var badge = await _context.Badges
                .FirstOrDefaultAsync(b => b.Key == "joined_guild");
            if (badge == null) return new();

            return await AwardBatchAsync(userId, new[] { badge });
        }
    }
}
