using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class HideoutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IQuestService _questService;
        private readonly IHideoutService _hideoutService;
        private readonly ILogger<HideoutController> _logger;

        public HideoutController(
            AppDbContext context,
            IQuestService questService,
            IHideoutService hideoutService,
            ILogger<HideoutController> logger)
        {
            _context = context;
            _questService = questService;
            _hideoutService = hideoutService;
            _logger = logger;
        }

        // ===== HIDEOUT MAIN PAGE =====
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(
            string? category,
            string? difficulty,
            string? frequency)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Ensure user has all facilities unlocked at level 1
            await _hideoutService.EnsureUserFacilitiesAsync(userId.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var facilities = await _hideoutService.GetUserFacilitiesAsync(userId.Value);
            var (buffAtk, buffHp, buffArmor, buffXpGain, buffStamina) =
                _hideoutService.GetFacilityBuffs(facilities);

            var completedTodayIds = await _questService.GetCompletedTodayAsync(userId.Value);

            var questQuery = _context.Quests.Where(q => q.IsActive);
            if (!string.IsNullOrEmpty(category))   questQuery = questQuery.Where(q => q.Category  == category);
            if (!string.IsNullOrEmpty(difficulty))  questQuery = questQuery.Where(q => q.Difficulty == difficulty);
            if (!string.IsNullOrEmpty(frequency))   questQuery = questQuery.Where(q => q.Frequency  == frequency);

            var quests = await questQuery
                .OrderBy(q => q.Category).ThenBy(q => q.Name)
                .ToListAsync();

            ViewBag.User              = user;
            ViewBag.Facilities        = facilities;
            ViewBag.BuffAtk           = buffAtk;
            ViewBag.BuffHp            = buffHp;
            ViewBag.BuffArmor         = buffArmor;
            ViewBag.BuffXpGain        = buffXpGain;
            ViewBag.BuffStamina       = buffStamina;
            ViewBag.CompletedTodayIds = completedTodayIds;
            ViewBag.Categories        = AppConstants.Categories.All;
            ViewBag.Difficulties      = AppConstants.Difficulty.All;
            ViewBag.Frequencies       = AppConstants.Frequency.All;
            ViewBag.SelectedCategory  = category;
            ViewBag.SelectedDifficulty = difficulty;
            ViewBag.SelectedFrequency = frequency;

            _logger.LogInformation($"User {userId} visited hideout");

            return View(quests);
        }

        // ===== COMPLETE QUESTS FROM HIDEOUT =====
        [HttpPost("Confirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(List<int> questIds)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (questIds == null || !questIds.Any())
            {
                TempData["Error"] = AppConstants.Messages.NO_QUEST_SELECTED;
                return RedirectToAction(nameof(Index));
            }

            var user = await _context.Users
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var completedTodayIds = await _questService.GetCompletedTodayAsync(userId.Value);
            var newQuestIds = questIds.Where(id => !completedTodayIds.Contains(id)).ToList();

            if (!newQuestIds.Any())
            {
                TempData["Error"] = AppConstants.Messages.ALL_QUESTS_DONE;
                return RedirectToAction(nameof(Index));
            }

            var quests = await _context.Quests
                .Where(q => newQuestIds.Contains(q.Id))
                .ToListAsync();

            int oldXP    = user.XP;
            int oldLevel = user.Level;

            double xpMultiplier = 1.0 + (user.XPGainPercent / 100.0);

            // Also apply hideout Archive facility buff to XP multiplier
            var facilities = await _hideoutService.GetUserFacilitiesAsync(userId.Value);
            var (_, _, _, buffXpGain, _) = _hideoutService.GetFacilityBuffs(facilities);
            xpMultiplier += buffXpGain / 100.0;

            int totalXP = 0;
            foreach (var quest in quests)
            {
                int earnedXP = (int)Math.Round(quest.XPReward * xpMultiplier);

                _context.UserQuests.Add(new UserQuest
                {
                    UserId       = user.Id,
                    QuestId      = quest.Id,
                    CompletedDate = DateTime.Today,
                    Status       = "Confirmed",
                    XPEarned     = earnedXP
                });

                totalXP += earnedXP;
                quest.TimesCompleted++;
            }

            user.XP  += totalXP;
            int newLevel    = _questService.CalculateLevel(user.XP);
            int levelsGained = newLevel - oldLevel;
            user.Level = newLevel;
            user.TotalQuestsCompleted += quests.Count;
            user.TotalXPEarned        += totalXP;
            user.LastActiveDate        = DateTime.UtcNow;

            _questService.UpdateStreak(user);
            _questService.UpdateRpgStats(user, quests);

            if (levelsGained > 0)
                _questService.GrantLevelUpStats(user, levelsGained);

            await _context.SaveChangesAsync();

            TempData["ToastXP"] = $"✅ +{totalXP} XP từ {quests.Count} nhiệm vụ!";
            if (user.CurrentStreak > 1)
                TempData["ToastStreak"] = $"🔥 Streak {user.CurrentStreak} ngày liên tiếp!";
            if (levelsGained > 0)
                TempData["ToastLevel"] = $"⬆️ Lên Level {newLevel}!";

            var newBadges = await _questService.AwardBadgesAsync(user, oldXP);
            if (newBadges.Any())
                TempData["ToastBadge"] = $"🏆 Đạt badge: {string.Join(", ", newBadges)}";

            _logger.LogInformation(
                $"User {userId} completed {quests.Count} quests from hideout, +{totalXP} XP");

            return RedirectToAction(nameof(Index));
        }

        // ===== UPGRADE FACILITY (placeholder) =====
        [HttpPost("Upgrade/{facilityId}")]
        [ValidateAntiForgeryToken]
        public IActionResult Upgrade(int facilityId)
        {
            TempData["Info"] = "⚒️ Facility upgrades are coming in a future update!";
            return RedirectToAction(nameof(Index));
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}
