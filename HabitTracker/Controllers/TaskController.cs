using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    /// <summary>
    /// Quản lý Quest của user - xem danh sách, hoàn thành quest
    /// </summary>
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IQuestService _questService;
        private readonly ISearchService _searchService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            AppDbContext context,
            IQuestService questService,
            ISearchService searchService,
            ILogger<TaskController> logger)
        {
            _context = context;
            _questService = questService;
            _searchService = searchService;
            _logger = logger;
        }

        // ===== DANH SÁCH QUEST VỚI FILTER =====
        public async Task<IActionResult> Index(
            string? category,
            string? difficulty,
            string? frequency)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Lấy quest đã hoàn thành hôm nay
            var completedTodayIds = await _questService.GetCompletedTodayAsync(userId.Value);

            // Query với filter
            var query = _context.Quests.Where(q => q.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(q => q.Category == category);

            if (!string.IsNullOrEmpty(difficulty))
                query = query.Where(q => q.Difficulty == difficulty);

            if (!string.IsNullOrEmpty(frequency))
                query = query.Where(q => q.Frequency == frequency);

            var quests = await query
                .OrderBy(q => q.Category)
                .ThenBy(q => q.Name)
                .ToListAsync();

            ViewBag.CompletedTodayIds = completedTodayIds;
            ViewBag.Categories = AppConstants.Categories.All;
            ViewBag.Difficulties = AppConstants.Difficulty.All;
            ViewBag.Frequencies = AppConstants.Frequency.All;
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedDifficulty = difficulty;
            ViewBag.SelectedFrequency = frequency;

            return View(quests);
        }

        // ===== SEARCH VỚI FILTER NÂNG CAO =====
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            string? q,
            string? category,
            string? difficulty,
            string? frequency,
            bool? completed)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Use search service for advanced filtering
            var quests = await _searchService.SearchQuestsAsync(
                userId.Value, q, category, difficulty, frequency, completed, null, null);

            ViewBag.SearchQuery = q;
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedDifficulty = difficulty;
            ViewBag.SelectedFrequency = frequency;
            ViewBag.Categories = AppConstants.Categories.All;
            ViewBag.Difficulties = AppConstants.Difficulty.All;
            ViewBag.Frequencies = AppConstants.Frequency.All;

            _logger.LogInformation($"User {userId} searched quests: query='{q}', found {quests.Count} results");

            return View("Search", quests);
        }

        // ===== AUTOCOMPLETE SUGGESTIONS =====
        [HttpGet("autocomplete")]
        public async Task<IActionResult> Autocomplete(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(new List<string>());

            var suggestions = await _searchService.AutocompleteQuestAsync(q, limit: 10);
            return Json(suggestions);
        }

        // ===== XÁC NHẬN HOÀN THÀNH QUEST =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(List<int> questIds)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Validate input
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

            // Lấy quest đã hoàn thành hôm nay
            var completedTodayIds = await _questService.GetCompletedTodayAsync(userId.Value);

            // Lọc những quest chưa làm hôm nay
            var newQuestIds = questIds
                .Where(id => !completedTodayIds.Contains(id))
                .ToList();

            if (!newQuestIds.Any())
            {
                TempData["Error"] = AppConstants.Messages.ALL_QUESTS_DONE;
                return RedirectToAction(nameof(Index));
            }

            // Lấy thông tin quest
            var quests = await _context.Quests
                .Where(q => newQuestIds.Contains(q.Id))
                .ToListAsync();

            // Track old values
            int oldXP = user.XP;
            int oldLevel = user.Level;

            // Cộng XP và tạo UserQuest records
            int totalXP = 0;
            foreach (var quest in quests)
            {
                _context.UserQuests.Add(new UserQuest
                {
                    UserId = user.Id,
                    QuestId = quest.Id,
                    CompletedDate = DateTime.Today,
                    Status = "Confirmed",
                    XPEarned = quest.XPReward
                });

                totalXP += quest.XPReward;
                quest.TimesCompleted++; // Track for trending
            }

            // Update user stats
            user.XP += totalXP;
            user.Level = _questService.CalculateLevel(user.XP);
            user.TotalQuestsCompleted += quests.Count;
            user.TotalXPEarned += totalXP;
            user.LastActiveDate = DateTime.UtcNow;

            // Update streak
            _questService.UpdateStreak(user);

            // Save changes
            await _context.SaveChangesAsync();

            // ===== TOAST NOTIFICATIONS =====
            SetToastXP(totalXP, quests.Count);
            SetToastStreak(user);
            SetToastLevelUp(oldLevel, user.Level);
            await SetToastBadges(user, oldXP);

            _logger.LogInformation(
                $"User {userId} completed {quests.Count} quests, gained {totalXP} XP, Level: {oldLevel}->{user.Level}");

            return RedirectToAction(nameof(Index));
        }

        // ===== QUEST DETAILS =====
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var quest = await _context.Quests
                .Include(q => q.UserQuests)
                .FirstOrDefaultAsync(q => q.Id == id && q.IsActive);

            if (quest == null)
                return NotFound();

            var userId = GetUserId();
            if (userId != null)
            {
                var completedCount = await _context.UserQuests
                    .CountAsync(uq => uq.QuestId == id && uq.UserId == userId);

                ViewBag.UserCompletedCount = completedCount;
            }

            ViewBag.TotalCompleted = quest.UserQuests?.Count ?? 0;

            return View(quest);
        }

        // ===== HELPER METHODS =====
        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
        }

        private void SetToastXP(int totalXP, int questCount)
        {
            TempData["ToastXP"] = $"✅ +{totalXP} XP từ {questCount} nhiệm vụ!";
        }

        private void SetToastStreak(User user)
        {
            if (user.CurrentStreak > 1)
                TempData["ToastStreak"] = $"🔥 Streak {user.CurrentStreak} ngày liên tiếp!";
        }

        private void SetToastLevelUp(int oldLevel, int newLevel)
        {
            if (newLevel > oldLevel)
                TempData["ToastLevel"] = $"⬆️ Lên Level {newLevel}!";
        }

        private async Task SetToastBadges(User user, int oldXP)
        {
            var newBadges = await _questService.AwardBadgesAsync(user, oldXP);

            if (newBadges.Any())
                TempData["ToastBadge"] = $"🏆 Đạt badge: {string.Join(", ", newBadges)}";
        }
    }
}