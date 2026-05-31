using HabitTracker.Constants;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services;

public interface IBossQuestService
{
    // Shop
    Task<List<BossQuest>> GetShopQuestsAsync(int userId);
    Task<(bool Success, string? Error)> BuyScrollAsync(int userId, string questKey);

    // Lifecycle
    Task<(bool Success, string? Error)> InvitePartyAsync(int leaderId, string questKey);
    Task<(bool Success, string? Error)> AcceptQuestAsync(int userId);
    Task<(bool Success, string? Error)> RejectQuestAsync(int userId);
    Task<(bool Success, string? Error)> ForceStartAsync(int leaderId);
    Task<(bool Success, string? Error)> CancelQuestAsync(int leaderId);
    Task<(bool Success, string? Error)> AbortQuestAsync(int leaderId);

    // Combat (called by TaskService and SpellService)
    Task ApplyTaskDamageAsync(int userId, double rawDelta, TaskType taskType, double critMult);
    Task ApplyMissedDailyRageAsync(int userId, double rawDelta, double taskPriority);
    Task FinishQuestAsync(int partyQuestId);

    // View data
    Task<PartyQuest?> GetActiveQuestAsync(int partyId);
    Task<PartyQuestStatusDto?> GetQuestStatusAsync(int partyId);
}
