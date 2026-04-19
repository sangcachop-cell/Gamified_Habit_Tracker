using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    /// <summary>
    /// Dashboard của user - hiển thị thống kê, streak, badges
    /// </summary>
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStatisticsService _statisticsService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            AppDbContext context,
            IStatisticsService statisticsService,
            ILogger<DashboardController> logger)
        {
            _context = context;
            _statisticsService = statisticsService;
            _logger = logger;
        }

        // ===== MAIN DASHBOARD =====
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .Include(u => u.UserBadges)
                    .ThenInclude(ub => ub.Badge)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login", "Account");

            // Lấy dữ liệu 7 ngày gần nhất
            var chartData = await GetLast7DaysData(userId.Value);

            ViewBag.ChartLabels = string.Join(",", chartData.Select(x => $"'{x.date}'"));
            ViewBag.ChartData = string.Join(",", chartData.Select(x => x.count));

            _logger.LogInformation($"User {userId} loaded dashboard");

            return View(user);
        }

        // ===== QUESTS HISTORY =====
        [HttpGet("History")]
        public async Task<IActionResult> History(int page = 1, int pageSize = 10)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Lấy UserQuest history
            var history = await _context.UserQuests
                .Where(uq => uq.UserId == userId)
                .Include(uq => uq.Quest)
                .OrderByDescending(uq => uq.CompletedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.UserQuests
                .Where(uq => uq.UserId == userId)
                .CountAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;

            _logger.LogInformation($"User {userId} viewed history page {page}");

            return View(history);
        }

        // ===== BADGES VIEW =====
        [HttpGet("Badges")]
        public async Task<IActionResult> Badges()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var badges = await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .Include(ub => ub.Badge)
                .OrderByDescending(ub => ub.EarnedDate)
                .ToListAsync();

            ViewBag.TotalBadges = badges.Count;

            _logger.LogInformation($"User {userId} viewed {badges.Count} badges");

            return View(badges);
        }

        // ===== ADVANCED STATISTICS WITH CHARTS =====
        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            try
            {
                // Create view model with all statistics
                var stats = new StatisticsViewModel
                {
                    // Quest stats
                    QuestStats = await _statisticsService.GetQuestStatsAsync(userId.Value),

                    // Category breakdown (last 30 days)
                    CategoryBreakdown = await _statisticsService.GetCompletionByCategoryAsync(userId.Value, 30),

                    // XP trend (last 30 days)
                    XPTrend = await _statisticsService.GetXPTrendAsync(userId.Value, 30),

                    // Performance metrics (last 30 days)
                    PerformanceMetrics = await _statisticsService.GetPerformanceMetricsAsync(userId.Value, 30),

                    // Recent achievements
                    RecentAchievements = await _statisticsService.GetRecentAchievementsAsync(userId.Value, 10),

                    // Activity by day of week (last 4 weeks)
                    ActivityByDay = await _statisticsService.GetActivityByDayOfWeekAsync(userId.Value, 4),

                    // Comparison with average
                    Comparison = await _statisticsService.CompareWithAverageAsync(userId.Value, 7)
                };

                _logger.LogInformation($"User {userId} loaded advanced statistics");

                return View(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading statistics for user {userId}: {ex.Message}");
                TempData["Error"] = "Không thể tải thống kê. Vui lòng thử lại sau.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ===== HELPER METHODS =====
        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
        }

        /// <summary>
        /// Lấy dữ liệu hoàn thành quest của 7 ngày gần nhất (cho dashboard chart)
        /// </summary>
        private async Task<List<(string date, int count)>> GetLast7DaysData(int userId)
        {
            try
            {
                var last7Days = Enumerable.Range(0, 7)
                    .Select(i => DateTime.Today.AddDays(-i))
                    .Reverse()
                    .ToList();

                var completions = await _context.UserQuests
                    .Where(uq => uq.UserId == userId &&
                                last7Days.Contains(uq.CompletedDate) &&
                                uq.Status == "Confirmed")
                    .GroupBy(uq => uq.CompletedDate)
                    .Select(g => new { date = g.Key, count = g.Count() })
                    .ToListAsync();

                var result = last7Days.Select(day =>
                {
                    var count = completions
                        .FirstOrDefault(c => c.date == day)?.count ?? 0;

                    return (date: day.ToString("dd/MM"), count);
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting last 7 days data: {ex.Message}");
                return new List<(string, int)>();
            }
        }
    }
}