using HabitTracker.Models;

namespace HabitTracker.Services
{
    public interface ICategoryService
    {
        // CRUD
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category, int adminUserId);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);

        // Search
        Task<List<Category>> SearchCategoriesAsync(string? query);

        // Get system categories
        List<string> GetSystemCategories();
    }
}