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
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var user = _context.Users.Find(userId);

            return View(user);
        }
    }
}