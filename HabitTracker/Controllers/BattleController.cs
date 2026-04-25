using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class BattleController : Controller
    {
        private const string SESSION_KEY = "CurrentBattle";

        private record MonsterDef(
            string Name, string Icon, string Description,
            int HP, int Attack, int Armor, int Speed, int XPReward);

        // ── Monster catalogue ────────────────────────────────────────────────
        private static readonly Dictionary<string, MonsterDef> Monsters = new()
        {
            ["test_unit"] = new MonsterDef(
                Name:        "Test Unit",
                Icon:        "🤖",
                Description: "A training automaton animated by dark magic. Methodical — and merciless.",
                HP:       40,
                Attack:   10,
                Armor:     2,
                Speed:    10,   // baseline — a slow but sturdy training dummy
                XPReward: 30
            )
        };

        private readonly AppDbContext   _context;
        private readonly IQuestService  _questService;
        private readonly ILogger<BattleController> _logger;

        public BattleController(
            AppDbContext context,
            IQuestService questService,
            ILogger<BattleController> logger)
        {
            _context      = context;
            _questService = questService;
            _logger       = logger;
        }

        // ── LOBBY ────────────────────────────────────────────────────────────
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            if (GetUserId() == null) return RedirectToAction("Login", "Account");
            ClearBattle();
            ViewBag.Monsters = Monsters
                .Select(kv => new {
                    Id          = kv.Key,
                    kv.Value.Name,
                    kv.Value.Icon,
                    kv.Value.Description,
                    kv.Value.HP,
                    kv.Value.Attack,
                    kv.Value.Armor,
                    kv.Value.Speed,
                    kv.Value.XPReward
                })
                .ToList();
            return View();
        }

        // ── START ────────────────────────────────────────────────────────────
        [HttpPost("Start")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(string monsterId = "test_unit")
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !Monsters.TryGetValue(monsterId, out var m))
                return RedirectToAction(nameof(Index));

            var state = new BattleState
            {
                PlayerName      = user.Username,
                PlayerCurrentHP = user.HP,
                PlayerMaxHP     = user.HP,
                PlayerAttack    = user.AttackDamage,
                PlayerArmor     = user.Armor,
                PlayerSpeed     = user.Speed,

                MonsterName        = m.Name,
                MonsterIcon        = m.Icon,
                MonsterDescription = m.Description,
                MonsterCurrentHP   = m.HP,
                MonsterMaxHP       = m.HP,
                MonsterAttack      = m.Attack,
                MonsterArmor       = m.Armor,
                MonsterSpeed       = m.Speed,

                Status   = "InProgress",
                Turn     = 1,
                XPReward = m.XPReward,
                Log      = new List<string>
                {
                    $"⚔️ Battle begins!  {user.Username} (SPD {user.Speed})  vs  {m.Name} (SPD {m.Speed})!"
                }
            };

            SaveBattle(state);
            _logger.LogInformation($"User {userId} started battle vs {m.Name}");
            return RedirectToAction(nameof(Arena));
        }

        // ── ARENA ────────────────────────────────────────────────────────────
        [HttpGet("Arena")]
        public IActionResult Arena()
        {
            if (GetUserId() == null) return RedirectToAction("Login", "Account");
            var state = LoadBattle();
            if (state == null) return RedirectToAction(nameof(Index));
            return View(state);
        }

        // ── PROCESS TURN ─────────────────────────────────────────────────────
        [HttpPost("Action")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Action(string action)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var state = LoadBattle();
            if (state == null || state.Status != "InProgress")
                return RedirectToAction(nameof(Arena));

            var rng = new Random();
            var log = new List<string> { $"── Turn {state.Turn} ──" };

            // ── Speed flags ──────────────────────────────────────────────────
            bool playerFaster = state.PlayerSpeed >= state.MonsterSpeed;
            // Bonus action triggers at 1.5× speed advantage
            bool playerBonus  = state.PlayerSpeed  >= (int)(state.MonsterSpeed * 1.5);
            bool monsterBonus = state.MonsterSpeed >= (int)(state.PlayerSpeed  * 1.5);
            bool defending    = action.ToLower() == "defend";

            // Speed announcement
            if (playerFaster)
                log.Add($"⚡ You move first! (SPD {state.PlayerSpeed} vs {state.MonsterSpeed})");
            else
                log.Add($"⚡ {state.MonsterName} acts first! (SPD {state.MonsterSpeed} vs {state.PlayerSpeed})");
            if (playerBonus)
                log.Add("⚡ You outspeed the enemy — bonus strike incoming!");
            if (monsterBonus)
                log.Add($"⚡ {state.MonsterName} is blindingly fast — bonus strike incoming!");

            // ── Local helpers ────────────────────────────────────────────────

            void PlayerAttack()
            {
                if (state.MonsterCurrentHP <= 0 || state.Status != "InProgress") return;
                int dmg = Math.Max(1, state.PlayerAttack - state.MonsterArmor);
                state.MonsterCurrentHP = Math.Max(0, state.MonsterCurrentHP - dmg);
                log.Add($"⚔️ You strike {state.MonsterName} for {dmg} damage!");
                if (state.MonsterCurrentHP == 0)
                {
                    log.Add($"💀 {state.MonsterName} is destroyed!");
                    state.Status = "Victory";
                }
            }

            void MonsterAttack(bool isDefending)
            {
                if (state.Status != "InProgress") return;
                int eff = isDefending ? state.PlayerArmor * 2 : state.PlayerArmor;
                int dmg = Math.Max(0, state.MonsterAttack - eff);
                if (dmg == 0)
                {
                    log.Add($"💥 {state.MonsterName} attacks — completely absorbed by your guard!");
                    return;
                }
                state.PlayerCurrentHP = Math.Max(0, state.PlayerCurrentHP - dmg);
                string prefix = isDefending
                    ? $"💥 {state.MonsterName} forces through your guard for"
                    : $"💥 {state.MonsterName} hits you for";
                log.Add($"{prefix} {dmg} damage!");
                if (state.PlayerCurrentHP == 0)
                {
                    state.Status = "Defeat";
                    log.Add("💀 You have been defeated...");
                }
            }

            void MonsterAI(bool isDefending)
            {
                if (state.Status != "InProgress") return;
                double hpRatio   = (double)state.MonsterCurrentHP / state.MonsterMaxHP;
                int    atkChance = hpRatio < 0.30 ? 85 : 70;
                if (rng.Next(100) < atkChance)
                    MonsterAttack(isDefending);
                else
                    log.Add($"🛡️ {state.MonsterName} braces defensively.");
            }

            void PlayerRun(bool monsterAlreadyActed)
            {
                if (rng.Next(100) < 60)
                {
                    log.Add("🏃 You successfully retreat from battle!");
                    state.Status = "Fled";
                }
                else if (!monsterAlreadyActed)
                {
                    // Monster didn't act yet → gets a free punish hit
                    log.Add($"🏃 Escape blocked — {state.MonsterName} punishes your retreat!");
                    MonsterAttack(false);
                }
                else
                {
                    // Monster already had its turn, failed flee = just stays in combat
                    log.Add($"🏃 Escape blocked — you're forced to keep fighting!");
                }
            }

            // ── Turn resolution ──────────────────────────────────────────────

            if (playerFaster)
            {
                //  1. Player acts
                //  2. Monster AI responds
                //  3. [optional] Player bonus attack

                switch (action.ToLower())
                {
                    case "attack":
                        PlayerAttack();
                        if (state.Status == "InProgress") MonsterAI(false);
                        if (state.Status == "InProgress" && playerBonus)
                        {
                            log.Add("⚡ Bonus strike!");
                            PlayerAttack();
                        }
                        break;

                    case "defend":
                        log.Add("🛡️ You take a defensive stance!");
                        MonsterAI(true);
                        // No bonus turn for defending
                        break;

                    case "run":
                        PlayerRun(monsterAlreadyActed: false);
                        break;
                }
            }
            else
            {
                //  1. Monster AI acts first (player stance already chosen)
                //  2. Player acts (if alive)
                //  3. [optional] Monster bonus attack

                switch (action.ToLower())
                {
                    case "attack":
                        MonsterAI(false);
                        if (state.Status == "InProgress") PlayerAttack();
                        if (state.Status == "InProgress" && monsterBonus)
                        {
                            log.Add($"⚡ {state.MonsterName} bonus strike!");
                            MonsterAttack(false);
                        }
                        break;

                    case "defend":
                        // Defensive stance is pre-emptive — applies even to monster's first strike
                        MonsterAI(true);
                        log.Add("🛡️ You brace through the assault!");
                        if (state.Status == "InProgress" && monsterBonus)
                        {
                            log.Add($"⚡ {state.MonsterName} follows up — guard holds!");
                            MonsterAttack(true); // still defending
                        }
                        break;

                    case "run":
                        MonsterAI(false);
                        if (state.Status == "InProgress") PlayerRun(monsterAlreadyActed: true);
                        // No monster bonus turn when player is fleeing
                        break;
                }
            }

            // ── Victory: award XP ────────────────────────────────────────────
            if (state.Status == "Victory")
            {
                var user = await _context.Users
                    .Include(u => u.UserBadges)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    int oldXP    = user.XP;
                    int oldLevel = user.Level;
                    user.XP            += state.XPReward;
                    user.TotalXPEarned += state.XPReward;

                    int newLevel = _questService.CalculateLevel(user.XP);
                    if (newLevel > oldLevel)
                    {
                        user.Level = newLevel;
                        _questService.GrantLevelUpStats(user, newLevel - oldLevel);
                        log.Add($"⬆️ Level Up! You reached Level {newLevel}!");
                    }

                    user.LastActiveDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    await _questService.AwardBadgesAsync(user, oldXP);
                    log.Add($"🏆 Victory! +{state.XPReward} XP awarded!");
                }
            }

            // Newest entries go to top of log
            state.Log.InsertRange(0, Enumerable.Reverse(log));
            if (state.Log.Count > 50) state.Log = state.Log.Take(50).ToList();

            state.Turn++;
            SaveBattle(state);

            _logger.LogInformation(
                $"User {userId} action '{action}' T{state.Turn - 1} → {state.Status}");
            return RedirectToAction(nameof(Arena));
        }

        // ── RESET ────────────────────────────────────────────────────────────
        [HttpPost("Reset")]
        [ValidateAntiForgeryToken]
        public IActionResult Reset()
        {
            ClearBattle();
            return RedirectToAction(nameof(Index));
        }

        // ── HELPERS ──────────────────────────────────────────────────────────
        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);

        private BattleState? LoadBattle()
        {
            var json = HttpContext.Session.GetString(SESSION_KEY);
            return string.IsNullOrEmpty(json)
                ? null
                : JsonSerializer.Deserialize<BattleState>(json);
        }

        private void SaveBattle(BattleState state) =>
            HttpContext.Session.SetString(SESSION_KEY, JsonSerializer.Serialize(state));

        private void ClearBattle() =>
            HttpContext.Session.Remove(SESSION_KEY);
    }
}
