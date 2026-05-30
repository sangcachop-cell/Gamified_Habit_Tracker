using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IQuestService _questService;
        private readonly IEconomyService _economyService;
        private readonly IBossQuestService _bossQuestService;
        private readonly ILogger<TaskService> _logger;

        public TaskService(AppDbContext context, IQuestService questService,
                           IEconomyService economyService, IBossQuestService bossQuestService,
                           ILogger<TaskService> logger)
        {
            _context          = context;
            _questService     = questService;
            _economyService   = economyService;
            _bossQuestService = bossQuestService;
            _logger           = logger;
        }

        // ===== READ =====

        public async Task<TaskBoardViewModel> GetUserTasksAsync(int userId)
        {
            var tasks = await _context.HabitTasks
                .Where(t => t.UserId == userId && t.IsActive)
                .Include(t => t.ChecklistItems!.OrderBy(c => c.SortOrder))
                .Include(t => t.TagAssignments!)
                    .ThenInclude(a => a.UserTaskTag)
                .OrderBy(t => t.SortOrder)
                .ToListAsync();

            return new TaskBoardViewModel
            {
                Habits  = tasks.Where(t => t.Type == TaskType.Habit).ToList(),
                Dailies = tasks.Where(t => t.Type == TaskType.Daily).ToList(),
                Todos   = tasks.Where(t => t.Type == TaskType.Todo && t.DateCompleted == null).ToList(),
                Rewards = tasks.Where(t => t.Type == TaskType.Reward).ToList()
            };
        }

        // ===== CREATE =====

        public async Task<HabitTask> CreateTaskAsync(int userId, CreateTaskViewModel model)
        {
            var maxOrder = await _context.HabitTasks
                .Where(t => t.UserId == userId && t.Type == model.Type && t.IsActive)
                .MaxAsync(t => (int?)t.SortOrder) ?? -1;

            var task = new HabitTask
            {
                UserId    = userId,
                Type      = model.Type,
                Text      = model.Text.Trim(),
                Notes     = model.Notes?.Trim(),
                Priority  = model.Priority,
                SortOrder = maxOrder + 1,
                CreatedAt = DateTime.UtcNow
            };

            switch (model.Type)
            {
                case TaskType.Habit:
                    task.Up             = model.Up;
                    task.Down           = model.Down;
                    task.HabitFrequency = model.HabitFrequency;
                    break;

                case TaskType.Daily:
                    task.DailyFrequency   = model.DailyFrequency;
                    task.EveryX           = Math.Max(1, model.EveryX);
                    task.StartDate        = model.StartDate ?? DateTime.Today;
                    task.RepeatMonday     = model.RepeatMonday;
                    task.RepeatTuesday    = model.RepeatTuesday;
                    task.RepeatWednesday  = model.RepeatWednesday;
                    task.RepeatThursday   = model.RepeatThursday;
                    task.RepeatFriday     = model.RepeatFriday;
                    task.RepeatSaturday   = model.RepeatSaturday;
                    task.RepeatSunday     = model.RepeatSunday;
                    break;

                case TaskType.Todo:
                    task.DueDate = model.DueDate;
                    break;

                case TaskType.Reward:
                    task.GoldCost = Math.Max(1, model.GoldCost);
                    break;
            }

            _context.HabitTasks.Add(task);
            await _context.SaveChangesAsync();

            // Add checklist items
            if (model.ChecklistItems.Any())
            {
                for (int i = 0; i < model.ChecklistItems.Count; i++)
                {
                    var text = model.ChecklistItems[i]?.Trim();
                    if (string.IsNullOrEmpty(text)) continue;
                    _context.TaskChecklistItems.Add(new TaskChecklistItem
                    {
                        HabitTaskId = task.Id,
                        Text        = text,
                        SortOrder   = i
                    });
                }
                await _context.SaveChangesAsync();
            }

            return task;
        }

        // ===== UPDATE =====

        public async Task UpdateTaskAsync(int taskId, int userId, EditTaskViewModel model)
        {
            var task = await _context.HabitTasks
                .Include(t => t.ChecklistItems)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && t.IsActive);

            if (task == null) return;

            task.Text      = model.Text.Trim();
            task.Notes     = model.Notes?.Trim();
            task.Priority  = model.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            switch (task.Type)
            {
                case TaskType.Habit:
                    task.Up             = model.Up;
                    task.Down           = model.Down;
                    task.HabitFrequency = model.HabitFrequency;
                    break;

                case TaskType.Daily:
                    task.DailyFrequency  = model.DailyFrequency;
                    task.EveryX          = Math.Max(1, model.EveryX);
                    task.StartDate       = model.StartDate;
                    task.RepeatMonday    = model.RepeatMonday;
                    task.RepeatTuesday   = model.RepeatTuesday;
                    task.RepeatWednesday = model.RepeatWednesday;
                    task.RepeatThursday  = model.RepeatThursday;
                    task.RepeatFriday    = model.RepeatFriday;
                    task.RepeatSaturday  = model.RepeatSaturday;
                    task.RepeatSunday    = model.RepeatSunday;
                    break;

                case TaskType.Todo:
                    task.DueDate = model.DueDate;
                    break;

                case TaskType.Reward:
                    task.GoldCost = Math.Max(1, model.GoldCost);
                    break;
            }

            // Rebuild checklist: remove deleted items, keep existing, add new
            var keptIds = model.ExistingChecklist.Select(e => e.ItemId).ToHashSet();
            var toRemove = task.ChecklistItems?
                .Where(c => !keptIds.Contains(c.Id))
                .ToList() ?? new List<TaskChecklistItem>();

            _context.TaskChecklistItems.RemoveRange(toRemove);

            // Update text of kept items
            foreach (var existing in model.ExistingChecklist)
            {
                var item = task.ChecklistItems?.FirstOrDefault(c => c.Id == existing.ItemId);
                if (item != null) item.Text = existing.Text.Trim();
            }

            // Add new items
            int nextOrder = (task.ChecklistItems?.Count ?? 0);
            foreach (var text in model.NewChecklistItems)
            {
                var trimmed = text?.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                _context.TaskChecklistItems.Add(new TaskChecklistItem
                {
                    HabitTaskId = task.Id,
                    Text        = trimmed,
                    SortOrder   = nextOrder++
                });
            }

            await _context.SaveChangesAsync();
        }

        // ===== DELETE (soft) =====

        public async Task DeleteTaskAsync(int taskId, int userId)
        {
            var task = await _context.HabitTasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null) return;

            task.IsActive  = false;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // ===== SCORE =====

        public async Task<ScoreResultViewModel> ScoreTaskAsync(int taskId, int userId, string direction)
        {
            var task = await _context.HabitTasks
                .Include(t => t.ChecklistItems)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.IsActive);

            if (task == null)
                return new ScoreResultViewModel { Success = false, ErrorMessage = "Task not found" };

            if (task.UserId != userId)
                return new ScoreResultViewModel { Success = false, ErrorMessage = "Unauthorized" };

            var user = await _context.Users
                .Include(u => u.UserBadges)
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new ScoreResultViewModel { Success = false, ErrorMessage = "User not found" };

            int oldXP    = user.XP;
            int oldLevel = user.Level;
            bool isUp    = direction == "up";

            // Delta formula from Habitica: 0.9747^value × ±1
            double delta  = Math.Pow(AppConstants.TaskValueLimits.DECAY, task.Value) * (isUp ? 1 : -1);
            task.Value    = Math.Clamp(task.Value + delta, AppConstants.TaskValueLimits.MIN, AppConstants.TaskValueLimits.MAX);

            // Reward: gold check + deduction (no XP, no economy service call)
            if (task.Type == TaskType.Reward && isUp)
            {
                if (user.Gold < task.GoldCost)
                    return new ScoreResultViewModel
                    {
                        Success      = false,
                        NotEnoughGold = true,
                        NewGold      = user.Gold,
                        NewHP        = user.HP,
                        NewMana      = user.Mana,
                        ErrorMessage = "Not enough gold"
                    };
                user.Gold -= task.GoldCost;
            }

            // Economy: XP/Gold/Mana/HP/drops (all non-Reward tasks)
            EconomyResult? economy = null;
            if (task.Type != TaskType.Reward)
            {
                economy = await _economyService.ApplyTaskScoreEconomyAsync(user, task, delta, isUp);

                // Boss quest damage
                if (isUp && Math.Abs(delta) > 0)
                {
                    double critMult = economy?.IsCrit == true ? AppConstants.CRIT_MULT : 1.0;
                    await _bossQuestService.ApplyTaskDamageAsync(userId, Math.Abs(delta), task.Type, critMult);
                }
            }

            int xpGained = economy?.XpGained ?? 0;

            if (isUp)
            {
                user.TotalQuestsCompleted++;
                user.LastActiveDate = DateTime.UtcNow;
            }

            // Type-specific side-effects
            switch (task.Type)
            {
                case TaskType.Habit:
                    if (isUp) task.CounterUp++;
                    else      task.CounterDown++;
                    break;

                case TaskType.Daily:
                    if (isUp) task.IsCompleted = true;
                    break;

                case TaskType.Todo:
                    if (isUp) task.DateCompleted = DateTime.UtcNow;
                    break;
            }

            user.Level = Math.Min(AppConstants.MAX_LEVEL, _questService.CalculateLevel(user.XP));
            int levelsGained = user.Level - oldLevel;
            bool leveledUp   = levelsGained > 0;

            if (leveledUp)
            {
                user.StatPoints += levelsGained;
                user.HP = AppConstants.MAX_HP;   // full heal on level-up (Habitica behavior)
            }

            if (isUp) _questService.UpdateStreak(user);

            await _context.SaveChangesAsync();

            var newBadges = await _questService.AwardBadgesAsync(user, oldXP);

            return new ScoreResultViewModel
            {
                Success        = true,
                XpGained       = xpGained,
                NewXP          = user.XP,
                NewLevel       = user.Level,
                LeveledUp      = leveledUp,
                NewStreak      = user.CurrentStreak,
                NewValue       = task.Value,
                NewBadges      = newBadges,
                CounterUp      = task.CounterUp,
                CounterDown    = task.CounterDown,
                IsCompleted    = task.IsCompleted,
                GoldGained     = economy?.GoldGained ?? (isUp && task.Type == TaskType.Reward ? -task.GoldCost : 0),
                NewGold        = user.Gold,
                NewHP          = user.HP,
                NewMana        = user.Mana,
                IsCrit         = economy?.IsCrit ?? false,
                Died           = economy?.Died ?? false,
                DroppedItemIcon = economy?.DroppedItemIcon,
                DroppedItemName = economy?.DroppedItemName,
                StatPointsGained = levelsGained,
                NewStatPoints    = user.StatPoints
            };
        }

        // ===== IS DUE (shouldDo logic) =====

        public bool IsDue(HabitTask task, DateTime today)
        {
            if (task.Type != TaskType.Daily) return false;

            var start = task.StartDate?.Date ?? task.CreatedAt.Date;
            if (today.Date < start) return false;

            var freq = task.DailyFrequency ?? DailyFrequency.Weekly;

            return freq switch
            {
                DailyFrequency.Weekly => today.DayOfWeek switch
                {
                    DayOfWeek.Monday    => task.RepeatMonday,
                    DayOfWeek.Tuesday   => task.RepeatTuesday,
                    DayOfWeek.Wednesday => task.RepeatWednesday,
                    DayOfWeek.Thursday  => task.RepeatThursday,
                    DayOfWeek.Friday    => task.RepeatFriday,
                    DayOfWeek.Saturday  => task.RepeatSaturday,
                    DayOfWeek.Sunday    => task.RepeatSunday,
                    _                   => false
                },

                DailyFrequency.Daily when task.EveryX > 1
                    => (today.Date - start).Days % task.EveryX == 0,

                DailyFrequency.Monthly
                    => today.Day == start.Day,

                DailyFrequency.Yearly
                    => today.Month == start.Month && today.Day == start.Day,

                _ => true  // Daily every day (EveryX == 1)
            };
        }

        // ===== CRON =====

        public async Task RunCronAsync(int userId)
        {
            var today = DateTime.Today;

            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return;
            if (user.LastCronDate?.Date == today) return;

            var tasks = await _context.HabitTasks
                .Where(t => t.UserId == userId && t.IsActive)
                .Include(t => t.ChecklistItems)
                .ToListAsync();

            var yesterday = today.AddDays(-1);

            bool anyDailyDue        = false;
            bool allDailiesCompleted = true;

            foreach (var task in tasks)
            {
                switch (task.Type)
                {
                    case TaskType.Daily:
                        bool isDueYesterday = IsDue(task, yesterday);

                        // Perfect Day tracking — read IsCompleted before reset
                        if (isDueYesterday)
                        {
                            anyDailyDue = true;
                            if (!task.IsCompleted) allDailiesCompleted = false;
                        }

                        if (isDueYesterday && !task.IsCompleted)
                        {
                            // Stealth: Rogue spell absorbs daily damage instances
                            if (user.StealthBuff > 0)
                            {
                                user.StealthBuff--;
                                // Absorbed — skip HP damage and streak decrement
                            }
                            else
                            {
                                // FrozenStreaks: Mage Chilling Frost — skip streak change
                                if (!user.FrozenStreaks && task.Streak > 0)
                                    task.Streak--;

                                double cronDelta = Math.Pow(AppConstants.TaskValueLimits.DECAY, task.Value) * -1;
                                await _economyService.ApplyCronDamageAsync(user, task, cronDelta);
                                await _bossQuestService.ApplyMissedDailyRageAsync(userId, Math.Abs(cronDelta), AppConstants.PriorityMultipliers.Get(task.Priority));
                            }
                        }
                        else if (task.IsCompleted)
                        {
                            // FrozenStreaks: skip streak increment
                            if (!user.FrozenStreaks)
                                task.Streak++;
                        }
                        task.IsCompleted = false;
                        break;

                    case TaskType.Habit:
                        bool shouldReset = task.HabitFrequency switch
                        {
                            HabitResetFrequency.Daily   => true,
                            HabitResetFrequency.Weekly  => today.DayOfWeek == DayOfWeek.Monday,
                            HabitResetFrequency.Monthly => today.Day == 1,
                            _                           => false
                        };
                        if (shouldReset)
                        {
                            task.CounterUp   = 0;
                            task.CounterDown = 0;
                        }
                        break;
                }
            }

            // Buff reset every cron (Habitica: overwrites, never accumulates)
            user.BuffSTR    = 0;
            user.BuffCON    = 0;
            user.BuffINT    = 0;
            user.BuffPER    = 0;
            user.BuffExpiry = null;

            // Perfect Day: all due Dailies completed → grant ceil(level/2) to all stats
            if (anyDailyDue && allDailiesCompleted)
            {
                int buffAmount  = (int)Math.Ceiling(Math.Min(user.Level, 100) / 2.0);
                user.BuffSTR    = buffAmount;
                user.BuffCON    = buffAmount;
                user.BuffINT    = buffAmount;
                user.BuffPER    = buffAmount;
                user.BuffExpiry = DateTime.UtcNow.AddDays(1);
            }

            await _economyService.ApplyCronManaRegenAsync(user);
            user.DailyDropCount    = 0;
            user.LastDropResetDate = today;
            user.LastCronDate      = today;

            // Reset Mage Chilling Frost — only covers one cron cycle
            if (user.FrozenStreaks)
                user.FrozenStreaks = false;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Cron ran for user {UserId} on {Date}", userId, today);
        }

        // ===== CHECKLIST TOGGLE =====

        public async Task<bool> ToggleChecklistItemAsync(int itemId, int userId)
        {
            var item = await _context.TaskChecklistItems
                .Include(c => c.HabitTask)
                .FirstOrDefaultAsync(c => c.Id == itemId);

            if (item == null || item.HabitTask?.UserId != userId) return false;

            item.IsCompleted = !item.IsCompleted;
            await _context.SaveChangesAsync();
            return item.IsCompleted;
        }

        // ===== REORDER =====

        public async Task ReorderTasksAsync(int userId, TaskType type, List<int> orderedIds)
        {
            var tasks = await _context.HabitTasks
                .Where(t => t.UserId == userId && t.Type == type && t.IsActive)
                .ToListAsync();

            var taskMap = tasks.ToDictionary(t => t.Id);

            for (int i = 0; i < orderedIds.Count; i++)
            {
                if (taskMap.TryGetValue(orderedIds[i], out var task))
                    task.SortOrder = i;
            }

            await _context.SaveChangesAsync();
        }

        // ===== CLEAR COMPLETED TODOS =====

        public async Task ClearCompletedTodosAsync(int userId)
        {
            var completed = await _context.HabitTasks
                .Where(t => t.UserId == userId && t.Type == TaskType.Todo &&
                            t.DateCompleted != null && t.IsActive)
                .ToListAsync();

            foreach (var task in completed)
            {
                task.IsActive  = false;
                task.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
