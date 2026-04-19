using System.Diagnostics;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel
            {
                TotalUsers = await _context.Users.CountAsync(u => !u.IsAdmin),
                TotalQuestsCompleted = await _context.UserQuests.CountAsync(),
                TotalBadges = await _context.UserBadges.CountAsync(),
                TotalQuests = await _context.Quests.CountAsync(q => q.IsActive),
            };

            // Top 5 users by XP (exclude admin)
            var topUsers = await _context.Users
                .Where(u => !u.IsAdmin)
                .Include(u => u.UserBadges)
                    .ThenInclude((UserBadge ub) => ub.Badge)
                .OrderByDescending(u => u.XP)
                .Take(5)
                .ToListAsync();

            vm.TopUsers = topUsers.Select(u =>
            {
                // Pick the rarest badge
                var rarityOrder = new[] { "Legendary", "Epic", "Rare", "Common" };
                var topBadge = u.UserBadges?
                    .Where(ub => ub.Badge != null)
                    .OrderBy(ub => Array.IndexOf(rarityOrder, ub.Badge!.Rarity))
                    .FirstOrDefault();

                return new LeaderboardEntry
                {
                    Username = u.Username,
                    Avatar = u.Avatar,
                    Level = u.Level,
                    XP = u.XP,
                    CurrentStreak = u.CurrentStreak,
                    TopBadgeName = topBadge?.Badge?.Name,
                    TopBadgeIcon = topBadge?.Badge?.Icon,
                    TopBadgeRarity = topBadge?.Badge?.Rarity,
                };
            }).ToList();

            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}