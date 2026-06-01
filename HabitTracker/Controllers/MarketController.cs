using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class MarketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEconomyService _economyService;

        public MarketController(AppDbContext context, IEconomyService economyService)
        {
            _context = context;
            _economyService = economyService;
        }

        // GET /Market
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            var ownedKeys = await _context.UserGearItems
                .Where(g => g.UserId == userId.Value)
                .Select(g => g.GearItem!.Key)
                .ToHashSetAsync();

            var allGear = await _context.GearItems.ToListAsync();

            // Regular shop items (not armoire)
            var shopGear = allGear.Where(g => !g.IsArmoire).ToList();
            var armoireGear = allGear.Where(g => g.IsArmoire).ToList();

            // Group: GearClass → Slot → items
            var byClassAndSlot = shopGear
                .GroupBy(g => g.GearClass)
                .ToDictionary(
                    grp => grp.Key,
                    grp => grp.GroupBy(g => g.Slot)
                               .ToDictionary(sg => sg.Key, sg => sg.OrderBy(g => g.Tier).ToList())
                );

            var vm = new MarketViewModel
            {
                User = user,
                ByClassAndSlot = byClassAndSlot,
                OwnedKeys = ownedKeys,
                ArmoireItems = armoireGear
            };

            return View(vm);
        }

        // POST /Market/Buy?key=weapon_warrior_1
        [HttpPost("Buy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(string key)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return Json(new { success = false, error = "User not found" });

            var gear = await _context.GearItems.FirstOrDefaultAsync(g => g.Key == key);
            if (gear == null) return Json(new { success = false, error = "Item not found" });

            bool alreadyOwned = await _context.UserGearItems
                .AnyAsync(g => g.UserId == userId.Value && g.GearItem!.Key == key);
            if (alreadyOwned) return Json(new { success = false, error = "Already owned" });

            if (user.Gold < gear.GoldCost)
                return Json(new { success = false, error = $"Need {gear.GoldCost} GP (you have {user.Gold:F1})" });

            // Class check — special/all items can be bought by anyone
            if (gear.GearClass != "all" && gear.GearClass != "special" &&
                gear.GearClass != user.Class)
                return Json(new { success = false, error = $"Requires {gear.GearClass} class" });

            user.Gold -= gear.GoldCost;
            _context.UserGearItems.Add(new UserGearItem
            {
                UserId = userId.Value,
                GearItemId = gear.Id,
                AcquiredAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return Json(new { success = true, newGold = user.Gold });
        }

        // POST /Market/PullArmoire
        [HttpPost("PullArmoire")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PullArmoire()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var result = await _economyService.PullArmoireAsync(userId.Value);

            var user = await _context.Users.FindAsync(userId.Value);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                rewardType = result.RewardType.ToString().ToLower(),
                gearKey = result.GearKey,
                gearName = result.GearName,
                gearImgPath = result.GearImgPath,
                foodIcon = result.FoodIcon,
                foodName = result.FoodName,
                xpGained = result.XpGained,
                newGold = user?.Gold ?? 0
            });
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}