using HabitTracker.Models;
using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services
{
    public interface IEconomyService
    {
        /// <summary>Called by TaskService.ScoreTaskAsync — applies XP/Gold/Mana/HP/drops in-memory. Does NOT SaveChanges.</summary>
        Task<EconomyResult> ApplyTaskScoreEconomyAsync(User user, HabitTask task, double delta, bool isUp);

        /// <summary>Called by TaskService.RunCronAsync per missed Daily — applies HP damage in-memory. Does NOT SaveChanges.</summary>
        Task ApplyCronDamageAsync(User user, HabitTask missedDaily, double delta);

        /// <summary>Called by TaskService.RunCronAsync after the daily loop — regenerates 2% mana in-memory. Does NOT SaveChanges.</summary>
        Task ApplyCronManaRegenAsync(User user);

        Task<(bool Success, string? Error)> BuyHealthPotionAsync(int userId);
        Task<bool> ToggleSleepAsync(int userId);
        Task ReviveAsync(int userId);
        Task<List<UserInventoryItem>> GetInventoryAsync(int userId);

        // ===== Phase 4 =====
        Task<(bool Success, string? Error)> BuyGemAsync(int userId);
        Task<ArmoireResult> PullArmoireAsync(int userId);
        Task<(bool Success, string? Error, int GoldAwarded)> SellItemAsync(int userId, int gameItemId);
    }
}
