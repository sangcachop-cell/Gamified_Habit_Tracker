using HabitTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(userId);

            // ===== LẤY 7 NGÀY =====
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i))
                .Reverse()
                .ToList();

            var data = last7Days.Select(day => new
            {
                date = day.ToString("dd/MM"),
                count = _context.HabitLogs
                    .Count(x => x.Date == day && x.Habit.UserId == userId)
            });

            ViewBag.ChartLabels = string.Join(",", data.Select(x => $"'{x.date}'"));
            ViewBag.ChartData = string.Join(",", data.Select(x => x.count));

            return View(user);
        }
    }
}