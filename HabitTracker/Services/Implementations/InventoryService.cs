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
            bool hasBread = await _context.UserInventoryItems
                .AnyAsync(i => i.UserId == userId && i.ItemId == "bread");
            bool hasWater = await _context.UserInventoryItems
                .AnyAsync(i => i.UserId == userId && i.ItemId == "water_bottle");

            if (!hasBread)
                _context.UserInventoryItems.Add(new UserInventoryItem
                {
                    UserId = userId, ItemId = "bread",
                    ContainerType = ItemCatalogue.STORAGE, GridX = 0, GridY = 0,
                    AcquiredAt = DateTime.Now
                });

            if (!hasWater)
                _context.UserInventoryItems.Add(new UserInventoryItem
                {
                    UserId = userId, ItemId = "water_bottle",
                    ContainerType = ItemCatalogue.STORAGE, GridX = 1, GridY = 0,
                    AcquiredAt = DateTime.Now
                });

            bool hasBackpack = await _context.UserInventoryItems
                .AnyAsync(i => i.UserId == userId && i.ItemId == "simple_backpack");
            bool hasArmor = await _context.UserInventoryItems
                .AnyAsync(i => i.UserId == userId && i.ItemId == "simple_armor");
            bool hasRig = await _context.UserInventoryItems
                .AnyAsync(i => i.UserId == userId && i.ItemId == "simple_rig");

            // Also check equipped slots
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                hasBackpack = hasBackpack || user.EquippedBackpackItem == "simple_backpack";
                hasArmor    = hasArmor    || user.EquippedArmorItem    == "simple_armor";
                hasRig      = hasRig      || user.EquippedRigItem      == "simple_rig";
            }

            if (!hasBackpack)
                _context.UserInventoryItems.Add(new UserInventoryItem
                {
                    UserId = userId, ItemId = "simple_backpack",
                    ContainerType = ItemCatalogue.STORAGE, GridX = 0, GridY = 1,
                    AcquiredAt = DateTime.Now
                });
            if (!hasArmor)
                _context.UserInventoryItems.Add(new UserInventoryItem
                {
                    UserId = userId, ItemId = "simple_armor",
                    ContainerType = ItemCatalogue.STORAGE, GridX = 3, GridY = 0,
                    AcquiredAt = DateTime.Now
                });
            if (!hasRig)
                _context.UserInventoryItems.Add(new UserInventoryItem
                {
                    UserId = userId, ItemId = "simple_rig",
                    ContainerType = ItemCatalogue.STORAGE, GridX = 4, GridY = 0,
                    AcquiredAt = DateTime.Now
                });

            if (!hasBread || !hasWater || !hasBackpack || !hasArmor || !hasRig)
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User {userId} received missing starter items");
            }
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

            // Slot-size constraint (Pocket = 1×1, Rig = 2×1)
            var slotConstraint = ItemCatalogue.SlotConstraint(targetContainer);
            if (slotConstraint != null && (w != slotConstraint.Value.W || h != slotConstraint.Value.H))
                return false;

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

        public async Task<bool> EquipItemAsync(int userId, int userInventoryItemId)
        {
            var invItem = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.Id == userInventoryItemId && i.UserId == userId);
            if (invItem == null) return false;
            if (!ItemCatalogue.IsEquippable(invItem.ItemId)) return false;

            var eq   = ItemCatalogue.Equipment[invItem.ItemId];
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            // If slot occupied by same item, nothing to do
            string? current = eq.Slot switch {
                ItemCatalogue.SLOT_BACKPACK => user.EquippedBackpackItem,
                ItemCatalogue.SLOT_ARMOR    => user.EquippedArmorItem,
                _                           => user.EquippedRigItem,
            };
            if (current == invItem.ItemId) return false;

            // If slot occupied by a different item, unequip it first (return to storage)
            if (current != null) await UnequipSlotAsync(userId, eq.Slot);

            // Remove from inventory
            _context.UserInventoryItems.Remove(invItem);

            // Set slot
            switch (eq.Slot)
            {
                case ItemCatalogue.SLOT_BACKPACK: user.EquippedBackpackItem = invItem.ItemId; break;
                case ItemCatalogue.SLOT_ARMOR:    user.EquippedArmorItem    = invItem.ItemId; break;
                default:                          user.EquippedRigItem      = invItem.ItemId; break;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} equipped {invItem.ItemId} to {eq.Slot}");
            return true;
        }

        public async Task<bool> UnequipSlotAsync(int userId, string slot)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            string? itemId = slot switch {
                ItemCatalogue.SLOT_BACKPACK => user.EquippedBackpackItem,
                ItemCatalogue.SLOT_ARMOR    => user.EquippedArmorItem,
                _                           => user.EquippedRigItem,
            };
            if (itemId == null) return false;
            if (!ItemCatalogue.Items.TryGetValue(itemId, out var def)) return false;

            // Find free slot in Storage
            var storageItems = await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ContainerType == ItemCatalogue.STORAGE)
                .ToListAsync();
            var (cols, rows) = ItemCatalogue.ContainerSize(ItemCatalogue.STORAGE);
            var freePos = FindFreeSlot(storageItems, def.Width, def.Height, cols, rows);
            if (freePos == null) return false;

            _context.UserInventoryItems.Add(new UserInventoryItem
            {
                UserId = userId, ItemId = itemId,
                ContainerType = ItemCatalogue.STORAGE,
                GridX = freePos.Value.x, GridY = freePos.Value.y,
                AcquiredAt = DateTime.Now
            });

            switch (slot)
            {
                case ItemCatalogue.SLOT_BACKPACK: user.EquippedBackpackItem = null; break;
                case ItemCatalogue.SLOT_ARMOR:    user.EquippedArmorItem    = null; break;
                default:                          user.EquippedRigItem      = null; break;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} unequipped {itemId} from {slot}");
            return true;
        }

        // ── helpers ──────────────────────────────────────────────────────────

        private static (int x, int y)? FindFreeSlot(
            List<UserInventoryItem> existing, int w, int h, int cols, int rows)
        {
            for (int y = 0; y <= rows - h; y++)
            for (int x = 0; x <= cols - w; x++)
                if (!Overlaps(existing, x, y, w, h)) return (x, y);
            return null;
        }

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
