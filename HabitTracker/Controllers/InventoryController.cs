using HabitTracker.Constants;
using HabitTracker.Data;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventory;
        private readonly AppDbContext _context;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventory, AppDbContext context, ILogger<InventoryController> logger)
        {
            _inventory = inventory;
            _context   = context;
            _logger    = logger;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            await _inventory.EnsureStarterItemsAsync(userId.Value);

            var user     = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var storage  = await _inventory.GetItemsAsync(userId.Value, ItemCatalogue.STORAGE);
            var backpack = await _inventory.GetItemsAsync(userId.Value, ItemCatalogue.BACKPACK);

            int storageLevel = await _context.UserFacilities
                .Where(uf => uf.UserId == userId && uf.FacilityId == FacilityCatalogue.STORAGE_FACILITY_ID)
                .Select(uf => uf.Level)
                .FirstOrDefaultAsync();
            if (storageLevel == 0) storageLevel = 1;
            var (sCols, sRows) = ItemCatalogue.StorageSizeForLevel(storageLevel);
            ViewBag.StorageCols = sCols;
            ViewBag.StorageRows = sRows;

            ViewBag.StorageItems  = BuildPlaced(storage);
            ViewBag.BackpackItems = BuildPlaced(backpack);

            // Equipment state
            ViewBag.EquippedBackpackItem = user?.EquippedBackpackItem;
            ViewBag.EquippedArmorItem    = user?.EquippedArmorItem;
            ViewBag.EquippedRigItem      = user?.EquippedRigItem;

            // Equipped container grids
            if (user?.EquippedBackpackItem != null)
            {
                var bp = await _inventory.GetItemsAsync(userId.Value, ItemCatalogue.EQUIPPED_BACKPACK);
                ViewBag.EquippedBackpackItems = BuildPlaced(bp);
            }
            if (user?.EquippedRigItem != null)
            {
                var rig = await _inventory.GetItemsAsync(userId.Value, ItemCatalogue.EQUIPPED_RIG);
                ViewBag.EquippedRigItems = BuildPlaced(rig);
            }

            _logger.LogInformation($"User {userId} opened inventory");
            return View();
        }

        [HttpPost("Move")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move(int itemId, string targetContainer, int targetX, int targetY)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            bool ok = await _inventory.MoveItemAsync(userId.Value, itemId, targetContainer, targetX, targetY);
            return Json(new { success = ok, error = ok ? null : "Position out of bounds or overlaps another item" });
        }

        [HttpPost("Rotate/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rotate(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            bool ok = await _inventory.RotateItemAsync(userId.Value, id);
            if (!ok)
                TempData["InventoryInfo"] = "Cannot rotate — item would go out of bounds.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Equip/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Equip(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            bool ok = await _inventory.EquipItemAsync(userId.Value, id);
            TempData["InventoryInfo"] = ok ? "Item equipped!" : "Cannot equip that item.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Unequip/{slot}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unequip(string slot)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            bool ok = await _inventory.UnequipSlotAsync(userId.Value, slot);
            TempData["InventoryInfo"] = ok ? "Item unequipped." : "Cannot unequip — storage full.";
            return RedirectToAction(nameof(Index));
        }

        // ── helpers ──────────────────────────────────────────────────────────

        private static List<PlacedItem> BuildPlaced(IEnumerable<UserInventoryItem> items)
        {
            var result = new List<PlacedItem>();
            foreach (var item in items)
            {
                if (!ItemCatalogue.Items.TryGetValue(item.ItemId, out var def)) continue;
                int w = item.IsRotated ? def.Height : def.Width;
                int h = item.IsRotated ? def.Width  : def.Height;

                result.Add(new PlacedItem(
                    Id:          item.Id,
                    ItemId:      item.ItemId,
                    Name:        def.Name,
                    Icon:        def.Icon,
                    Description: def.Description,
                    Category:    def.Category,
                    TileColor:   def.TileColor,
                    GridX:       item.GridX,
                    GridY:       item.GridY,
                    W:           w,
                    H:           h,
                    IsRotated:   item.IsRotated,
                    CanRotate:   ItemCatalogue.CanRotate(item.ItemId),
                    Container:   item.ContainerType
                ));
            }
            return result;
        }

        private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }

    // ── Shared view-model record ─────────────────────────────────────────────
    public record PlacedItem(
        int    Id,
        string ItemId,
        string Name,
        string Icon,
        string Description,
        string Category,
        string TileColor,
        int    GridX,
        int    GridY,
        int    W,
        int    H,
        bool   IsRotated,
        bool   CanRotate,
        string Container
    );
}
