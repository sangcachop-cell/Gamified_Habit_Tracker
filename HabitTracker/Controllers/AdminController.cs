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

        // ===== USER MANAGEMENT =====

        public async Task<IActionResult> Users(string? q)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var query = _context.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                if (int.TryParse(q, out var uid))
                    query = query.Where(u => u.Id == uid);
                else
                    query = query.Where(u => u.Username.Contains(q) || u.Email.Contains(q));
            }

            var users = await query.OrderByDescending(u => u.Id).Take(200).ToListAsync();
            ViewBag.Q = q;
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var user = await _context.Users.FindAsync(id);
            if (user == null) { TempData["Error"] = "User not found."; return RedirectToAction(nameof(Users)); }

            user.IsAdmin = !user.IsAdmin;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Admin toggled for {user.Username}.";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MuteUser(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var user = await _context.Users.FindAsync(id);
            if (user == null) { TempData["Error"] = "User not found."; return RedirectToAction(nameof(Users)); }

            user.IsMuted = !user.IsMuted;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"{user.Username} is now {(user.IsMuted ? "muted" : "unmuted")}.";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var user = await _context.Users.FindAsync(id);
            if (user == null) { TempData["Error"] = "User not found."; return RedirectToAction(nameof(Users)); }

            var adminId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (user.Id == adminId) { TempData["Error"] = "Cannot ban yourself."; return RedirectToAction(nameof(Users)); }

            user.IsBanned = !user.IsBanned;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"{user.Username} is now {(user.IsBanned ? "banned" : "unbanned")}.";
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> UserDetail(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.UserBadges).ThenInclude(ub => ub.Badge)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var tasks = await _context.HabitTasks
                .AsNoTracking()
                .Where(t => t.UserId == id)
                .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
                .Take(50)
                .ToListAsync();

            var gear = await _context.UserGearItems
                .AsNoTracking()
                .Include(g => g.GearItem)
                .Where(g => g.UserId == id)
                .ToListAsync();

            var inventory = await _context.UserInventoryItems
                .AsNoTracking()
                .Include(i => i.GameItem)
                .Where(i => i.UserId == id && i.Quantity > 0)
                .ToListAsync();

            ViewBag.Tasks     = tasks;
            ViewBag.Gear      = gear;
            ViewBag.Inventory = inventory;
            return View(user);
        }

        // ===== EMAIL / IP BLOCKLIST =====

        public async Task<IActionResult> Blocklist()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var entries = await _context.AdminBlocklistEntries
                .AsNoTracking()
                .OrderByDescending(e => e.AddedAt)
                .ToListAsync();

            return View(entries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBlocklist(string type, string value, string? note)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            if (string.IsNullOrWhiteSpace(value))
            {
                TempData["Error"] = "Value is required.";
                return RedirectToAction(nameof(Blocklist));
            }

            var adminId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            _context.AdminBlocklistEntries.Add(new AdminBlocklistEntry
            {
                Type           = type == "ip" ? "ip" : "email",
                Value          = value.Trim().ToLower(),
                Note           = note?.Trim(),
                AddedAt        = DateTime.UtcNow,
                AddedByAdminId = adminId
            });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Blocklisted: {value}";
            return RedirectToAction(nameof(Blocklist));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBlocklist(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var entry = await _context.AdminBlocklistEntries.FindAsync(id);
            if (entry != null)
            {
                _context.AdminBlocklistEntries.Remove(entry);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Removed: {entry.Value}";
            }
            return RedirectToAction(nameof(Blocklist));
        }

        // ===== GROUP MANAGEMENT =====

        public async Task<IActionResult> Groups()
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var guilds = await _context.Guilds
                .AsNoTracking()
                .Include(g => g.Members)
                .OrderByDescending(g => g.Id)
                .ToListAsync();

            var parties = await _context.Parties
                .AsNoTracking()
                .Include(p => p.Members)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            ViewBag.Guilds  = guilds;
            ViewBag.Parties = parties;
            return View();
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

        // GET /Admin/Reports
        public async Task<IActionResult> Reports(bool showResolved = false)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var reports = await _context.Reports
                .AsNoTracking()
                .Where(r => showResolved ? r.IsResolved : !r.IsResolved)
                .Include(r => r.Reporter)
                .Include(r => r.ReportedUser)
                .Include(r => r.ReportedMessage)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            ViewBag.ShowResolved = showResolved;
            return View(reports);
        }

        // POST /Admin/ResolveReport/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveReport(int id)
        {
            var adminCheck = CheckAdmin();
            if (adminCheck != null) return adminCheck;

            var report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();

            var adminId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            report.IsResolved = true;
            report.ResolvedAt = DateTime.UtcNow;
            report.ResolvedByAdminId = adminId;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Report resolved.";
            return RedirectToAction(nameof(Reports));
        }
    }
}