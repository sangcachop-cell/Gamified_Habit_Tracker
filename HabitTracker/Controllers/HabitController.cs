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

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var habits = _context.Habits
                .Where(h => h.UserId == userId)
                .ToList();

            return View(habits);
        }

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

        public IActionResult Complete(int id)
        {
            var habit = _context.Habits.Find(id);

            if (habit != null)
            {
                var today = DateTime.Today;

                if (habit.LastCompletedDate != today)
                {
                    var user = _context.Users.Find(habit.UserId);

                    // XP
                    user.XP += habit.XPReward;

                    if (user.XP >= 100)
                    {
                        user.Level++;
                        user.XP -= 100;
                    }

                    // STREAK
                    if (habit.LastCompletedDate == today.AddDays(-1))
                        habit.Streak++;
                    else
                        habit.Streak = 1;

                    habit.LastCompletedDate = today;

                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var habits = _context.Habits
                .Where(h => h.UserId == userId)
                .ToList();

            var today = DateTime.Today;

            int total = habits.Count;
            int done = habits.Count(h => h.LastCompletedDate == today);

            ViewBag.Percent = total == 0 ? 0 : (done * 100 / total);

            return View(habits);
        }

        private void UpdateStreak(User user)
        {
            var today = DateTime.Today;

            if (user.LastCheckInDate == null)
            {
                user.CurrentStreak = 1;
            }
            else if (user.LastCheckInDate.Value.Date == today.AddDays(-1))
            {
                user.CurrentStreak++;
            }
            else if (user.LastCheckInDate.Value.Date != today)
            {
                user.CurrentStreak = 1;
            }

            user.LastCheckInDate = today;

            if (user.CurrentStreak > user.LongestStreak)
            {
                user.LongestStreak = user.CurrentStreak;
            }
        }

        private void CheckBadge(User user)
        {
            var badges = _context.Badges.ToList();

            foreach (var badge in badges)
            {
                bool has = _context.UserBadges
                    .Any(x => x.UserId == user.Id && x.BadgeId == badge.Id);

                if (!has && user.XP >= badge.RequiredXP)
                {
                    _context.UserBadges.Add(new UserBadge
                    {
                        UserId = user.Id,
                        BadgeId = badge.Id
                    });
                }
            }
        }
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
        public IActionResult Details(int id)
        {
            var habit = _context.Habits
                .FirstOrDefault(h => h.Id == id);

            if (habit == null)
                return RedirectToAction("Index");

            return View(habit);
        }
    }
}