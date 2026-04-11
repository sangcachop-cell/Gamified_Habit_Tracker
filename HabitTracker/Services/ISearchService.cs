using HabitTracker.Models;

namespace HabitTracker.Services
{
    public interface ISearchService
    {
        // Search quests
        Task<List<Quest>> SearchQuestsAsync(
            int userId,
            string? searchQuery,
            string? category,
            string? difficulty,
            string? frequency,
            bool? completedToday,
            DateTime? dateFrom,
            DateTime? dateTo);

        // Search users
        Task<List<User>> SearchUsersAsync(string? query, int limit = 10);

        // Autocomplete
        Task<List<string>> AutocompleteQuestAsync(string query, int limit = 5);

        // Trending quests (last 7 days)
        Task<List<Quest>> GetTrendingQuestsAsync(int limit = 10);

        // Most completed
        Task<List<(Quest Quest, int Count)>> GetMostCompletedQuestsAsync(int limit = 10);
    }
}