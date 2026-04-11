using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(AppDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Category> CreateCategoryAsync(Category category, int adminUserId)
        {
            category.CreatedByUserId = adminUserId;
            category.CreatedAt = DateTime.UtcNow;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Category created: {category.Name} by user {adminUserId}");
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Category updated: {category.Name}");
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Category soft-deleted: {category.Name}");
            return true;
        }

        public async Task<List<Category>> SearchCategoriesAsync(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAllCategoriesAsync();

            var search = query.ToLower();
            return await _context.Categories
                .Where(c => c.IsActive && (c.Name.ToLower().Contains(search) || c.Description.ToLower().Contains(search)))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public List<string> GetSystemCategories()
        {
            return new List<string> { "Sức khỏe", "Học tập", "Tinh thần", "Tài chính" };
        }
    }
}