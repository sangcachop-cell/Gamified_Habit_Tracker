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
    public class EquipmentController : Controller
    {
        private readonly AppDbContext      _context;
        private readonly ICharacterService _characterService;

        public EquipmentController(AppDbContext context, ICharacterService characterService)
        {
            _context          = context;
            _characterService = characterService;
        }

        // GET /Equipment
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            var ownedKeys = user.OwnedGear!
                .Where(ug => ug.GearItem != null)
                .Select(ug => ug.GearItem!.Key)
                .ToHashSet();

            var ownedBySlot = user.OwnedGear!
                .Where(ug => ug.GearItem != null)
                .Select(ug => ug.GearItem!)
                .GroupBy(g => g.Slot)
                .ToDictionary(g => g.Key, g => g.OrderBy(i => i.Tier).ToList());

            // Shop: gear for user's class + "all" class (show all tiers including owned)
            var userClass = user.Class ?? "";
            var shopItems = await _context.GearItems
                .Where(g => g.GearClass == userClass || g.GearClass == "all")
                .OrderBy(g => g.Slot).ThenBy(g => g.Tier)
                .ToListAsync();
            var shopBySlot = shopItems
                .GroupBy(g => g.Slot)
                .ToDictionary(g => g.Key, g => g.ToList());

            var stats = await _characterService.GetEffectiveStatsAsync(user);

            var vm = new EquipmentViewModel
            {
                User        = user,
                OwnedBySlot = ownedBySlot,
                ShopBySlot  = shopBySlot,
                OwnedKeys   = ownedKeys,
                Stats       = stats,
                EquippedKeys = new Dictionary<string, string?>
                {
                    ["weapon"]        = user.EquippedWeapon,
                    ["armor"]         = user.EquippedArmor,
                    ["head"]          = user.EquippedHead,
                    ["shield"]        = user.EquippedShield,
                    ["back"]          = user.EquippedBack,
                    ["eyewear"]       = user.EquippedEyewear,
                    ["headAccessory"] = user.EquippedHeadAccessory,
                    ["body"]          = user.EquippedBody,
                },
                CostumeKeys = new Dictionary<string, string?>
                {
                    ["weapon"]        = user.CostumeWeapon,
                    ["armor"]         = user.CostumeArmor,
                    ["head"]          = user.CostumeHead,
                    ["shield"]        = user.CostumeShield,
                    ["back"]          = user.CostumeBack,
                    ["eyewear"]       = user.CostumeEyewear,
                    ["headAccessory"] = user.CostumeHeadAccessory,
                    ["body"]          = user.CostumeBody,
                },
            };

            return View(vm);
        }

        // POST /Equipment/Equip?key=weapon_warrior_1&mode=equipped
        [HttpPost("Equip")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Equip(string key, string mode = "equipped")
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user == null) return Json(new { success = false, error = "User not found" });

            // Validate ownership
            bool owns = user.OwnedGear!.Any(ug => ug.GearItem?.Key == key);
            if (!owns) return Json(new { success = false, error = "You don't own that item" });

            var gear = await _context.GearItems.FirstOrDefaultAsync(g => g.Key == key);
            if (gear == null) return Json(new { success = false, error = "Item not found" });

            bool isCostume = mode == "costume";

            // Shield blocked when wearing a two-handed weapon
            if (gear.Slot == "shield" && !isCostume)
            {
                var wepKey = user.EquippedWeapon;
                if (wepKey != null)
                {
                    var wep = await _context.GearItems.FirstOrDefaultAsync(g => g.Key == wepKey);
                    if (wep?.TwoHanded == true)
                        return Json(new { success = false, error = "Can't equip shield with a two-handed weapon" });
                }
            }
            if (gear.Slot == "shield" && isCostume)
            {
                var wepKey = user.CostumeWeapon;
                if (wepKey != null)
                {
                    var wep = await _context.GearItems.FirstOrDefaultAsync(g => g.Key == wepKey);
                    if (wep?.TwoHanded == true)
                        return Json(new { success = false, error = "Can't equip shield with a two-handed weapon" });
                }
            }

            // Two-handed weapon auto-clears shield slot
            if (gear.TwoHanded)
            {
                if (isCostume) user.CostumeShield  = null;
                else           user.EquippedShield  = null;
            }

            SetSlot(user, gear.Slot, key, isCostume);
            await _context.SaveChangesAsync();

            // Re-compute stats (gear loaded from OwnedGear navigation)
            var stats = await _characterService.GetEffectiveStatsAsync(user);

            return Json(new
            {
                success     = true,
                slot        = gear.Slot,
                key,
                mode,
                twoHanded   = gear.TwoHanded,
                wornImgPath = gear.GetWornImagePath(user.BodyType ?? "broad"),
                shopImgPath = gear.ShopImagePath,
                itemName    = gear.Name,
                newStats    = new { stats.STR, stats.CON, stats.INT, stats.PER, stats.MaxMana },
            });
        }

        // POST /Equipment/Unequip?slot=weapon&mode=equipped
        [HttpPost("Unequip")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unequip(string slot, string mode = "equipped")
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user == null) return Json(new { success = false, error = "User not found" });

            SetSlot(user, slot, null, mode == "costume");
            await _context.SaveChangesAsync();

            var stats = await _characterService.GetEffectiveStatsAsync(user);

            return Json(new
            {
                success  = true,
                slot,
                mode,
                newStats = new { stats.STR, stats.CON, stats.INT, stats.PER, stats.MaxMana },
            });
        }

        // POST /Equipment/ToggleCostume
        [HttpPost("ToggleCostume")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCostume()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return Json(new { success = false, error = "User not found" });

            user.CostumeModeEnabled = !user.CostumeModeEnabled;
            await _context.SaveChangesAsync();

            return Json(new { success = true, costumeEnabled = user.CostumeModeEnabled });
        }

        // POST /Equipment/Buy?key=weapon_warrior_1
        [HttpPost("Buy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(string key)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user == null) return Json(new { success = false, error = "User not found" });

            var gear = await _context.GearItems.FirstOrDefaultAsync(g => g.Key == key);
            if (gear == null) return Json(new { success = false, error = "Item not found" });

            // Only buy gear for user's own class (or "all" class items)
            if (gear.GearClass != "all" && gear.GearClass != (user.Class ?? ""))
                return Json(new { success = false, error = "That item is for a different class" });

            // Already owned?
            bool alreadyOwns = user.OwnedGear!.Any(ug => ug.GearItem?.Key == key);
            if (alreadyOwns) return Json(new { success = false, error = "You already own this item" });

            if (user.Gold < gear.GoldCost)
                return Json(new { success = false, error = $"Need {gear.GoldCost} GP (you have {user.Gold:F0})" });

            user.Gold -= gear.GoldCost;
            _context.UserGearItems.Add(new UserGearItem
            {
                UserId     = userId.Value,
                GearItemId = gear.Id,
                AcquiredAt = DateTime.UtcNow,
            });
            await _context.SaveChangesAsync();

            return Json(new
            {
                success     = true,
                key,
                itemName    = gear.Name,
                slot        = gear.Slot,
                shopImgPath = gear.ShopImagePath,
                newGold     = Math.Round(user.Gold, 2),
            });
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private static void SetSlot(User user, string slot, string? key, bool costume)
        {
            if (costume)
            {
                switch (slot)
                {
                    case "weapon":        user.CostumeWeapon        = key; break;
                    case "armor":         user.CostumeArmor         = key; break;
                    case "head":          user.CostumeHead          = key; break;
                    case "shield":        user.CostumeShield        = key; break;
                    case "back":          user.CostumeBack          = key; break;
                    case "eyewear":       user.CostumeEyewear       = key; break;
                    case "headAccessory": user.CostumeHeadAccessory = key; break;
                    case "body":          user.CostumeBody          = key; break;
                }
            }
            else
            {
                switch (slot)
                {
                    case "weapon":        user.EquippedWeapon        = key; break;
                    case "armor":         user.EquippedArmor         = key; break;
                    case "head":          user.EquippedHead          = key; break;
                    case "shield":        user.EquippedShield        = key; break;
                    case "back":          user.EquippedBack          = key; break;
                    case "eyewear":       user.EquippedEyewear       = key; break;
                    case "headAccessory": user.EquippedHeadAccessory = key; break;
                    case "body":          user.EquippedBody          = key; break;
                }
            }
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}
