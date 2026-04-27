using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(AppDbContext context, ILogger<InventoryService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        public async Task EnsureStarterItemsAsync(int userId)
        {
            bool hasItems = await _context.UserInventoryItems.AnyAsync(i => i.UserId == userId);
            if (hasItems) return;

            // 1 Bread at Storage (0,0) — 1×1
            _context.UserInventoryItems.Add(new UserInventoryItem
            {
                UserId = userId, ItemId = "bread",
                ContainerType = ItemCatalogue.STORAGE, GridX = 0, GridY = 0,
                AcquiredAt = DateTime.Now
            });

            // 1 Water Bottle at Storage (1,0) — occupies cols 1-2, row 0
            _context.UserInventoryItems.Add(new UserInventoryItem
            {
                UserId = userId, ItemId = "water_bottle",
                ContainerType = ItemCatalogue.STORAGE, GridX = 1, GridY = 0,
                AcquiredAt = DateTime.Now
            });

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} received starter items");
        }

        public async Task<List<UserInventoryItem>> GetItemsAsync(int userId, string containerType)
        {
            return await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ContainerType == containerType)
                .ToListAsync();
        }

        public async Task<bool> RotateItemAsync(int userId, int itemId)
        {
            var item = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.Id == itemId && i.UserId == userId);

            if (item == null) return false;
            if (!ItemCatalogue.Items.TryGetValue(item.ItemId, out var def)) return false;
            if (!ItemCatalogue.CanRotate(item.ItemId)) return false;

            // After toggle: new dimensions swap W and H
            int newW = item.IsRotated ? def.Width  : def.Height;
            int newH = item.IsRotated ? def.Height : def.Width;

            // Check container bounds
            var (cols, rows) = ItemCatalogue.ContainerSize(item.ContainerType);
            if (item.GridX + newW > cols || item.GridY + newH > rows)
            {
                _logger.LogInformation($"Rotate rejected: item {itemId} would exceed container bounds");
                return false;
            }

            // Check overlap with sibling items
            var siblings = await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ContainerType == item.ContainerType && i.Id != itemId)
                .ToListAsync();

            if (Overlaps(siblings, item.GridX, item.GridY, newW, newH))
            {
                _logger.LogInformation($"Rotate rejected: item {itemId} would overlap another item");
                return false;
            }

            item.IsRotated = !item.IsRotated;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MoveItemAsync(int userId, int itemId, string targetContainer, int x, int y)
        {
            var item = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.Id == itemId && i.UserId == userId);

            if (item == null) return false;
            if (!ItemCatalogue.Items.TryGetValue(item.ItemId, out var def)) return false;

            int w = item.IsRotated ? def.Height : def.Width;
            int h = item.IsRotated ? def.Width  : def.Height;

            // Bounds check
            var (cols, rows) = ItemCatalogue.ContainerSize(targetContainer);
            if (x < 0 || y < 0 || x + w > cols || y + h > rows) return false;

            // Overlap check (exclude self)
            var siblings = await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ContainerType == targetContainer && i.Id != itemId)
                .ToListAsync();

            if (Overlaps(siblings, x, y, w, h)) return false;

            item.ContainerType = targetContainer;
            item.GridX = x;
            item.GridY = y;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} moved item {itemId} to {targetContainer} ({x},{y})");
            return true;
        }

        // ── helpers ──────────────────────────────────────────────────────────

        private static bool Overlaps(IEnumerable<UserInventoryItem> items, int x, int y, int w, int h)
        {
            foreach (var other in items)
            {
                if (!ItemCatalogue.Items.TryGetValue(other.ItemId, out var d)) continue;
                int ow = other.IsRotated ? d.Height : d.Width;
                int oh = other.IsRotated ? d.Width  : d.Height;

                bool noOverlap = x >= other.GridX + ow || x + w <= other.GridX ||
                                 y >= other.GridY + oh || y + h <= other.GridY;
                if (!noOverlap) return true;
            }
            return false;
        }
    }
}
