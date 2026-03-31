using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // ===== INDEX =====
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var tasks = _context.Tasks
                .Where(t => t.UserId == userId && t.IsActive)
                .ToList();

            return View(tasks);
        }

        // ===== CREATE =====
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Models.Task task)
        {
            task.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ===== COMPLETE =====
        public IActionResult Complete(int id)
        {
            var task = _context.Tasks.Find(id);

            if (task == null) return RedirectToAction("Index");

            var today = DateTime.Today;

            var log = _context.TaskLogs
                .FirstOrDefault(x => x.TaskId == id && x.Date == today);

            if (log == null)
            {
                _context.TaskLogs.Add(new TaskLog
                {
                    TaskId = id,
                    Date = today,
                    IsCompleted = true
                });

                var user = _context.Users.Find(task.UserId);

                // 🎮 XP PRO MAX
                int bonus = 0;

                if (task.Type == "Daily") bonus = 5;
                if (task.Type == "Weekly") bonus = 15;
                if (task.Type == "Monthly") bonus = 30;

                int totalXP = task.XP + bonus;

                // 🔥 streak bonus
                int streak = _context.TaskLogs
                    .Count(x => x.TaskId == id && x.IsCompleted);

                totalXP += streak * 2;

                user.XP += totalXP;

                // LEVEL UP
                if (user.XP >= 100)
                {
                    user.Level++;
                    user.XP -= 100;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ===== DELETE =====
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Find(id);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}