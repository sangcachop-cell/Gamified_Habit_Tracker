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
    public class CharacterController : Controller
    {
        private readonly AppDbContext        _context;
        private readonly ICharacterService   _characterService;
        private readonly IQuestService       _questService;
        private readonly ISpellService       _spellService;
        private readonly IWebHostEnvironment _env;

        public CharacterController(
            AppDbContext        context,
            ICharacterService   characterService,
            IQuestService       questService,
            ISpellService       spellService,
            IWebHostEnvironment env)
        {
            _context          = context;
            _characterService = characterService;
            _questService     = questService;
            _spellService     = spellService;
            _env              = env;
        }

        // GET /Character
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .Include(u => u.OwnedGear!)
                    .ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null) return RedirectToAction("Login", "Account");

            var effectiveStats = await _characterService.GetEffectiveStatsAsync(user);

            var vm = new CharacterViewModel
            {
                User           = user,
                EffectiveStats = effectiveStats,
                XpWithinLevel  = _questService.XpWithinLevel(user.XP),
                XpToNextLevel  = _questService.XpToNextLevel(user.Level),
            };

            return View(vm);
        }

        // POST /Character/AllocateStat?stat=STR
        [HttpPost("AllocateStat")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateStat([FromQuery] string stat)
        {
            var userId = GetUserId();
            if (userId == null)
                return Json(new { success = false, error = "Not logged in" });

            var (success, error) = await _characterService.AllocateStatAsync(userId.Value, stat);

            if (!success)
                return Json(new { success = false, error });

            // Return fresh stat values for client-side update
            var user = await _context.Users.FindAsync(userId.Value);
            return Json(new
            {
                success,
                newStatPoints = user?.StatPoints ?? 0,
                newSTR        = user?.STR ?? 0,
                newCON        = user?.CON ?? 0,
                newINT        = user?.INT ?? 0,
                newPER        = user?.PER ?? 0,
            });
        }

        // ===== CLASS SELECTION =====

        // GET /Character/SelectClass
        [HttpGet("SelectClass")]
        public async Task<IActionResult> SelectClass()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)              return RedirectToAction("Login", "Account");
            if (user.Level < 10)           return RedirectToAction("Index");
            if (!string.IsNullOrEmpty(user.Class)) return RedirectToAction("Index");

            return View();
        }

        // POST /Character/ChooseClass
        [HttpPost("ChooseClass")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseClass([FromForm] string selectedClass)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var (success, error) = await _characterService.ChooseClassAsync(userId.Value, selectedClass);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("SelectClass");
            }

            var className = selectedClass[0..1].ToUpper() + selectedClass[1..];
            TempData["ToastLevel"] = $"You are now a {className}! Class bonuses are now active.";
            return RedirectToAction("Index");
        }

        // ===== SPELLS =====

        // GET /Character/Spells
        [HttpGet("Spells")]
        public async Task<IActionResult> Spells()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .Include(u => u.OwnedGear!)
                    .ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null) return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(user.Class))
                return RedirectToAction("Index");

            var es = await _characterService.GetEffectiveStatsAsync(user);

            var tasks = await _context.HabitTasks
                .Where(t => t.UserId == userId.Value
                         && t.IsActive
                         && t.Type != TaskType.Reward)
                .OrderBy(t => t.Type)
                .ThenBy(t => t.SortOrder)
                .ToListAsync();

            var vm = new SpellsViewModel
            {
                User           = user,
                EffectiveStats = es,
                Spells         = _spellService.GetSpellsForClass(user.Class),
                Tasks          = tasks,
            };

            return View(vm);
        }

        // POST /Character/CastSpell  (AJAX)
        [HttpPost("CastSpell")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CastSpell(
            [FromForm] string spellKey,
            [FromForm] int?   taskId = null)
        {
            var userId = GetUserId();
            if (userId == null)
                return Json(new { success = false, error = "Not logged in" });

            var (success, error, data) = await _spellService.CastAsync(userId.Value, spellKey, taskId);

            if (!success)
                return Json(new { success = false, error });

            // Load fresh user (with gear) for response values
            var user     = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            var effStats = await _characterService.GetEffectiveStatsAsync(user!);
            int maxMana  = effStats.MaxMana;

            return Json(new
            {
                success  = true,
                spellKey,
                newMana  = (int)user.Mana,
                maxMana,
                newHP    = Math.Round(user.HP, 1),
                newGold  = Math.Round(user.Gold, 2),
                newXP    = user.XP,
                data,
            });
        }

        // ===== AVATAR CUSTOMIZATION =====

        // GET /Character/Customize
        [HttpGet("Customize")]
        public async Task<IActionResult> Customize()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            bool dirty = false;
            if (string.IsNullOrEmpty(user.BodyType))   { user.BodyType   = "broad";   dirty = true; }
            if (string.IsNullOrEmpty(user.SkinColor))  { user.SkinColor  = "915533";  dirty = true; }
            if (string.IsNullOrEmpty(user.HairColor))  { user.HairColor  = "black";   dirty = true; }
            if (string.IsNullOrEmpty(user.ShirtStyle)) { user.ShirtStyle = "black";   dirty = true; }
            if (user.HairStyle < 1)                    { user.HairStyle  = 1;         dirty = true; }
            if (dirty) await _context.SaveChangesAsync();

            var root = _env.WebRootPath;

            static string After(string s, string prefix) => s[prefix.Length..];

            var skinDir = Path.Combine(_env.WebRootPath, "fe", "customize", "skin");
            var skinColors = Directory.Exists(skinDir)
                ? Directory.EnumerateFiles(skinDir, "skin_*.png")
                    .Select(f => Path.GetFileNameWithoutExtension(f).Replace("skin_", ""))
                    .ToList()
                : new List<string>();

            var hairDir = Path.Combine(_env.WebRootPath, "fe", "customize", "hair");
            var hairColors = Directory.EnumerateFiles(hairDir, "hair_base_1_*.png")
                .Select(f => After(Path.GetFileNameWithoutExtension(f), "hair_base_1_"))
                .OrderBy(c => c)
                .ToList();

            var hairStyleCount = Directory.EnumerateFiles(hairDir,   "hair_base_*_black.png").Count();
            var hairBangsCount = Directory.EnumerateFiles(hairDir,   "hair_bangs_*_black.png").Count();

            var beardDir = Path.Combine(_env.WebRootPath, "fe", "customize", "beards");
            var hairBeardCount   = Directory.EnumerateFiles(beardDir, "hair_beard_*_black.png").Count();
            var hairMustacheCount = Directory.EnumerateFiles(beardDir, "hair_mustache_*_black.png").Count();

            var shirtDir = Path.Combine(_env.WebRootPath, "fe", "customize", "shirts");
            var shirtStyles = Directory.EnumerateFiles(shirtDir, "broad_shirt_*.png")
                .Select(f => After(Path.GetFileNameWithoutExtension(f), "broad_shirt_"))
                .OrderBy(s => s)
                .ToList();

            var vm = new CustomizeViewModel
            {
                User              = user,
                SkinColors        = skinColors,
                HairColors        = hairColors,
                HairStyleCount    = hairStyleCount,
                HairBangsCount    = hairBangsCount,
                HairBeardCount    = hairBeardCount,
                HairMustacheCount = hairMustacheCount,
                ShirtStyles       = shirtStyles,
            };

            return View(vm);
        }

        // POST /Character/SaveAppearance
        [HttpPost("SaveAppearance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAppearance(
            [FromForm] string bodyType,
            [FromForm] string skinColor,
            [FromForm] int    hairStyle,
            [FromForm] string hairColor,
            [FromForm] int    hairBangs,
            [FromForm] int    hairBeard,
            [FromForm] int    hairMustache,
            [FromForm] string shirtStyle)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            user.BodyType     = bodyType is "broad" or "slim" ? bodyType : "broad";
            user.SkinColor    = skinColor;
            user.HairStyle    = Math.Clamp(hairStyle, 1, 20);
            user.HairColor    = hairColor;
            user.HairBangs    = Math.Clamp(hairBangs, 0, 4);
            user.HairBeard    = Math.Clamp(hairBeard, 0, 3);
            user.HairMustache = Math.Clamp(hairMustache, 0, 2);
            user.ShirtStyle   = shirtStyle;
            user.UpdatedAt    = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            TempData["ToastLevel"] = "Appearance saved!";
            return RedirectToAction("Customize");
        }

        // ===== REBIRTH =====

        // GET /Character/Rebirth
        [HttpGet("Rebirth")]
        public async Task<IActionResult> Rebirth()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");
            if (user.Level < 10) return RedirectToAction("Index");

            return View(user);
        }

        // POST /Character/Rebirth
        [HttpPost("Rebirth")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rebirth([FromForm] string confirm)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null || user.Level < 10) return RedirectToAction("Index");

            await _characterService.RebirthAsync(userId.Value);

            TempData["ToastRebirth"] = $"You have been reborn! Rebirth #{user.RebirthCount + 1}";
            return RedirectToAction("Index");
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}
