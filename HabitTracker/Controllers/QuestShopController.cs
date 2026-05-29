using HabitTracker.Constants;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class QuestShopController : Controller
    {
        // GET /QuestShop
        [HttpGet("")]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId == null) return RedirectToAction("Login", "Account");
            return View();
        }
    }
}
