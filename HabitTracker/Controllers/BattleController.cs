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

        // MonsterDef no longer stores static stats — they're computed per-wave from player level
        private record MonsterDef(string Name, string Icon, string Description, int TotalWaves = 5);

        private static readonly Dictionary<string, MonsterDef> Monsters = new()
        {
            ["test_unit"] = new MonsterDef(
                Name:        "Test Unit Gauntlet",
                Icon:        "🤖",
                Description: "Five waves of Test Units that mirror your own power. Each wave hits harder. Stats scale with your level — no free rides.",
                TotalWaves:  5
            )
        };

        // ── Wave formula ─────────────────────────────────────────────────────
        // Mirrors user's base stat formulas (Level contribution only — no RPG stat bonus).
        // Wave multiplier: ×1.0 / ×1.3 / ×1.6 / ×1.9 / ×2.2
        private static (string Name, string Icon, int HP, int Attack, int Armor, int Speed, int XP)
            ComputeWave(int playerLevel, int wave)
        {
            int lvl = Math.Max(1, playerLevel);
            double mult = 1.0 + (wave - 1) * 0.3;

            int hp  = Math.Max(20,  (int)((80 + lvl * 5)        * mult));
            int atk = Math.Max(3,   (int)((5  + lvl)            * mult));
            int arm = Math.Max(0,   (int)((lvl * 0.5)           * mult));
            int spd = Math.Min(100, (int)(5 + Math.Sqrt(lvl) * 3) + (wave - 1) * 2);
            int xp  = Math.Max(10,  (int)((15 + lvl * 3)        * mult));

            var (name, icon) = wave switch
            {
                1 => ("Test Unit",        "🤖"),
                2 => ("Test Unit Mk.II",  "🤖"),
                3 => ("Test Unit Mk.III", "🤖"),
                4 => ("Test Unit Mk.IV",  "🤖"),
                _ => ("Test Unit PRIME",  "⚙️"),
            };

            return (name, icon, hp, atk, arm, spd, xp);
        }

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
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");
            ClearBattle();

            var user        = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            int playerLevel = user?.Level ?? 1;

            // Build lobby entries with wave-1 preview and total XP estimate
            ViewBag.LobbyEntries = Monsters.Select(kv =>
            {
                var w1      = ComputeWave(playerLevel, 1);
                var wLast   = ComputeWave(playerLevel, kv.Value.TotalWaves);
                int totalXP = Enumerable.Range(1, kv.Value.TotalWaves)
                                        .Sum(w => ComputeWave(playerLevel, w).XP);
                return new
                {
                    Id          = kv.Key,
                    kv.Value.Name,
                    kv.Value.Icon,
                    kv.Value.Description,
                    kv.Value.TotalWaves,
                    W1HP = w1.HP, W1ATK = w1.Attack, W1ARM = w1.Armor, W1SPD = w1.Speed,
                    WLastHP = wLast.HP, WLastATK = wLast.Attack, WLastARM = wLast.Armor, WLastSPD = wLast.Speed,
                    TotalXP     = totalXP,
                    PlayerLevel = playerLevel
                };
            }).ToList();

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

            var wave1 = ComputeWave(user.Level, 1);

            var state = new BattleState
            {
                PlayerName      = user.Username,
                PlayerCurrentHP = user.HP,
                PlayerMaxHP     = user.HP,
                PlayerAttack    = user.AttackDamage,
                PlayerArmor     = user.Armor,
                PlayerSpeed     = user.Speed,
                PlayerLevel     = user.Level,

                MonsterName        = wave1.Name,
                MonsterIcon        = wave1.Icon,
                MonsterDescription = m.Description,
                MonsterCurrentHP   = wave1.HP,
                MonsterMaxHP       = wave1.HP,
                MonsterAttack      = wave1.Attack,
                MonsterArmor       = wave1.Armor,
                MonsterSpeed       = wave1.Speed,

                Status      = "InProgress",
                Turn        = 1,
                WaveNumber  = 1,
                TotalWaves  = m.TotalWaves,
                XPReward    = wave1.XP,
                Log         = new List<string>
                {
                    $"⚔️ Gauntlet begins! {user.Username} (SPD {user.Speed}) vs {wave1.Name} (SPD {wave1.Speed}) — Wave 1/{m.TotalWaves}!"
                }
            };

            SaveBattle(state);
            _logger.LogInformation($"User {userId} started {m.Name} at Lv{user.Level}");
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

            if (action.ToLower() == "run")
                return RedirectToAction(nameof(Arena));

            var rng = new Random();
            var log = new List<string> { $"── Wave {state.WaveNumber}/{state.TotalWaves} · Turn {state.Turn} ──" };

            // ── Speed flags ──────────────────────────────────────────────────
            bool playerFaster = state.PlayerSpeed >= state.MonsterSpeed;
            bool playerBonus  = state.PlayerSpeed  >= (int)(state.MonsterSpeed * 1.5);
            bool monsterBonus = state.MonsterSpeed >= (int)(state.PlayerSpeed  * 1.5);

            if (playerFaster)
                log.Add($"⚡ You move first! (SPD {state.PlayerSpeed} vs {state.MonsterSpeed})");
            else
                log.Add($"⚡ {state.MonsterName} acts first! (SPD {state.MonsterSpeed} vs {state.PlayerSpeed})");
            if (playerBonus)  log.Add("⚡ You outspeed the enemy — bonus strike incoming!");
            if (monsterBonus) log.Add($"⚡ {state.MonsterName} is blindingly fast — bonus strike incoming!");

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
                    state.Status = "WaveCleared"; // intermediate — handled below
                }
            }

            void MonsterAttack(bool isFullBlock)
            {
                if (state.Status != "InProgress") return;
                if (isFullBlock) { log.Add($"💥 {state.MonsterName} attacks — blocked completely!"); return; }
                int dmg = Math.Max(0, state.MonsterAttack - state.PlayerArmor);
                if (dmg == 0) { log.Add($"💥 {state.MonsterName} attacks — absorbed by armor!"); return; }
                state.PlayerCurrentHP = Math.Max(0, state.PlayerCurrentHP - dmg);
                log.Add($"💥 {state.MonsterName} hits you for {dmg} damage!");
                if (state.PlayerCurrentHP == 0)
                {
                    state.Status = "Defeat";
                    log.Add($"💀 You fall on Wave {state.WaveNumber}...");
                }
            }

            void MonsterAI(bool isFullBlock)
            {
                if (state.Status != "InProgress") return;
                double hpRatio   = (double)state.MonsterCurrentHP / state.MonsterMaxHP;
                int    atkChance = hpRatio < 0.30 ? 85 : 70;
                if (rng.Next(100) < atkChance) MonsterAttack(isFullBlock);
                else log.Add($"🛡️ {state.MonsterName} braces defensively.");
            }

            (bool guardHeld, int failChance) ResolveDefend()
            {
                state.ConsecutiveDefends++;
                int chance = state.ConsecutiveDefends switch { 1 => 0, 2 => 35, 3 => 65, _ => 85 };
                bool failed = rng.Next(100) < chance;
                if (!failed)
                    log.Add($"🛡️ Guard raised — all damage blocked! (consecutive #{state.ConsecutiveDefends})");
                else
                    log.Add($"💔 Guard breaks from fatigue! (consecutive #{state.ConsecutiveDefends} · {chance}% fail)");
                return (!failed, chance);
            }

            // ── Turn resolution ──────────────────────────────────────────────

            if (playerFaster)
            {
                switch (action.ToLower())
                {
                    case "attack":
                        state.ConsecutiveDefends = 0;
                        PlayerAttack();
                        if (state.Status == "InProgress") MonsterAI(false);
                        if (state.Status == "InProgress" && playerBonus)
                        { log.Add("⚡ Bonus strike!"); PlayerAttack(); }
                        break;

                    case "defend":
                        var (held1, _) = ResolveDefend();
                        MonsterAI(held1);
                        break;
                }
            }
            else
            {
                switch (action.ToLower())
                {
                    case "attack":
                        state.ConsecutiveDefends = 0;
                        MonsterAI(false);
                        if (state.Status == "InProgress") PlayerAttack();
                        if (state.Status == "InProgress" && monsterBonus)
                        { log.Add($"⚡ {state.MonsterName} bonus strike!"); MonsterAttack(false); }
                        break;

                    case "defend":
                        var (held2, _) = ResolveDefend();
                        MonsterAI(held2);
                        if (state.Status == "InProgress" && monsterBonus)
                        { log.Add($"⚡ {state.MonsterName} follows up!"); MonsterAttack(held2); }
                        break;
                }
            }

            // ── Wave cleared: award XP then spawn next or declare victory ────
            if (state.Status == "WaveCleared")
            {
                var user = await _context.Users
                    .Include(u => u.UserBadges)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    // Award this wave's XP immediately
                    int waveXP   = state.XPReward;
                    int oldXP    = user.XP;
                    int oldLevel = user.Level;
                    user.XP            += waveXP;
                    user.TotalXPEarned += waveXP;
                    state.TotalXPReward += waveXP;

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
                    log.Add($"✨ Wave {state.WaveNumber} cleared! +{waveXP} XP");
                }

                if (state.WaveNumber >= state.TotalWaves)
                {
                    // All waves done — true victory
                    state.Status = "Victory";
                    log.Add($"🏆 GAUNTLET COMPLETE! {state.TotalXPReward} XP total earned!");
                }
                else
                {
                    // Spawn next wave
                    state.WaveNumber++;
                    var next = ComputeWave(state.PlayerLevel, state.WaveNumber);
                    state.MonsterName      = next.Name;
                    state.MonsterIcon      = next.Icon;
                    state.MonsterCurrentHP = next.HP;
                    state.MonsterMaxHP     = next.HP;
                    state.MonsterAttack    = next.Attack;
                    state.MonsterArmor     = next.Armor;
                    state.MonsterSpeed     = next.Speed;
                    state.XPReward         = next.XP;
                    state.Status           = "InProgress";
                    log.Add($"💨 Wave {state.WaveNumber}/{state.TotalWaves} — {next.Name} enters! (HP {next.HP} · ATK {next.Attack} · SPD {next.Speed})");
                }
            }

            // Prepend newest entries to log
            state.Log.InsertRange(0, Enumerable.Reverse(log));
            if (state.Log.Count > 60) state.Log = state.Log.Take(60).ToList();

            state.Turn++;
            SaveBattle(state);

            _logger.LogInformation(
                $"User {userId} action '{action}' W{state.WaveNumber} T{state.Turn - 1} → {state.Status}");
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
