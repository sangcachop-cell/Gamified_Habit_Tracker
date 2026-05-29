using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class SpellService : ISpellService
    {
        private readonly AppDbContext      _context;
        private readonly ICharacterService _characterService;
        private readonly ILogger<SpellService> _logger;

        // ===== STATIC SPELL CATALOG =====

        private static readonly List<SpellDefinition> Catalog = new()
        {
            // WARRIOR
            new("smash",             "Brutal Smash",       "Strike a task with raw strength, improving its value and granting XP scaled to STR.", "warrior", 10, 11, TargetsTask: true),
            new("defensiveStance",   "Defensive Stance",   "Steel yourself, buffing CON for one day to reduce incoming damage.",                   "warrior", 25, 12, TargetsTask: false),
            new("valorousPresence",  "Valorous Presence",  "Inspire your party, buffing their STR. (Requires party — Phase 7)",                   "warrior", 20, 13, TargetsTask: false),
            new("intimidate",        "Intimidating Gaze",  "Intimidate enemies, buffing party CON. (Requires party — Phase 7)",                   "warrior", 15, 14, TargetsTask: false),

            // MAGE
            new("fireball",          "Burst of Flames",    "Ignite a task with arcane fire, granting bonus XP scaled to INT.",                    "mage",    10, 11, TargetsTask: true),
            new("mpheal",            "Ethereal Surge",     "Channel mana to your party. (Requires party — Phase 7)",                              "mage",    30, 12, TargetsTask: false),
            new("earth",             "Stone Skin",         "Buff party INT with earth magic. (Requires party — Phase 7)",                         "mage",    35, 13, TargetsTask: false),
            new("frost",             "Chilling Frost",     "Freeze time itself — protect your streaks from cron for one day.",                    "mage",    40, 14, TargetsTask: false),

            // ROGUE
            new("pickPocket",        "Pickpocket",         "Siphon gold from a task's difficulty, earning bonus coins.",                          "rogue",   10, 11, TargetsTask: true),
            new("backStab",          "Backstab",           "Strike from the shadows for bonus XP and gold. High crit chance.",                    "rogue",   15, 12, TargetsTask: true),
            new("toolsOfTrade",      "Tools of Trade",     "Share your skills with the party, buffing their PER. (Requires party — Phase 7)",     "rogue",   25, 13, TargetsTask: false),
            new("stealth",           "Stealth",            "Vanish into the shadows, absorbing a number of missed daily damage instances.",        "rogue",   45, 14, TargetsTask: false),

            // HEALER
            new("heal",              "Healing Light",      "Channel healing energy to restore HP scaled to CON and INT.",                         "healer",  15, 11, TargetsTask: false),
            new("brightness",        "Searing Brightness", "Bathe all your tasks in golden light, gently improving every task's value.",          "healer",  15, 12, TargetsTask: false, TargetsAllTasks: true),
            new("protectAura",       "Protective Aura",    "Shield your party with a CON aura. (Requires party — Phase 7)",                       "healer",  30, 13, TargetsTask: false),
            new("healAll",           "Blessing",           "Bless your entire party with healing. (Requires party — Phase 7)",                    "healer",  25, 14, TargetsTask: false),
        };

        public SpellService(
            AppDbContext context,
            ICharacterService characterService,
            ILogger<SpellService> logger)
        {
            _context          = context;
            _characterService = characterService;
            _logger           = logger;
        }

        // ===== PUBLIC API =====

        public IReadOnlyList<SpellDefinition> GetSpellsForClass(string className) =>
            Catalog.Where(s => s.ClassName == className)
                   .OrderBy(s => s.MinLevel)
                   .ToList();

        public IReadOnlyList<SpellDefinition> GetAllSpells() => Catalog.AsReadOnly();

        public async Task<(bool Success, string? Error, Dictionary<string, object>? Data)>
            CastAsync(int userId, string spellKey, int? taskId = null)
        {
            // 1. Load user with gear (needed for GetEffectiveStatsAsync)
            var user = await _context.Users
                .Include(u => u.OwnedGear!)
                    .ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return (false, "User not found", null);

            // 2. Find spell
            var spell = Catalog.FirstOrDefault(s => s.Key == spellKey);
            if (spell == null) return (false, "Unknown spell", null);

            // 3. Validate: class ownership
            if (user.Class != spell.ClassName)
                return (false, $"This spell belongs to the {spell.ClassName} class", null);

            // 4. Validate: level gate
            if (user.Level < spell.MinLevel)
                return (false, $"Requires level {spell.MinLevel} (you are level {user.Level})", null);

            // 5. Validate: mana
            if (user.Mana < spell.ManaCost)
                return (false, $"Not enough mana (need {spell.ManaCost}, have {(int)user.Mana})", null);

            // 6. Load task if required
            HabitTask? task = null;
            if (spell.TargetsTask)
            {
                if (taskId == null)
                    return (false, "This spell requires a task target", null);

                task = await _context.HabitTasks
                    .FirstOrDefaultAsync(t => t.Id == taskId.Value
                                           && t.UserId == userId
                                           && t.IsActive
                                           && t.Type != TaskType.Reward);

                if (task == null)
                    return (false, "Task not found or cannot be targeted", null);
            }

            // 7. Compute effective stats
            var es = await _characterService.GetEffectiveStatsAsync(user);

            // 8. Pre-load async data needed by specific spells
            var data = new Dictionary<string, object>();

            if (spellKey == "stealth")
            {
                int dailyCount = await _context.HabitTasks
                    .CountAsync(t => t.UserId == userId && t.IsActive && t.Type == TaskType.Daily);
                data["_dailyCount"] = dailyCount;
            }
            else if (spellKey == "brightness")
            {
                var allTasks = await _context.HabitTasks
                    .Where(t => t.UserId == userId && t.IsActive && t.Type != TaskType.Reward)
                    .ToListAsync();
                data["_tasks"] = allTasks;
            }

            // 9. Dispatch
            string? error = spellKey switch
            {
                "smash"            => CastBrutalSmash(user, es, task!, data),
                "defensiveStance"  => CastDefensiveStance(user, es, data),
                "fireball"         => CastFireball(user, es, task!, data),
                "frost"            => CastChillingFrost(user, data),
                "pickPocket"       => CastPickpocket(user, es, task!, data),
                "backStab"         => CastBackstab(user, es, task!, data),
                "stealth"          => CastStealth(user, es, data),
                "heal"             => CastHeal(user, es, data),
                "brightness"       => CastBrightnessSync(user, es, data),
                // Party spells — Phase 7
                "valorousPresence" or "intimidate" or
                "mpheal"           or "earth"       or
                "toolsOfTrade"     or
                "protectAura"      or "healAll"     => "Party spells unlock in Phase 7 when the party system is ready.",
                _                                   => "Spell not yet implemented",
            };

            if (error != null) return (false, error, null);

            // 10. Deduct mana after successful cast
            int maxMana = es.MaxMana;
            user.Mana   = Math.Clamp(user.Mana - spell.ManaCost, 0, maxMana);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User {Id} cast {Spell}", userId, spellKey);
                return (true, null, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save cast for user {Id} spell {Spell}", userId, spellKey);
                return (false, "Failed to save. Please try again.", null);
            }
        }

        // ===== HELPERS =====

        /// <summary>Habitica diminishing-returns curve: max * bonus / (bonus + halfway)</summary>
        private static double DR(double bonus, double max, double? halfway = null)
        {
            if (bonus <= 0) return 0;
            double h = halfway ?? max / 2.0;
            return max * (bonus / (bonus + h));
        }

        /// <summary>Clamp task value to Habitica limits.</summary>
        private static double ClampTaskValue(double v) =>
            Math.Clamp(v, AppConstants.TaskValueLimits.MIN, AppConstants.TaskValueLimits.MAX);

        /// <summary>
        /// Roll a crit: base chance 3% (+ stat/100 scaling).
        /// critMult = 1.5 + 4*stat/(stat+200) if hit, else 1.0.
        /// Uses same DayRng pattern as EconomyService.
        /// </summary>
        private static double RollCrit(int userId, int stat, double baseCritChance = 0.03)
        {
            var rng         = DayRng(userId, stat);           // extra seed from stat so each spell rolls independently
            double chance   = baseCritChance * (1.0 + stat / 100.0);
            bool hit        = rng.NextDouble() < chance;
            return hit ? 1.5 + 4.0 * stat / (stat + 200.0) : 1.0;
        }

        private static Random DayRng(int userId, int extraSeed = 0) =>
            new Random(userId * 397 ^ DateTime.UtcNow.DayOfYear ^ extraSeed);

        private static double CalculateBonus(double taskValue, int stat, double critMult = 1.0, double statScale = 0.5) =>
            ((taskValue < 0 ? 1.0 : taskValue + 1.0) + stat * statScale) * critMult;

        // ===== SPELL IMPLEMENTATIONS =====

        private static string? CastBrutalSmash(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            double critMult   = RollCrit(user.Id, es.PER);
            double bonus      = es.STR * critMult;

            double taskDelta  = DR(bonus, 2.5, 35);
            double xpGained   = DR(bonus, 55, 70);

            task.Value  = ClampTaskValue(task.Value + taskDelta);
            user.XP    += (int)Math.Round(xpGained);

            data["xpGained"]       = (int)Math.Round(xpGained);
            data["taskValueDelta"] = Math.Round(taskDelta, 2);
            data["isCrit"]         = critMult > 1.0;
            data["toastMessage"]   = $"Brutal Smash! +{(int)Math.Round(xpGained)} XP{(critMult > 1 ? " (CRIT!)" : "")}";
            return null;
        }

        private static string? CastDefensiveStance(User user, EffectiveStats es, Dictionary<string, object> data)
        {
            int buffAdd = (int)Math.Ceiling(DR(es.CON, 40, 200));
            if (buffAdd < 1) buffAdd = 1;

            user.BuffCON   += buffAdd;
            user.BuffExpiry = DateTime.UtcNow.AddDays(1);

            data["buffCONApplied"] = buffAdd;
            data["toastMessage"]   = $"Defensive Stance! +{buffAdd} CON buff for 1 day.";
            return null;
        }

        private static string? CastFireball(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            double critMult  = RollCrit(user.Id, es.PER);
            double taskMult  = task.Value < 0 ? 1.0 : task.Value + 1.0;
            double bonus     = es.INT * critMult * taskMult * 0.075;
            double xpGained  = DR(bonus, 75);

            user.XP += (int)Math.Round(xpGained);

            data["xpGained"]     = (int)Math.Round(xpGained);
            data["isCrit"]       = critMult > 1.0;
            data["toastMessage"] = $"Burst of Flames! +{(int)Math.Round(xpGained)} XP{(critMult > 1 ? " (CRIT!)" : "")}";
            return null;
        }

        private static string? CastChillingFrost(User user, Dictionary<string, object> data)
        {
            if (user.FrozenStreaks)
                return "Chilling Frost is already active — your streaks are protected today.";

            user.FrozenStreaks = true;

            data["frozenStreaks"] = true;
            data["toastMessage"]  = "Chilling Frost! Your streaks are frozen for today's cron.";
            return null;
        }

        private static string? CastPickpocket(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            double baseVal  = task.Value < 0 ? 1.0 : task.Value + 1.0;
            double bonus    = baseVal + es.PER * 0.5;
            double gold     = DR(bonus, 25, 75);
            if (gold < 0.01) gold = 0.01;

            user.Gold += gold;

            data["goldGained"]   = Math.Round(gold, 2);
            data["toastMessage"] = $"Pickpocket! +{gold:F2} gold.";
            return null;
        }

        private static string? CastBackstab(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            // Backstab: 30% base crit chance using STR stat
            double critMult = RollCrit(user.Id, es.STR, baseCritChance: 0.30);
            double bonus    = CalculateBonus(task.Value, es.STR, critMult);
            double xp       = DR(bonus, 75, 50);
            double gold     = DR(bonus, 18, 75);

            user.XP   += (int)Math.Round(xp);
            user.Gold += gold;

            data["xpGained"]     = (int)Math.Round(xp);
            data["goldGained"]   = Math.Round(gold, 2);
            data["isCrit"]       = critMult > 1.0;
            data["toastMessage"] = $"Backstab! +{(int)Math.Round(xp)} XP, +{gold:F2} gold{(critMult > 1 ? " (CRIT!)" : "")}.";
            return null;
        }

        private static string? CastStealth(User user, EffectiveStats es, Dictionary<string, object> data)
        {
            // Count active dailies owned by user — can't await here so use a sync call
            // This is run in context of an async method in CastAsync; we resolve via a workaround.
            // The actual count is computed in CastAsync before this call via data["_dailyCount"].
            int dailyCount = data.TryGetValue("_dailyCount", out var dc) ? (int)dc : 0;
            data.Remove("_dailyCount");

            double maxBuff = dailyCount * 0.64;
            if (maxBuff < 1) maxBuff = 1;   // always at least 1 even with 0 dailies
            int buffAdd = (int)Math.Ceiling(DR(es.PER, maxBuff, 55));
            if (buffAdd < 1) buffAdd = 1;

            user.StealthBuff += buffAdd;

            data["stealthApplied"] = buffAdd;
            data["toastMessage"]   = $"Stealth! Will absorb {buffAdd} daily damage instance{(buffAdd != 1 ? "s" : "")}.";
            return null;
        }

        private static string? CastHeal(User user, EffectiveStats es, Dictionary<string, object> data)
        {
            if (user.HP >= AppConstants.MAX_HP)
                return "Your HP is already full.";

            double hpGained = (es.CON + es.INT + 5) * 0.075;
            double newHP    = Math.Min(AppConstants.MAX_HP, user.HP + hpGained);
            double actual   = newHP - user.HP;
            user.HP         = newHP;

            data["hpGained"]     = Math.Round(actual, 1);
            data["toastMessage"] = $"Healing Light! +{Math.Round(actual, 1)} HP restored.";
            return null;
        }

        private string? CastBrightnessSync(User user, EffectiveStats es, Dictionary<string, object> data)
        {
            // Load tasks synchronously is not ideal — we use a trick:
            // tasks are pre-loaded and stored in data["_tasks"] by CastAsync
            if (!data.TryGetValue("_tasks", out var rawTasks) || rawTasks is not List<HabitTask> tasks)
                return "Could not load tasks for Searing Brightness.";
            data.Remove("_tasks");

            double valuePerTask = 4.0 * (es.INT / (es.INT + 40.0));
            int count = 0;

            foreach (var t in tasks)
            {
                t.Value = ClampTaskValue(t.Value + valuePerTask);
                count++;
            }

            data["tasksBuffed"]    = count;
            data["valuePerTask"]   = Math.Round(valuePerTask, 3);
            data["toastMessage"]   = $"Searing Brightness! Boosted {count} tasks by +{valuePerTask:F3} each.";
            return null;
        }
    }
}
