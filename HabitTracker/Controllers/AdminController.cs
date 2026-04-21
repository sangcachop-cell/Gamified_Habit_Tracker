using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    /// <summary>
    /// Admin panel - Quản lý Quest
    /// Yêu cầu session IsAdmin = "true"
    /// </summary>
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IQuestService _questService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            AppDbContext context,
            IQuestService questService,
            ILogger<AdminController> logger)
        {
            _context = context;
            _questService = questService;
            _logger = logger;
        }

        // ===== HELPER =====
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString(AppConstants.SESSION_IS_ADMIN) == "true";
        }

        private IActionResult CheckAdmin()
        {
            if (!IsAdmin())
            {
                _logger.LogWarning("Unauthorized admin access attempt");
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        // ===== DANH SÁCH QUEST =====
        public async Task<IActionResult> Index()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var quests = await _context.Quests
                .OrderBy(q => q.Category)
                .ThenBy(q => q.Name)
                .ToListAsync();

            return View(quests);
        }

        // ===== THÊM QUEST =====
        public IActionResult Create()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            ViewBag.Categories = AppConstants.Categories.All;
            ViewBag.Difficulties = AppConstants.Difficulty.All;
            ViewBag.Frequencies = AppConstants.Frequency.All;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quest model)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            // Validate model
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = AppConstants.Categories.All;
                ViewBag.Difficulties = AppConstants.Difficulty.All;
                ViewBag.Frequencies = AppConstants.Frequency.All;
                return View(model);
            }

            // Tự động set XP theo difficulty
            model.XPReward = _questService.GetXPRewardByDifficulty(model.Difficulty);
            model.IsActive = true;

            _context.Quests.Add(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin created quest: {model.Name}");
            TempData["Success"] = $"{AppConstants.Toasts.QUEST_CREATED}{model.Name}";

            return RedirectToAction(nameof(Index));
        }

        // ===== SỬA QUEST =====
        public async Task<IActionResult> Edit(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                TempData["Error"] = "Quest không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = AppConstants.Categories.All;
            ViewBag.Difficulties = AppConstants.Difficulty.All;
            ViewBag.Frequencies = AppConstants.Frequency.All;

            return View(quest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Quest model)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = AppConstants.Categories.All;
                ViewBag.Difficulties = AppConstants.Difficulty.All;
                ViewBag.Frequencies = AppConstants.Frequency.All;
                return View(model);
            }

            var quest = await _context.Quests.FindAsync(model.Id);
            if (quest == null)
            {
                TempData["Error"] = "Quest không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            // Update properties
            quest.Name = model.Name?.Trim();
            quest.Description = model.Description?.Trim();
            quest.Icon = model.Icon?.Trim();
            quest.Category = model.Category;
            quest.Difficulty = model.Difficulty;
            quest.Frequency = model.Frequency;
            quest.IsActive = model.IsActive;

            // Recalculate XP reward
            quest.XPReward = _questService.GetXPRewardByDifficulty(model.Difficulty);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin updated quest: {quest.Name}");
            TempData["Success"] = $"{AppConstants.Toasts.QUEST_UPDATED}{quest.Name}";

            return RedirectToAction(nameof(Index));
        }

        // ===== XÓA (SOFT DELETE) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                TempData["Error"] = "Quest không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            // Soft delete: chỉ ẩn đi
            quest.IsActive = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin deleted (soft) quest: {quest.Name}");
            TempData["Success"] = $"{AppConstants.Toasts.QUEST_DELETED}{quest.Name}";

            return RedirectToAction(nameof(Index));
        }

        // ===== KHÔI PHỤC =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                TempData["Error"] = "Quest không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            // Restore
            quest.IsActive = true;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin restored quest: {quest.Name}");
            TempData["Success"] = $"{AppConstants.Toasts.QUEST_RESTORED}{quest.Name}";

            return RedirectToAction(nameof(Index));
        }

        // ===== QUẢN LÝ NGƯỜI DÙNG =====
        public async Task<IActionResult> Users()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var users = await _context.Users
                .OrderByDescending(u => u.XP)
                .ToListAsync();

            return View(users);
        }

        // ===== TOGGLE ADMIN STATUS =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User không tồn tại!";
                return RedirectToAction(nameof(Users));
            }

            user.IsAdmin = !user.IsAdmin;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin toggled user role: {user.Email}, IsAdmin: {user.IsAdmin}");
            TempData["Success"] = $"Đã cập nhật quyền cho {user.Username}";

            return RedirectToAction(nameof(Users));
        }


        // ===== QUẢN LÝ BADGE =====
        public async Task<IActionResult> Badges()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var badges = await _context.Badges
                .OrderBy(b => b.RequiredXP)
                .ToListAsync();

            return View(badges);
        }

        public IActionResult CreateBadge()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;
            return View(new Badge { CreatedAt = DateTime.UtcNow, IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBadge(Badge model)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            model.CreatedAt = DateTime.UtcNow;
            model.IsActive = true;
            _context.Badges.Add(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin created badge: {model.Name}");
            TempData["Success"] = $"✅ Đã tạo badge: {model.Name}";
            return RedirectToAction(nameof(Badges));
        }

        public async Task<IActionResult> EditBadge(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var badge = await _context.Badges.FindAsync(id);
            if (badge == null) return RedirectToAction(nameof(Badges));
            return View(badge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBadge(Badge model)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var badge = await _context.Badges.FindAsync(model.Id);
            if (badge == null) return RedirectToAction(nameof(Badges));

            badge.Name = model.Name?.Trim() ?? badge.Name;
            badge.Description = model.Description?.Trim();
            badge.Icon = model.Icon?.Trim();
            badge.RequiredXP = model.RequiredXP;
            badge.Rarity = model.Rarity;
            badge.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            TempData["Success"] = $"✅ Đã cập nhật badge: {badge.Name}";
            return RedirectToAction(nameof(Badges));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreBadge(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var badge = await _context.Badges.FindAsync(id);
            if (badge == null) return RedirectToAction(nameof(Badges));

            badge.IsActive = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"✅ Đã khôi phục badge: {badge.Name}";
            return RedirectToAction(nameof(Badges));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBadge(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var badge = await _context.Badges.FindAsync(id);
            if (badge == null) return RedirectToAction(nameof(Badges));

            badge.IsActive = false;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"✅ Đã ẩn badge: {badge.Name}";
            return RedirectToAction(nameof(Badges));
        }

        // ===== STATS DASHBOARD =====
        public async Task<IActionResult> Stats()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var stats = new Dictionary<string, object>
            {
                ["TotalUsers"] = await _context.Users.CountAsync(),
                ["TotalQuests"] = await _context.Quests.CountAsync(q => q.IsActive),
                ["TotalCompletions"] = await _context.UserQuests.CountAsync(),
                ["ActiveToday"] = await _context.UserQuests
                    .Where(uq => uq.CompletedDate == DateTime.Today)
                    .Select(uq => uq.UserId)
                    .Distinct()
                    .CountAsync(),
                ["TopUser"] = await _context.Users
                    .OrderByDescending(u => u.XP)
                    .Select(u => u.Username)
                    .FirstOrDefaultAsync()
            };

            return View(stats);
        }
    }
}