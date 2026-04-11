using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class SearchService : ISearchService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SearchService> _logger;

        public SearchService(AppDbContext context, ILogger<SearchService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Quest>> SearchQuestsAsync(
            int userId,
            string? searchQuery,
            string? category,
            string? difficulty,
            string? frequency,
            bool? completedToday,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            try
            {
                var query = _context.Quests.Where(q => q.IsActive);

                // Search by name/description
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    var search = searchQuery.ToLower();
                    query = query.Where(q =>
                        q.Name.ToLower().Contains(search) ||
                        (q.Description != null && q.Description.ToLower().Contains(search)));
                }

                // Filter by category (system category only)
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(q => q.Category == category);
                }

                // Filter by difficulty
                if (!string.IsNullOrEmpty(difficulty))
                    query = query.Where(q => q.Difficulty == difficulty);

                // Filter by frequency
                if (!string.IsNullOrEmpty(frequency))
                    query = query.Where(q => q.Frequency == frequency);

                // Filter by date range
                if (dateFrom.HasValue)
                    query = query.Where(q => q.CreatedAt >= dateFrom.Value);

                if (dateTo.HasValue)
                    query = query.Where(q => q.CreatedAt <= dateTo.Value);

                // Filter by completed today
                if (completedToday.HasValue)
                {
                    var completedTodayIds = await _context.UserQuests
                        .Where(uq => uq.UserId == userId && uq.CompletedDate == DateTime.Today)
                        .Select(uq => uq.QuestId)
                        .ToListAsync();

                    if (completedToday.Value)
                        query = query.Where(q => completedTodayIds.Contains(q.Id));
                    else
                        query = query.Where(q => !completedTodayIds.Contains(q.Id));
                }

                var quests = await query
                    .OrderBy(q => q.Category)
                    .ThenBy(q => q.Name)
                    .ToListAsync();

                _logger.LogInformation($"User {userId} searched: found {quests.Count} quests");
                return quests;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Search error: {ex.Message}");
                return new List<Quest>();
            }
        }

        public async Task<List<User>> SearchUsersAsync(string? query, int limit = 10)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return new List<User>();

                var search = query.ToLower();
                return await _context.Users
                    .Where(u => u.Username.ToLower().Contains(search) || u.Email.ToLower().Contains(search))
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"User search error: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<List<string>> AutocompleteQuestAsync(string query, int limit = 5)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return new List<string>();

                var search = query.ToLower();
                return await _context.Quests
                    .Where(q => q.IsActive && q.Name.ToLower().Contains(search))
                    .Select(q => q.Name)
                    .Distinct()
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Autocomplete error: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<Quest>> GetTrendingQuestsAsync(int limit = 10)
        {
            try
            {
                var sevenDaysAgo = DateTime.Today.AddDays(-7);

                // Get quest IDs with most completions in last 7 days
                var trendingIds = await _context.UserQuests
                    .Where(uq => uq.CompletedDate >= sevenDaysAgo && uq.Status == "Confirmed")
                    .GroupBy(uq => uq.QuestId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(limit)
                    .ToListAsync();

                // Get quest details
                var quests = await _context.Quests
                    .Where(q => trendingIds.Contains(q.Id))
                    .OrderBy(q => trendingIds.IndexOf(q.Id))
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {quests.Count} trending quests");
                return quests;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Trending quests error: {ex.Message}");
                return new List<Quest>();
            }
        }

        public async Task<List<(Quest Quest, int Count)>> GetMostCompletedQuestsAsync(int limit = 10)
        {
            try
            {
                // Step 1: Get completion counts from database
                var completionData = await _context.UserQuests
                    .Where(uq => uq.Status == "Confirmed")
                    .GroupBy(uq => uq.QuestId)
                    .Select(g => new { QuestId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(limit)
                    .ToListAsync();

                // Step 2: Get quest details
                var questIds = completionData.Select(x => x.QuestId).ToList();
                var quests = await _context.Quests
                    .Where(q => questIds.Contains(q.Id))
                    .ToListAsync();

                // Step 3: Combine in memory
                var result = completionData
                    .Select(cc => (quests.FirstOrDefault(q => q.Id == cc.QuestId), cc.Count))
                    .Where(x => x.Item1 != null)
                    .ToList();

                _logger.LogInformation($"Retrieved {result.Count} most completed quests");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Most completed error: {ex.Message}");
                return new List<(Quest, int)>();
            }
        }
    }
}