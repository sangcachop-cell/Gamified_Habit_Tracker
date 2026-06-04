using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace HabitTracker.Controllers
{
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public SettingsController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET /Settings
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            return View(user);
        }

        // POST /Settings/SaveDayStart
        [HttpPost]
        public async Task<IActionResult> SaveDayStart(int dayStart)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in." });

            if (dayStart < 0 || dayStart > 23)
                return Json(new { success = false, error = "Day start must be 0–23." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Json(new { success = false, error = "User not found." });

            user.DayStart = dayStart;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // POST /Settings/SavePreferences
        [HttpPost]
        public async Task<IActionResult> SavePreferences(bool suppressNotifications)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Json(new { success = false, error = "User not found." });

            user.SuppressNotifications = suppressNotifications;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // POST /Settings/SavePrivacy
        [HttpPost]
        public async Task<IActionResult> SavePrivacy(string pmPermission, string profileVisibility)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in." });

            var validPM = new[] { "everyone", "nobody" };
            var validVis = new[] { "public", "private" };

            if (!validPM.Contains(pmPermission) || !validVis.Contains(profileVisibility))
                return Json(new { success = false, error = "Invalid value." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Json(new { success = false, error = "User not found." });

            user.PMPermission = pmPermission;
            user.ProfileVisibility = profileVisibility;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // GET /Settings/GetApiToken  — returns or generates token
        [HttpGet]
        public async Task<IActionResult> GetApiToken()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Json(new { success = false, error = "User not found." });

            if (string.IsNullOrEmpty(user.ApiToken))
            {
                user.ApiToken = GenerateToken();
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, token = user.ApiToken });
        }

        // POST /Settings/RegenerateToken
        [HttpPost]
        public async Task<IActionResult> RegenerateToken()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Json(new { success = false, error = "User not found." });

            user.ApiToken = GenerateToken();
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Json(new { success = true, token = user.ApiToken });
        }

        // GET /Settings/ExportData
        [HttpGet]
        public async Task<IActionResult> ExportData()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.UserBadges!).ThenInclude(ub => ub.Badge)
                .Include(u => u.OwnedGear!)
                .Include(u => u.OwnedPets!)
                .Include(u => u.InventoryItems!)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return RedirectToAction("Login", "Account");

            var tasks = await _context.HabitTasks
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .Select(t => new { t.Text, t.Type, t.Priority, t.IsCompleted, t.CreatedAt })
                .ToListAsync();

            var export = new
            {
                ExportedAt = DateTime.UtcNow,
                Account = new { user.Username, user.Email, user.CreatedAt },
                Character = new
                {
                    user.Level, user.XP, user.HP, user.Mana, user.Gold, user.Gems,
                    user.Class, user.STR, user.CON, user.INT, user.PER,
                    user.CurrentStreak, user.LongestStreak,
                    user.TotalTasksCompleted, user.PerfectDayCount
                },
                Tasks = tasks,
                Badges = user.UserBadges?.Select(ub => ub.Badge?.Name).ToList(),
                OwnedGearCount = user.OwnedGear?.Count ?? 0,
                OwnedPetsCount = user.OwnedPets?.Count ?? 0,
                InventoryItemsCount = user.InventoryItems?.Count ?? 0
            };

            var json = JsonSerializer.Serialize(export, new JsonSerializerOptions { WriteIndented = true });
            var bytes = Encoding.UTF8.GetBytes(json);
            var filename = $"habittracker_export_{user.Username}_{DateTime.UtcNow:yyyyMMdd}.json";

            return File(bytes, "application/json", filename);
        }

        // POST /Settings/ResetProgress — wipe tasks/HP/XP/gold, keep account
        [HttpPost]
        public async Task<IActionResult> ResetProgress(string password)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Json(new { success = false, error = "User not found." });

            if (!_authService.VerifyPassword(password, user.Password))
                return Json(new { success = false, error = "Incorrect password." });

            // Wipe game progress
            user.Level              = 1;
            user.XP                 = 0;
            user.HP                 = 50.0;
            user.Mana               = 30.0;
            user.Gold               = 0.0;
            user.Gems               = 0;
            user.CurrentStreak      = 0;
            user.LongestStreak      = 0;
            user.TotalTasksCompleted = 0;
            user.PerfectDayCount    = 0;
            user.STR = user.CON = user.INT = user.PER = 0;
            user.StatPoints         = 0;
            user.BuffSTR = user.BuffCON = user.BuffINT = user.BuffPER = 0;
            user.BuffExpiry         = null;
            user.StealthBuff        = 0;
            user.FrozenStreaks       = false;
            user.IsSleeping         = false;
            user.LastCronDate       = null;
            user.LastCheckInDate    = null;
            user.LastCompletedDate  = null;
            user.DailyDropCount     = 0;
            user.RebirthCount       = 0;
            user.UpdatedAt          = DateTime.UtcNow;

            // Remove tasks
            var tasks = await _context.HabitTasks.Where(t => t.UserId == userId).ToListAsync();
            _context.HabitTasks.RemoveRange(tasks);

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);

        private static string GenerateToken()
            => Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
    }
}
