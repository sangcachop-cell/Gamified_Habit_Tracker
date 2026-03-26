using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    public class HabitController : Controller
    {
        private readonly AppDbContext _context;

        public HabitController(AppDbContext context)
        {
            _context = context;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var habits = _context.Habits
                .Where(h => h.UserId == userId)
                .ToList();

            var now = DateTime.Now;

            foreach (var h in habits)
            {
                bool isDoneToday = _context.HabitLogs
                    .Any(x => x.HabitId == h.Id && x.Date == DateTime.Today);

                // 🔥 MISS nếu quá thời gian (date + time)
                if (h.Deadline != null && h.Deadline < now && !isDoneToday)
                {
                    h.Streak = 0;
                }
            }

            _context.SaveChanges();

            return View(habits);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Habit habit)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            habit.UserId = userId;

            _context.Habits.Add(habit);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= COMPLETE =================
        public IActionResult Complete(int id)
        {
            var habit = _context.Habits.Find(id);

            if (habit == null)
                return RedirectToAction("Index");

            var today = DateTime.Today;

            var log = _context.HabitLogs
                .FirstOrDefault(x => x.HabitId == id && x.Date == today);

            if (log == null)
            {
                // ===== SAVE LOG =====
                _context.HabitLogs.Add(new HabitLog
                {
                    HabitId = id,
                    Date = today,
                    IsCompleted = true
                });

                // ===== STREAK =====
                var yesterday = today.AddDays(-1);

                bool didYesterday = _context.HabitLogs
                    .Any(x => x.HabitId == id && x.Date == yesterday);

                if (didYesterday)
                    habit.Streak++;
                else
                    habit.Streak = 1;

                // ===== XP =====
                var user = _context.Users.Find(habit.UserId);

                user.XP += habit.XPReward;

                if (user.XP >= 100)
                {
                    user.Level++;
                    user.XP -= 100;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            var habit = _context.Habits.Find(id);

            if (habit != null)
            {
                _context.Habits.Remove(habit);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ================= DETAILS =================
        public IActionResult Details(int id)
        {
            var habit = _context.Habits.FirstOrDefault(h => h.Id == id);

            if (habit == null)
                return RedirectToAction("Index");

            return View(habit);
        }
        public IActionResult Suggestion()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var habits = _context.Habits
                .Where(h => h.UserId == userId)
                .ToList();

            var suggestions = new List<string>();

            foreach (var h in habits)
            {
                var logs = _context.HabitLogs
                    .Where(x => x.HabitId == h.Id)
                    .ToList();

                int total = logs.Count;
                int done = logs.Count(x => x.IsCompleted);

                if (total >= 5)
                {
                    double rate = (double)done / total;

                    if (rate < 0.5)
                    {
                        suggestions.Add($"⚠️ '{h.Name}' completion thấp ({rate:P0}), thử giảm độ khó.");
                    }
                    else if (rate > 0.8)
                    {
                        suggestions.Add($"🏆 '{h.Name}' rất tốt ({rate:P0}), hãy nâng cấp!");
                    }
                }

                if (h.Streak == 0 && total > 5)
                {
                    suggestions.Add($"💡 '{h.Name}' hay bị reset streak, thử bắt đầu nhỏ hơn.");
                }

                if (h.ReminderTime != null)
                {
                    var lateCount = logs.Count(x =>
                        x.Date.TimeOfDay > h.ReminderTime);

                    if (lateCount >= 3)
                    {
                        suggestions.Add($"⏰ Bạn hay làm trễ '{h.Name}', thử đổi giờ.");
                    }
                }
            }

            if (!habits.Any(h => h.Name.Contains("Water")))
            {
                suggestions.Add("💧 Bạn nên thêm habit uống nước!");
            }

            if (!habits.Any(h => h.Name.Contains("Exercise")))
            {
                suggestions.Add("🏃 Bạn nên thêm habit vận động!");
            }

            ViewBag.Suggestions = suggestions;

            return View();
        }
    }
}