using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class MinigameController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IQuestService _questService;
        private readonly IHideoutService _hideoutService;
        private readonly ILogger<MinigameController> _logger;

        public MinigameController(
            AppDbContext context,
            IQuestService questService,
            IHideoutService hideoutService,
            ILogger<MinigameController> logger)
        {
            _context = context;
            _questService = questService;
            _hideoutService = hideoutService;
            _logger = logger;
        }

        // ── GET /Minigame/Play/{questId} ──────────────────────────────────────
        [HttpGet("Play/{questId}")]
        public async Task<IActionResult> Play(int questId)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var quest = await _context.Quests
                .Include(q => q.AssignedFacility)
                .FirstOrDefaultAsync(q => q.Id == questId && q.IsActive);
            if (quest == null) return NotFound();

            // TODO: re-enable daily cooldown after testing
            // if (await _questService.IsQuestCompletedTodayAsync(userId.Value, questId))
            // {
            //     TempData["HideoutInfo"] = "Already completed today.";
            //     return RedirectToAction("Index", "Hideout");
            // }

            return View(quest.MinigameType, quest);
        }

        // ── POST /Minigame/Complete ───────────────────────────────────────────
        [HttpPost("Complete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int questId, string difficulty, bool success)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var quest = await _context.Quests.FindAsync(questId);
            if (quest == null) return RedirectToAction("Index", "Hideout");

            // TODO: re-enable daily cooldown after testing
            // if (await _questService.IsQuestCompletedTodayAsync(userId.Value, questId))
            // {
            //     TempData["HideoutInfo"] = "Already completed today.";
            //     return RedirectToAction("Index", "Hideout");
            // }

            var user = await _context.Users
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return RedirectToAction("Login", "Account");

            var facilities = await _hideoutService.GetUserFacilitiesAsync(userId.Value);
            var (_, _, _, buffXpGain, _) = _hideoutService.GetFacilityBuffs(facilities);

            int baseXp = BaseXp(difficulty);
            double xpMult = 1.0 + user.XPGainPercent / 100.0 + buffXpGain / 100.0;
            int earnedXp = success
                ? (int)Math.Round(baseXp * xpMult)
                : (int)Math.Max(1, Math.Ceiling(baseXp * xpMult / 10.0));

            var (fullStr, fullWill, fullInt, fullAgl, fullEnd) =
                AppConstants.RpgStats.ComputeStatGains(quest.Category, difficulty, quest.Frequency);

            int mult = StatMult(difficulty);
            int Scale(int v) => success
                ? v * mult
                : (v > 0 ? (int)Math.Max(1, Math.Ceiling(v * mult / 10.0)) : 0);
            user.STR  += Scale(fullStr);
            user.WILL += Scale(fullWill);
            user.INT  += Scale(fullInt);
            user.AGL  += Scale(fullAgl);
            user.END  += Scale(fullEnd);

            int oldXp    = user.XP;
            int oldLevel = user.Level;

            _context.UserQuests.Add(new UserQuest
            {
                UserId        = user.Id,
                QuestId       = quest.Id,
                CompletedDate = DateTime.Today,
                Status        = success ? "Confirmed" : "Failed",
                XPEarned      = earnedXp
            });

            user.XP += earnedXp;
            int newLevel    = _questService.CalculateLevel(user.XP);
            int levelsGained = newLevel - oldLevel;
            user.Level = newLevel;
            user.TotalQuestsCompleted++;
            user.TotalXPEarned  += earnedXp;
            user.LastActiveDate  = DateTime.UtcNow;

            _questService.UpdateStreak(user);
            if (levelsGained > 0) _questService.GrantLevelUpStats(user, levelsGained);

            await _context.SaveChangesAsync();

            string diffLabel = difficulty == "Hard" ? "Hard" : difficulty == "Medium" ? "Medium" : "Easy";
            if (success)
                TempData["ToastXP"] = $"✅ {quest.Icon} {quest.Name} [{diffLabel}] — +{earnedXp} XP!";
            else
                TempData["ToastXP"] = $"❌ Failed [{diffLabel}] — +{earnedXp} XP (consolation)";

            if (user.CurrentStreak > 1)
                TempData["ToastStreak"] = $"🔥 Streak {user.CurrentStreak} ngày!";
            if (levelsGained > 0)
                TempData["ToastLevel"] = $"⬆️ Lên Level {newLevel}!";

            var newBadges = await _questService.AwardBadgesAsync(user, oldXp);
            if (newBadges.Any())
                TempData["ToastBadge"] = $"🏆 {string.Join(", ", newBadges)}";

            _logger.LogInformation(
                $"User {userId} completed quest {questId} [{difficulty}] success={success} +{earnedXp}XP");

            return RedirectToAction("Index", "Hideout");
        }

        // ── helpers ───────────────────────────────────────────────────────────

        private static int BaseXp(string difficulty) => difficulty switch
        {
            "Medium" => 25,
            "Hard"   => 100,
            _        => 10
        };

        // Hard gives 2× stat gains to reward the extra difficulty
        private static int StatMult(string difficulty) => difficulty == "Hard" ? 2 : 1;

        private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}
