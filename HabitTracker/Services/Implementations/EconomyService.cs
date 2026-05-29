using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class EconomyService : IEconomyService
    {
        private readonly AppDbContext      _context;
        private readonly ILogger<EconomyService> _logger;
        private readonly ICharacterService _characterService;

        public EconomyService(AppDbContext context, ILogger<EconomyService> logger, ICharacterService characterService)
        {
            _context          = context;
            _logger           = logger;
            _characterService = characterService;
        }

        // ── Private helpers ──────────────────────────────────────────────────

        private static double PriorityMult(TaskPriority p) =>
            AppConstants.PriorityMultipliers.Get(p);

        private static (bool crit, double mult) RollCrit(int userId)
        {
            bool hit = Random.Shared.NextDouble() < AppConstants.CRIT_CHANCE;
            return (hit, hit ? AppConstants.CRIT_MULT : 1.0);
        }

        private static int CalcXP(double absDelta, double pm, double critMult, int intStat)
        {
            double intBonus = 1.0 + intStat * 0.025;
            return (int)Math.Round(absDelta * intBonus * pm * critMult * 6.0);
        }

        private static double CalcGold(double absDelta, double pm, double critMult,
                                       int perStat, int streak, TaskType type)
        {
            double perBonus = 1.0 + perStat * 0.02;
            double gold     = absDelta * pm * critMult * perBonus;
            if (type == TaskType.Daily) gold *= (1.0 + streak / 100.0);
            return gold;
        }

        private static double CalcManaGain(TaskType type, int intStat, bool isUp, int checklistItems)
        {
            int maxMana = intStat * 2 + 30;
            double gain = type switch
            {
                TaskType.Habit => Math.Max(0.25, 0.0025 * maxMana),
                TaskType.Daily => Math.Max(1.0,  0.01   * maxMana),
                TaskType.Todo  => Math.Max(Math.Max(1, checklistItems),
                                           0.01 * maxMana * Math.Max(1, checklistItems)),
                _              => 0.0
            };
            return isUp ? gain : -gain;
        }

        private static double CalcDropChance(HabitTask task, int userLevel, int userPER, double pm, double critMult)
        {
            double chance     = Math.Min(Math.Abs(task.Value - 21.27), 37.5) / 150.0 + 0.02;
            double newUserBon = 1.0 + Math.Max(0, 80 - 5 * userLevel) / 100.0;
            chance           *= pm * newUserBon * critMult;
            if (task.Type == TaskType.Daily) chance *= (1.0 + task.Streak / 100.0);
            int completed = task.ChecklistItems?.Count(i => i.IsCompleted) ?? 0;
            chance *= (1.0 + 0.5 * completed);
            chance *= (1.0 + userPER / 100.0);
            // Diminishing returns — capped at 75%
            chance = 0.75 * chance / (chance + 0.375);
            return chance;
        }

        private async Task<GameItem?> RollDropAsync(double chance, Random rng)
        {
            if (rng.NextDouble() > chance) return null;

            double r2       = rng.NextDouble();
            ItemType type;
            ItemRarity rarity = ItemRarity.Common;

            if      (r2 > 0.60) { type = ItemType.Food; }
            else if (r2 > 0.30) { type = ItemType.Egg;  }
            else
            {
                type      = ItemType.HatchingPotion;
                double r3 = rng.NextDouble();
                if      (r3 < 0.02) rarity = ItemRarity.VeryRare;
                else if (r3 < 0.09) rarity = ItemRarity.Rare;
                else if (r3 < 0.18) rarity = ItemRarity.Uncommon;
                else                rarity = ItemRarity.Common;
            }

            var candidates = await _context.GameItems
                .Where(g => g.Type == type &&
                            g.IsDroppable &&
                            (type != ItemType.HatchingPotion || g.Rarity == rarity))
                .ToListAsync();

            return candidates.Count == 0 ? null : candidates[rng.Next(candidates.Count)];
        }

        private async Task AwardItemAsync(int userId, GameItem item)
        {
            var existing = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.UserId == userId && i.GameItemId == item.Id);

            if (existing != null)
                existing.Quantity++;
            else
                _context.UserInventoryItems.Add(new UserInventoryItem
                {
                    UserId     = userId,
                    GameItemId = item.Id,
                    ObtainedAt = DateTime.UtcNow
                });
        }

        // ── Public API ───────────────────────────────────────────────────────

        public async Task<EconomyResult> ApplyTaskScoreEconomyAsync(
            User user, HabitTask task, double delta, bool isUp)
        {
            var result   = new EconomyResult();
            double pm    = PriorityMult(task.Priority);
            var (crit, critM) = RollCrit(user.Id);
            result.IsCrit    = crit;
            double absDelta  = Math.Abs(delta);
            var effStats     = await _characterService.GetEffectiveStatsAsync(user);
            int maxMana      = effStats.MaxMana;

            if (isUp)
            {
                // XP
                result.XpGained = CalcXP(absDelta, pm, critM, user.INT);
                user.XP            += result.XpGained;
                user.TotalXPEarned += result.XpGained;

                // Gold
                result.GoldGained = CalcGold(absDelta, pm, critM, user.PER, task.Streak, task.Type);
                user.Gold        += result.GoldGained;

                // Mana
                int checklist     = task.ChecklistItems?.Count ?? 0;
                double manaGain   = CalcManaGain(task.Type, effStats.INT, true, checklist) * critM;
                user.Mana         = Math.Clamp(user.Mana + manaGain, 0, maxMana);
                result.ManaGained = manaGain;

                // Reset daily drop count at start of new day (must run before cap check)
                if (user.LastDropResetDate?.Date != DateTime.UtcNow.Date)
                {
                    user.DailyDropCount    = 0;
                    user.LastDropResetDate = DateTime.UtcNow;
                }

                // Item drop (only if under daily cap)
                if (user.DailyDropCount < AppConstants.DAILY_DROP_CAP)
                {
                    double dropChance = CalcDropChance(task, user.Level, effStats.PER, pm, critM);
                    var dropped       = await RollDropAsync(dropChance, Random.Shared);

                    if (dropped != null)
                    {
                        await AwardItemAsync(user.Id, dropped);
                        user.DailyDropCount++;
                        result.DroppedItemIcon = !string.IsNullOrEmpty(dropped.ImagePath)
                            ? $"<img src='{dropped.ImagePath}' style='width:36px;height:36px;object-fit:contain;' onerror=\"this.replaceWith(document.createTextNode('{dropped.Icon}'))\">"
                            : dropped.Icon;
                        result.DroppedItemName = dropped.Name;
                    }
                }
            }
            else
            {
                // Negative Habit: HP damage + mana drain
                if (!user.IsSleeping)
                {
                    double conBonus = Math.Max(0.1, 1.0 - user.CON / 250.0);
                    double damage   = absDelta * conBonus * pm * 2.0;
                    user.HP         = Math.Max(0, user.HP - damage);
                    if (user.HP <= 0) result.Died = true;
                }

                double manaDrain = CalcManaGain(task.Type, effStats.INT, false, 0);
                user.Mana        = Math.Clamp(user.Mana + manaDrain, 0, maxMana);
                result.ManaGained = manaDrain;
            }

            return result;
        }

        public Task ApplyCronDamageAsync(User user, HabitTask missedDaily, double delta)
        {
            if (user.IsSleeping) return Task.CompletedTask;

            double pm       = PriorityMult(missedDaily.Priority);
            double conBonus = Math.Max(0.1, 1.0 - user.CON / 250.0);

            // Checklist partial reduction: only unfinished fraction causes full damage
            int total     = missedDaily.ChecklistItems?.Count ?? 0;
            int done      = missedDaily.ChecklistItems?.Count(i => i.IsCompleted) ?? 0;
            double frac   = total > 0 ? (double)(total - done) / total : 1.0;

            double damage = Math.Abs(delta) * conBonus * pm * 2.0 * frac;
            user.HP       = Math.Max(0, user.HP - damage);

            return Task.CompletedTask;
        }

        public async Task ApplyCronManaRegenAsync(User user)
        {
            var effStats = await _characterService.GetEffectiveStatsAsync(user);
            int maxMana  = effStats.MaxMana;
            user.Mana    = Math.Min(maxMana, user.Mana + maxMana * 0.02);
        }

        public async Task<(bool Success, string? Error)> BuyHealthPotionAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)              return (false, "User not found");
            if (user.HP >= AppConstants.MAX_HP)
                                           return (false, "HP already full");
            if (user.HP <= 0)              return (false, "Cannot use while dead — revive first");
            if (user.Gold < AppConstants.HEALTH_POTION_COST)
                                           return (false, $"Need {AppConstants.HEALTH_POTION_COST} GP (you have {user.Gold:F1})");

            user.Gold -= AppConstants.HEALTH_POTION_COST;
            user.HP    = Math.Min(AppConstants.MAX_HP, user.HP + AppConstants.HEALTH_POTION_HEAL);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<bool> ToggleSleepAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            user.IsSleeping = !user.IsSleeping;
            await _context.SaveChangesAsync();
            return user.IsSleeping;
        }

        public async Task ReviveAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return;

            user.HP    = AppConstants.MAX_HP;
            user.XP    = 0;
            user.Gold  = 0;
            user.Level = Math.Max(1, user.Level - 1);

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserInventoryItem>> GetInventoryAsync(int userId)
        {
            return await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.Quantity > 0)
                .Include(i => i.GameItem)
                .OrderBy(i => i.GameItem!.Type)
                .ThenBy(i => i.GameItem!.Name)
                .ToListAsync();
        }

        // ===== Phase 4 =====

        public async Task<(bool Success, string? Error)> BuyGemAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return (false, "User not found");
            if (user.Gold < AppConstants.GEM_GOLD_COST)
                return (false, $"Need {AppConstants.GEM_GOLD_COST} GP (you have {user.Gold:F1})");

            user.Gold -= AppConstants.GEM_GOLD_COST;
            user.Gems++;
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<ArmoireResult> PullArmoireAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new ArmoireResult { Success = false, Error = "User not found" };
            if (user.Gold < AppConstants.ARMOIRE_COST)
                return new ArmoireResult { Success = false, Error = $"Need {AppConstants.ARMOIRE_COST} GP (you have {user.Gold:F1})" };

            user.Gold -= AppConstants.ARMOIRE_COST;

            double roll = Random.Shared.NextDouble();
            ArmoireResult result;

            // 60% → gear (unowned armoire item)
            if (roll < AppConstants.ARMOIRE_GEAR_THRESHOLD)
            {
                var ownedKeys = await _context.UserGearItems
                    .Where(g => g.UserId == userId)
                    .Select(g => g.GearItem!.Key)
                    .ToListAsync();

                var candidates = await _context.GearItems
                    .Where(g => g.IsArmoire && !ownedKeys.Contains(g.Key))
                    .ToListAsync();

                if (candidates.Count > 0)
                {
                    var gear = candidates[Random.Shared.Next(candidates.Count)];
                    _context.UserGearItems.Add(new UserGearItem
                    {
                        UserId     = userId,
                        GearItemId = gear.Id,
                        AcquiredAt = DateTime.UtcNow
                    });
                    result = new ArmoireResult
                    {
                        Success     = true,
                        RewardType  = ArmoireRewardType.Gear,
                        GearKey     = gear.Key,
                        GearName    = gear.Name,
                        GearImgPath = gear.ShopImagePath
                    };
                    await _context.SaveChangesAsync();
                    return result;
                }
                // Fall through to food if all gear owned
            }

            // 20% → food (rolls up here if < 0.8 OR gear fallthrough)
            if (roll < AppConstants.ARMOIRE_FOOD_THRESHOLD || roll < AppConstants.ARMOIRE_GEAR_THRESHOLD)
            {
                var foods = await _context.GameItems
                    .Where(g => g.Type == ItemType.Food)
                    .ToListAsync();

                if (foods.Count > 0)
                {
                    var food = foods[Random.Shared.Next(foods.Count)];
                    await AwardItemAsync(userId, food);
                    result = new ArmoireResult
                    {
                        Success    = true,
                        RewardType = ArmoireRewardType.Food,
                        FoodIcon   = food.ImagePath,
                        FoodName   = food.Name
                    };
                    await _context.SaveChangesAsync();
                    return result;
                }
            }

            // 20% → XP (or fallback if no food either)
            int xp = Random.Shared.Next(AppConstants.ARMOIRE_XP_MIN, AppConstants.ARMOIRE_XP_MAX + 1);
            user.XP            += xp;
            user.TotalXPEarned += xp;
            await _context.SaveChangesAsync();
            return new ArmoireResult { Success = true, RewardType = ArmoireRewardType.Experience, XpGained = xp };
        }

        public async Task<(bool Success, string? Error, int GoldAwarded)> SellItemAsync(int userId, int gameItemId)
        {
            var row = await _context.UserInventoryItems
                .Include(i => i.GameItem)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.GameItemId == gameItemId);

            if (row == null)        return (false, "Item not in inventory", 0);
            if (row.GameItem == null) return (false, "Item data missing", 0);
            if (row.GameItem.GoldValue <= 0)
                                    return (false, "This item cannot be sold", 0);

            int gold = row.GameItem.GoldValue;

            if (row.Quantity <= 1)
                _context.UserInventoryItems.Remove(row);
            else
                row.Quantity--;

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return (false, "User not found", 0);
            user.Gold += gold;

            await _context.SaveChangesAsync();
            return (true, null, gold);
        }
    }
}
