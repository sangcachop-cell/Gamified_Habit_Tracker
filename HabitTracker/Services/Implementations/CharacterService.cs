using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class CharacterService : ICharacterService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CharacterService> _logger;

        public CharacterService(AppDbContext context, ILogger<CharacterService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        // ===== EFFECTIVE STATS =====

        public Task<EffectiveStats> GetEffectiveStatsAsync(User user)
        {
            if (user.OwnedGear == null)
                _logger.LogWarning(
                    "GetEffectiveStatsAsync called without OwnedGear loaded for user {Id}. " +
                    "Gear bonuses will be 0. Load with .Include(u => u.OwnedGear).ThenInclude(ug => ug.GearItem).",
                    user.Id);

            // 1. Collect equipped GearItems
            var equippedKeys = new HashSet<string?>(StringComparer.Ordinal)
            {
                user.EquippedWeapon,
                user.EquippedArmor,
                user.EquippedHead,
                user.EquippedShield,
                user.EquippedBack,
                user.EquippedHeadAccessory,
                user.EquippedEyewear,
                user.EquippedBody,
            };
            equippedKeys.Remove(null); // discard empty slots

            var equippedGear = user.OwnedGear?
                .Select(ug => ug.GearItem)
                .Where(g => g != null && equippedKeys.Contains(g.Key))
                .ToList() ?? new List<GearItem>();

            // 2. Gear bonus (raw sum)
            int gearSTR = equippedGear.Sum(g => g.STR);
            int gearCON = equippedGear.Sum(g => g.CON);
            int gearINT = equippedGear.Sum(g => g.INT);
            int gearPER = equippedGear.Sum(g => g.PER);

            // 3. Class bonus — +50% per gear piece that matches user's class
            int classBonusSTR = 0, classBonusCON = 0, classBonusINT = 0, classBonusPER = 0;
            if (!string.IsNullOrEmpty(user.Class))
            {
                foreach (var g in equippedGear.Where(g => g.GearClass == user.Class))
                {
                    classBonusSTR += (int)(g.STR * 0.5);
                    classBonusCON += (int)(g.CON * 0.5);
                    classBonusINT += (int)(g.INT * 0.5);
                    classBonusPER += (int)(g.PER * 0.5);
                }
            }

            // 4. Level bonus — floor(level / 2) added to ALL stats
            int levelBonus = user.Level / 2;

            // 5. Buffs — active only if BuffExpiry is in the future
            bool buffActive = user.BuffExpiry.HasValue && user.BuffExpiry.Value > DateTime.UtcNow;
            int buffSTR = buffActive ? user.BuffSTR : 0;
            int buffCON = buffActive ? user.BuffCON : 0;
            int buffINT = buffActive ? user.BuffINT : 0;
            int buffPER = buffActive ? user.BuffPER : 0;

            // 6. Totals
            int effSTR = user.STR + gearSTR + classBonusSTR + levelBonus + buffSTR;
            int effCON = user.CON + gearCON + classBonusCON + levelBonus + buffCON;
            int effINT = user.INT + gearINT + classBonusINT + levelBonus + buffINT;
            int effPER = user.PER + gearPER + classBonusPER + levelBonus + buffPER;

            int maxMana = effINT * 2 + (int)AppConstants.MANA_BASE;

            var stats = new EffectiveStats(
                STR: effSTR, CON: effCON, INT: effINT, PER: effPER,
                MaxHP:   (int)AppConstants.MAX_HP,
                MaxMana: maxMana,
                GearSTR: gearSTR, GearCON: gearCON, GearINT: gearINT, GearPER: gearPER,
                ClassBonusSTR: classBonusSTR, ClassBonusCON: classBonusCON,
                ClassBonusINT: classBonusINT, ClassBonusPER: classBonusPER,
                LevelBonus: levelBonus,
                BuffSTR: buffSTR, BuffCON: buffCON, BuffINT: buffINT, BuffPER: buffPER
            );

            return Task.FromResult(stats);
        }

        // ===== CLASS SELECTION =====

        public async Task<(bool Success, string? Error)> ChooseClassAsync(int userId, string className)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (false, "User not found");

            if (user.Level < 10)
                return (false, "Must be level 10 to choose a class");

            if (!string.IsNullOrEmpty(user.Class))
                return (false, "Class already chosen");

            if (!CharacterClass.All.Contains(className))
                return (false, $"Invalid class: {className}");

            user.Class = className;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User {Id} chose class {Class}", userId, className);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save class choice for user {Id}", userId);
                return (false, "Failed to save. Please try again.");
            }
        }

        // ===== REBIRTH =====

        public async Task RebirthAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return;

            var tasks = await _context.HabitTasks
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();

            // Level / XP / Stats / Class
            user.Level      = 1;
            user.XP         = 0;
            user.STR        = user.CON = user.INT = user.PER = user.StatPoints = 0;
            user.Class      = null;

            // Currency / Vitals
            user.Gold       = 0;
            user.HP         = AppConstants.MAX_HP;
            user.Mana       = AppConstants.MANA_BASE;

            // Buffs
            user.BuffSTR    = user.BuffCON = user.BuffINT = user.BuffPER = 0;
            user.BuffExpiry    = null;
            user.StealthBuff   = 0;
            user.FrozenStreaks  = false;

            // Streaks (keep LongestStreak as lifetime record)
            user.CurrentStreak = 0;

            // Unequip all gear slots (keep UserGearItem ownership rows)
            user.EquippedWeapon        = user.EquippedArmor  = user.EquippedHead = null;
            user.EquippedShield        = user.EquippedBack   = user.EquippedHeadAccessory = null;
            user.EquippedEyewear       = user.EquippedBody   = null;

            // Timestamps / drop state
            user.LastCronDate        = user.LastDropResetDate = null;
            user.LastCheckInDate     = user.LastCompletedDate = user.LastActiveDate = null;
            user.DailyDropCount      = 0;

            // Rebirth counter
            user.RebirthCount++;

            // Task reset: value → 0 for all non-Reward tasks; streaks/counters cleared
            foreach (var task in tasks)
            {
                if (task.Type == TaskType.Reward) continue;
                task.Value = 0;
                if (task.Type == TaskType.Daily)
                {
                    task.Streak      = 0;
                    task.IsCompleted = false;
                }
                else if (task.Type == TaskType.Habit)
                {
                    task.CounterUp   = 0;
                    task.CounterDown = 0;
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("User {Id} reborn (count: {Count})", userId, user.RebirthCount);
        }

        // ===== STAT ALLOCATION =====

        public async Task<(bool Success, string? Error)> AllocateStatAsync(int userId, string stat)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (false, "User not found");

            if (user.StatPoints <= 0)
                return (false, "No stat points remaining");

            switch (stat.ToUpperInvariant())
            {
                case "STR": user.STR++; break;
                case "CON": user.CON++; break;
                case "INT": user.INT++; break;
                case "PER": user.PER++; break;
                default:    return (false, $"Unknown stat: {stat}");
            }

            user.StatPoints--;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User {Id} allocated 1 point to {Stat}", userId, stat);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save stat allocation for user {Id}", userId);
                return (false, "Failed to save. Please try again.");
            }
        }
    }
}
