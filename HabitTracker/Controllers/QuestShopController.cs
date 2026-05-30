using HabitTracker.Constants;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class QuestShopController : Controller
    {
        private readonly IBossQuestService _bossQuestService;
        private readonly AppDbContext _db;

        public QuestShopController(IBossQuestService bossQuestService, AppDbContext db)
        {
            _bossQuestService = bossQuestService;
            _db               = db;
        }

        // GET /QuestShop
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return RedirectToAction("Login", "Account");

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) return RedirectToAction("Login", "Account");

            var quests = await _bossQuestService.GetShopQuestsAsync(userId.Value);

            // Owned quest scrolls: key → quantity
            var ownedScrolls = await _db.UserInventoryItems
                .AsNoTracking()
                .Where(i => i.UserId == userId && i.GameItem!.Type == ItemType.QuestScroll)
                .Select(i => new { i.GameItem!.Key, i.Quantity })
                .ToDictionaryAsync(x => x.Key.Replace("quest_", ""), x => x.Quantity);

            // Completed quest keys (for prereq checks)
            var completedKeys = await _db.PartyQuests
                .AsNoTracking()
                .Where(pq => pq.LeaderUserId == userId && pq.Status == "Complete")
                .Select(pq => pq.BossQuest!.Key)
                .Distinct()
                .ToHashSetAsync();

            var vm = new QuestShopViewModel
            {
                Quests             = quests,
                OwnedScrolls       = ownedScrolls,
                UserGold           = (int)user.Gold,
                UserGems           = user.Gems,
                UserLevel          = user.Level,
                CompletedQuestKeys = completedKeys,
            };

            return View(vm);
        }

        // POST /QuestShop/Buy
        [HttpPost("Buy")]
        public async Task<IActionResult> Buy([FromBody] BuyScrollRequest req)
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { success = false, error = "Not logged in." });

            var (success, error) = await _bossQuestService.BuyScrollAsync(userId.Value, req.QuestKey);
            if (!success) return Json(new { success = false, error });

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            return Json(new { success = true, newGold = (int)(user?.Gold ?? 0), newGems = user?.Gems ?? 0 });
        }

        public sealed class BuyScrollRequest
        {
            public string QuestKey { get; set; } = string.Empty;
        }
    }
}
