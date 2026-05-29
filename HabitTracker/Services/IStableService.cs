using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services
{
    public interface IStableService
    {
        Task<StableResult> HatchAsync(int userId, int eggGameItemId, int potionGameItemId);
        Task<StableResult> FeedAsync(int userId, string petKey, int foodGameItemId);
        Task<StableResult> SetActivePetAsync(int userId, string? petKey);
        Task<StableResult> SetActiveMountAsync(int userId, string? mountKey);
        Task<StableViewModel> GetStableViewModelAsync(int userId);
    }
}
