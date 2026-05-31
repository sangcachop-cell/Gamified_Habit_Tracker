using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class SpellService : ISpellService
    {
        private readonly AppDbContext         _context;
        private readonly ICharacterService    _characterService;
        private readonly IBossQuestService    _bossQuests;
        private readonly ILogger<SpellService> _logger;

        // ===== STATIC SPELL CATALOG =====

        private static readonly List<SpellDefinition> Catalog = new()
        {
            // WARRIOR
            new("smash",             "Brutal Smash",        "Strike a task with raw strength, improving its value and dealing boss damage scaled to STR.", "warrior", 10, 11, TargetsTask: true),
            new("defensiveStance",   "Defensive Stance",    "Steel yourself, buffing CON for one day to reduce incoming damage.",                           "warrior", 25, 12, TargetsTask: false),
            new("valorousPresence",  "Valorous Presence",   "Inspire your party, buffing their STR.",                                                       "warrior", 20, 13, TargetsTask: false),
            new("intimidate",        "Intimidating Gaze",   "Intimidate enemies, buffing party CON.",                                                       "warrior", 15, 14, TargetsTask: false),

            // MAGE
            new("fireball",          "Burst of Flames",     "Ignite a task with arcane fire, granting bonus XP scaled to INT and dealing boss damage.",     "mage",    10, 11, TargetsTask: true),
            new("mpheal",            "Ethereal Surge",      "Channel mana to your party (non-mages), restoring MP scaled to INT.",                          "mage",    30, 12, TargetsTask: false),
            new("earth",             "Earthquake",          "Shake the earth, granting the party an INT buff.",                                             "mage",    35, 13, TargetsTask: false),
            new("frost",             "Chilling Frost",      "Freeze time itself — protect your streaks from cron for one day.",                            "mage",    40, 14, TargetsTask: false),

            // ROGUE
            new("pickPocket",        "Pickpocket",          "Siphon gold from a task's difficulty, earning bonus coins.",                                   "rogue",   10, 11, TargetsTask: true),
            new("backStab",          "Backstab",            "Strike from the shadows for bonus XP and gold. High crit chance.",                             "rogue",   15, 12, TargetsTask: true),
            new("toolsOfTrade",      "Tools of the Trade",  "Share your skills with the party, buffing their PER.",                                         "rogue",   25, 13, TargetsTask: false),
            new("stealth",           "Stealth",             "Vanish into the shadows, absorbing a number of missed daily damage instances.",                 "rogue",   45, 14, TargetsTask: false),

            // HEALER
            new("heal",              "Healing Light",       "Channel healing energy to restore HP scaled to CON and INT.",                                  "healer",  15, 11, TargetsTask: false),
            new("brightness",        "Searing Brightness",  "Bathe all your tasks in golden light, gently improving every task's value.",                   "healer",  15, 12, TargetsTask: false, TargetsAllTasks: true),
            new("protectAura",       "Protective Aura",     "Shield your party with a massive CON aura.",                                                   "healer",  30, 13, TargetsTask: false),
            new("healAll",           "Blessing",            "Bless your entire party with healing light.",                                                  "healer",  25, 14, TargetsTask: false),
        };

        private static readonly HashSet<string> PartySpellKeys = new()
        {
            "valorousPresence", "intimidate", "mpheal", "earth", "toolsOfTrade", "protectAura", "healAll"
        };

        public SpellService(
            AppDbContext context,
            ICharacterService characterService,
            IBossQuestService bossQuests,
            ILogger<SpellService> logger)
        {
            _context          = context;
            _characterService = characterService;
            _bossQuests       = bossQuests;
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
            // 1. Load user with gear
            var user = await _context.Users
                .Include(u => u.OwnedGear!)
                    .ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return (false, "User not found", null);

            // 2. Find spell
            var spell = Catalog.FirstOrDefault(s => s.Key == spellKey);
            if (spell == null) return (false, "Unknown spell", null);

            // 3. Validate: class
            if (user.Class != spell.ClassName)
                return (false, $"This spell belongs to the {spell.ClassName} class", null);

            // 4. Validate: level
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
            string? error;
            if (PartySpellKeys.Contains(spellKey))
            {
                error = await CastPartySpellAsync(user, es, spell, data);
            }
            else
            {
                error = spellKey switch
                {
                    "smash"           => CastBrutalSmash(user, es, task!, data),
                    "defensiveStance" => CastDefensiveStance(user, es, data),
                    "fireball"        => CastFireball(user, es, task!, data),
                    "frost"           => CastChillingFrost(user, data),
                    "pickPocket"      => CastPickpocket(user, es, task!, data),
                    "backStab"        => CastBackstab(user, es, task!, data),
                    "stealth"         => CastStealth(user, es, data),
                    "heal"            => CastHeal(user, es, data),
                    "brightness"      => CastBrightnessSync(user, es, data),
                    _                 => "Spell not yet implemented",
                };
            }

            if (error != null) return (false, error, null);

            // 10. Deduct mana
            int maxMana    = es.MaxMana;
            user.Mana      = Math.Clamp(user.Mana - spell.ManaCost, 0, maxMana);

            // 11. Apply spell boss damage if any (smash/fireball store it in _bossDmg)
            double bossDmg = 0;
            if (data.TryGetValue("_bossDmg", out var bossDmgObj))
            {
                bossDmg = (double)bossDmgObj;
                data.Remove("_bossDmg");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save cast for user {Id} spell {Spell}", userId, spellKey);
                return (false, "Failed to save. Please try again.", null);
            }

            // Apply boss damage after main save (boss system handles its own SaveChangesAsync)
            if (bossDmg > 0)
                await ApplySpellBossDamageAsync(user, spell.Name, bossDmg);

            _logger.LogInformation("User {Id} cast {Spell}", userId, spellKey);
            return (true, null, data);
        }

        // ===== HELPERS =====

        /// <summary>Habitica diminishing-returns: max * bonus / (bonus + halfway)</summary>
        private static double DR(double bonus, double max, double? halfway = null)
        {
            if (bonus <= 0) return 0;
            double h = halfway ?? max / 2.0;
            return max * (bonus / (bonus + h));
        }

        private static double ClampTaskValue(double v) =>
            Math.Clamp(v, AppConstants.TaskValueLimits.MIN, AppConstants.TaskValueLimits.MAX);

        /// <summary>
        /// Habitica crit: base chance = baseCritChance * (1 + stat/100).
        /// On hit: multiplier = 1.5 + 4*stat/(stat+200). Miss: 1.0.
        /// </summary>
        private static double RollCrit(int userId, int stat, double baseCritChance = 0.03)
        {
            double chance = baseCritChance * (1.0 + stat / 100.0);
            bool hit      = Random.Shared.NextDouble() < chance;
            return hit ? 1.5 + 4.0 * stat / (stat + 200.0) : 1.0;
        }

        /// <summary>
        /// Habitica calculateBonus: (taskValue &lt; 0 ? 1 : taskValue+1) + stat * statScale * critMult
        /// Note: crit only scales the stat term, not the task value term.
        /// </summary>
        private static double CalcBonus(double taskValue, int stat, double critMult = 1.0, double statScale = 0.5) =>
            (taskValue < 0 ? 1.0 : taskValue + 1.0) + stat * statScale * critMult;

        // ===== NON-PARTY SPELL IMPLEMENTATIONS =====

        private static string? CastBrutalSmash(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            double critMult  = RollCrit(user.Id, es.CON);  // smash uses CON for crit
            double bonus     = es.STR * critMult;

            double taskDelta = DR(bonus, 2.5, 35);
            double bossDmg   = DR(bonus, 55, 70);

            task.Value = ClampTaskValue(task.Value + taskDelta);

            data["taskValueDelta"] = Math.Round(taskDelta, 2);
            data["isCrit"]         = critMult > 1.0;
            data["_bossDmg"]       = bossDmg;
            data["toastMessage"]   = $"Brutal Smash! Task improved +{Math.Round(taskDelta, 2)}{(critMult > 1 ? " (CRIT!)" : "")}";
            return null;
        }

        private static string? CastDefensiveStance(User user, EffectiveStats es, Dictionary<string, object> data)
        {
            // Subtract existing buff to avoid stacking DR
            int unbuffed = Math.Max(0, es.CON - user.BuffCON);
            int buffAdd  = (int)Math.Ceiling(DR(unbuffed, 40, 200));
            if (buffAdd < 1) buffAdd = 1;

            user.BuffCON   += buffAdd;
            user.BuffExpiry = DateTime.UtcNow.AddDays(1);

            data["buffCONApplied"] = buffAdd;
            data["toastMessage"]   = $"Defensive Stance! +{buffAdd} CON buff for 1 day.";
            return null;
        }

        private static string? CastFireball(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            double critMult = RollCrit(user.Id, es.PER);  // fireball uses PER for crit
            double xpGained = DR(es.INT * critMult, 75);   // halfway defaults to 37.5
            double bossDmg  = es.INT * 0.1;

            user.XP += (int)Math.Round(xpGained);

            data["xpGained"]     = (int)Math.Round(xpGained);
            data["isCrit"]       = critMult > 1.0;
            data["_bossDmg"]     = bossDmg;
            data["toastMessage"] = $"Burst of Flames! +{(int)Math.Round(xpGained)} XP{(critMult > 1 ? " (CRIT!)" : "")}";
            return null;
        }

        private static string? CastChillingFrost(User user, Dictionary<string, object> data)
        {
            if (user.FrozenStreaks)
                return "Chilling Frost is already active — your streaks are protected today.";

            user.FrozenStreaks = true;

            data["frozenStreaks"] = true;
            data["toastMessage"] = "Chilling Frost! Your streaks are frozen for today's cron.";
            return null;
        }

        private static string? CastPickpocket(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            double bonus = CalcBonus(task.Value, es.PER);
            double gold  = DR(bonus, 25, 75);
            if (gold < 0.01) gold = 0.01;

            user.Gold += gold;

            data["goldGained"]   = Math.Round(gold, 2);
            data["toastMessage"] = $"Pickpocket! +{gold:F2} gold.";
            return null;
        }

        private static string? CastBackstab(User user, EffectiveStats es, HabitTask task, Dictionary<string, object> data)
        {
            double critMult = RollCrit(user.Id, es.STR, baseCritChance: 0.30);
            double bonus    = CalcBonus(task.Value, es.STR, critMult);
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
            int dailyCount = data.TryGetValue("_dailyCount", out var dc) ? (int)dc : 0;
            data.Remove("_dailyCount");

            double maxBuff = Math.Max(1, dailyCount * 0.64);
            int buffAdd    = (int)Math.Ceiling(DR(es.PER, maxBuff, 55));
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

            double hpGained = Math.Min(50.0, (es.CON + es.INT + 5) * 0.075);
            double newHP    = Math.Min(AppConstants.MAX_HP, user.HP + hpGained);
            double actual   = newHP - user.HP;
            user.HP         = newHP;

            data["hpGained"]     = Math.Round(actual, 1);
            data["toastMessage"] = $"Healing Light! +{Math.Round(actual, 1)} HP restored.";
            return null;
        }

        private string? CastBrightnessSync(User user, EffectiveStats es, Dictionary<string, object> data)
        {
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

            data["tasksBuffed"]  = count;
            data["valuePerTask"] = Math.Round(valuePerTask, 3);
            data["toastMessage"] = $"Searing Brightness! Boosted {count} tasks by +{valuePerTask:F3} each.";
            return null;
        }

        // ===== PARTY SPELL DISPATCHER =====

        private async Task<string?> CastPartySpellAsync(
            User caster, EffectiveStats es, SpellDefinition spell, Dictionary<string, object> data)
        {
            var partyMembership = await _context.PartyMembers
                .FirstOrDefaultAsync(pm => pm.UserId == caster.Id);

            List<User> targets;
            int? partyId = null;

            if (partyMembership != null)
            {
                partyId = partyMembership.PartyId;
                targets = await _context.PartyMembers
                    .Where(pm => pm.PartyId == partyMembership.PartyId)
                    .Include(pm => pm.User)
                    .Select(pm => pm.User!)
                    .ToListAsync();
            }
            else
            {
                targets = new List<User> { caster };
            }

            int affected = ApplyPartyEffect(spell.Key, caster, es, targets, data);

            data["partySize"] = affected;

            if (partyId.HasValue)
            {
                _context.PartyMessages.Add(new PartyMessage
                {
                    PartyId  = partyId.Value,
                    AuthorId = caster.Id,
                    Body     = $"[SYS]✨ {caster.Username} cast {spell.Name} on the party!",
                    SentAt   = DateTime.UtcNow
                });
            }

            return null;
        }

        private static int ApplyPartyEffect(
            string spellKey, User caster, EffectiveStats es, List<User> targets,
            Dictionary<string, object> data)
        {
            int affected = targets.Count;
            DateTime expiry = DateTime.UtcNow.AddDays(1);

            switch (spellKey)
            {
                case "valorousPresence":
                {
                    int buffAdd = (int)Math.Ceiling(DR(Math.Max(0, es.STR - caster.BuffSTR), 20, 200));
                    if (buffAdd < 1) buffAdd = 1;
                    foreach (var t in targets) { t.BuffSTR += buffAdd; t.BuffExpiry = expiry; }
                    data["buffAmount"]   = buffAdd;
                    data["toastMessage"] = $"Valorous Presence! +{buffAdd} STR to {affected} member{(affected != 1 ? "s" : "")}.";
                    break;
                }
                case "intimidate":
                {
                    int buffAdd = (int)Math.Ceiling(DR(Math.Max(0, es.CON - caster.BuffCON), 24, 200));
                    if (buffAdd < 1) buffAdd = 1;
                    foreach (var t in targets) { t.BuffCON += buffAdd; t.BuffExpiry = expiry; }
                    data["buffAmount"]   = buffAdd;
                    data["toastMessage"] = $"Intimidating Gaze! +{buffAdd} CON to {affected} member{(affected != 1 ? "s" : "")}.";
                    break;
                }
                case "mpheal":
                {
                    int mpRestore = (int)Math.Ceiling(DR(es.INT, 25, 125));
                    if (mpRestore < 1) mpRestore = 1;
                    int mpTargets = 0;
                    foreach (var t in targets)
                    {
                        if (string.Equals(t.Class, "mage", StringComparison.OrdinalIgnoreCase)) continue;
                        int maxMp = t.INT * 2 + 30;
                        t.Mana = Math.Min(maxMp, t.Mana + mpRestore);
                        mpTargets++;
                    }
                    affected = mpTargets;
                    data["mpRestored"]   = mpRestore;
                    data["toastMessage"] = $"Ethereal Surge! +{mpRestore} MP to {mpTargets} non-mage{(mpTargets != 1 ? "s" : "")}.";
                    break;
                }
                case "earth":
                {
                    int buffAdd = (int)Math.Ceiling(DR(Math.Max(0, es.INT - caster.BuffINT), 30, 200));
                    if (buffAdd < 1) buffAdd = 1;
                    foreach (var t in targets) { t.BuffINT += buffAdd; t.BuffExpiry = expiry; }
                    data["buffAmount"]   = buffAdd;
                    data["toastMessage"] = $"Earthquake! +{buffAdd} INT to {affected} member{(affected != 1 ? "s" : "")}.";
                    break;
                }
                case "toolsOfTrade":
                {
                    int buffAdd = (int)Math.Ceiling(DR(Math.Max(0, es.PER - caster.BuffPER), 100, 50));
                    if (buffAdd < 1) buffAdd = 1;
                    foreach (var t in targets) { t.BuffPER += buffAdd; t.BuffExpiry = expiry; }
                    data["buffAmount"]   = buffAdd;
                    data["toastMessage"] = $"Tools of the Trade! +{buffAdd} PER to {affected} member{(affected != 1 ? "s" : "")}.";
                    break;
                }
                case "protectAura":
                {
                    int buffAdd = (int)Math.Ceiling(DR(Math.Max(0, es.CON - caster.BuffCON), 200, 200));
                    if (buffAdd < 1) buffAdd = 1;
                    foreach (var t in targets) { t.BuffCON += buffAdd; t.BuffExpiry = expiry; }
                    data["buffAmount"]   = buffAdd;
                    data["toastMessage"] = $"Protective Aura! +{buffAdd} CON to {affected} member{(affected != 1 ? "s" : "")}.";
                    break;
                }
                case "healAll":
                {
                    double hpEach = Math.Min(50.0, (es.CON + es.INT + 5) * 0.04);
                    foreach (var t in targets) t.HP = Math.Min(AppConstants.MAX_HP, t.HP + hpEach);
                    data["hpRestored"]   = Math.Round(hpEach, 1);
                    data["toastMessage"] = $"Blessing! +{Math.Round(hpEach, 1)} HP to {affected} member{(affected != 1 ? "s" : "")}.";
                    break;
                }
            }

            return affected;
        }

        // ===== BOSS DAMAGE FROM SPELLS =====

        private async Task ApplySpellBossDamageAsync(User caster, string spellName, double rawDamage)
        {
            var pm = await _context.PartyMembers.FirstOrDefaultAsync(m => m.UserId == caster.Id);
            if (pm == null) return;

            var pq = await _context.PartyQuests
                .Include(q => q.BossQuest)
                .FirstOrDefaultAsync(q => q.PartyId == pm.PartyId && q.Status == "Active");

            if (pq == null || pq.BossQuest == null || !pq.BossQuest.IsBossQuest) return;

            double def       = pq.BossQuest.BossDef > 0 ? pq.BossQuest.BossDef : 1.0;
            double actualDmg = rawDamage / def;

            pq.BossHpRemaining -= actualDmg;

            _context.PartyMessages.Add(new PartyMessage
            {
                PartyId  = pm.PartyId,
                AuthorId = caster.Id,
                Body     = $"[SYS]✨ {caster.Username}'s {spellName} dealt {actualDmg:F1} damage to {pq.BossQuest.Text}! (HP: {Math.Max(0, pq.BossHpRemaining):F0}/{pq.BossQuest.BossHp:F0})",
                SentAt   = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            if (pq.BossHpRemaining <= 0)
                await _bossQuests.FinishQuestAsync(pq.Id);
        }
    }
}
