using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        private readonly IEconomyService _economyService;
        private readonly AppDbContext    _context;

        public InventoryController(IEconomyService economyService, AppDbContext context)
        {
            _economyService = economyService;
            _context        = context;
        }

        // GET /Inventory
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            var items = await _economyService.GetInventoryAsync(userId.Value);

            var ownedGear = await _context.UserGearItems
                .Where(g => g.UserId == userId.Value)
                .Include(g => g.GearItem)
                .OrderBy(g => g.GearItem!.Slot)
                .ThenBy(g => g.GearItem!.Tier)
                .ToListAsync();

            var vm = new InventoryViewModel
            {
                User      = user,
                Items     = items,
                OwnedGear = ownedGear
            };

            return View(vm);
        }

        // POST /Inventory/Sell?gameItemId=X
        [HttpPost("Sell")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sell(int gameItemId)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var (success, error, goldAwarded) = await _economyService.SellItemAsync(userId.Value, gameItemId);

            var user = await _context.Users.FindAsync(userId.Value);
            return Json(new
            {
                success,
                error,
                goldAwarded,
                newGold = user?.Gold ?? 0
            });
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}
