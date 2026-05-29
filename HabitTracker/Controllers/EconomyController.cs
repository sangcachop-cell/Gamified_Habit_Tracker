using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class EconomyController : Controller
    {
        private readonly IEconomyService   _economyService;
        private readonly AppDbContext      _context;
        private readonly IQuestService     _questService;
        private readonly ICharacterService _characterService;

        public EconomyController(IEconomyService economyService, AppDbContext context, IQuestService questService, ICharacterService characterService)
        {
            _economyService   = economyService;
            _context          = context;
            _questService     = questService;
            _characterService = characterService;
        }

        // GET /Economy
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user      = await _context.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            var inventory = await _economyService.GetInventoryAsync(userId.Value);

            ViewBag.User      = user;
            ViewBag.Inventory = inventory;
            return View();
        }

        // GET /Economy/GetStats — AJAX HUD endpoint
        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            var userId = GetUserId();
            if (userId == null) return Json(null);

            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user == null) return Json(null);

            var effStats      = await _characterService.GetEffectiveStatsAsync(user);
            int maxMana       = effStats.MaxMana;
            int xpToNextLevel = _questService.XpToNextLevel(user.Level);
            int xpWithinLevel = _questService.XpWithinLevel(user.XP);

            return Json(new
            {
                hp            = user.HP,
                maxHp         = (int)AppConstants.MAX_HP,
                mana          = user.Mana,
                maxMana,
                gold          = user.Gold,
                level         = user.Level,
                xp            = user.XP,
                xpToNext      = xpToNextLevel,   // replaced hardcoded constant
                xpWithinLevel,
                xpToNextLevel,
                isSleeping    = user.IsSleeping,
                isDead        = user.HP <= 0,
                // Phase 3 — character stats
                str           = user.STR,
                con           = user.CON,
                intel         = user.INT,         // 'int' is a C# keyword; use 'intel'
                per           = user.PER,
                statPoints    = user.StatPoints,
                // Phase 4
                gems          = user.Gems
            });
        }

        // POST /Economy/BuyPotion
        [HttpPost("BuyPotion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyPotion()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var (success, error) = await _economyService.BuyHealthPotionAsync(userId.Value);

            var user = await _context.Users.FindAsync(userId.Value);
            return Json(new
            {
                success,
                error,
                newHp   = user?.HP   ?? 0,
                newGold = user?.Gold ?? 0
            });
        }

        // POST /Economy/ToggleSleep
        [HttpPost("ToggleSleep")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSleep()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false });

            bool isSleeping = await _economyService.ToggleSleepAsync(userId.Value);
            return Json(new { success = true, isSleeping });
        }

        // POST /Economy/Revive
        [HttpPost("Revive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revive()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false });

            await _economyService.ReviveAsync(userId.Value);

            var user = await _context.Users.FindAsync(userId.Value);
            return Json(new
            {
                success  = true,
                newHp    = user?.HP    ?? 0,
                newGold  = user?.Gold  ?? 0,
                newLevel = user?.Level ?? 1,
                newXp    = user?.XP    ?? 0
            });
        }

        // POST /Economy/BuyGem
        [HttpPost("BuyGem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyGem()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var (success, error) = await _economyService.BuyGemAsync(userId.Value);

            var user = await _context.Users.FindAsync(userId.Value);
            return Json(new
            {
                success,
                error,
                newGems = user?.Gems ?? 0,
                newGold = user?.Gold ?? 0
            });
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}
