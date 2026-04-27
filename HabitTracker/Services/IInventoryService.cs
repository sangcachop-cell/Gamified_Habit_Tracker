using HabitTracker.Models;

namespace HabitTracker.Services
{
    public interface IInventoryService
    {
        /// <summary>Seeds 1 Bread + 1 Water Bottle for users who have no items yet.</summary>
        Task EnsureStarterItemsAsync(int userId);

        /// <summary>Returns all placed items for a user in a given container.</summary>
        Task<List<UserInventoryItem>> GetItemsAsync(int userId, string containerType);

        /// <summary>
        /// Rotates an item 90°. Returns false if rotation would cause out-of-bounds or overlap.
        /// </summary>
        Task<bool> RotateItemAsync(int userId, int itemId);

        /// <summary>
        /// Moves an item to a new container/position. Returns false if invalid.
        /// </summary>
        Task<bool> MoveItemAsync(int userId, int itemId, string targetContainer, int x, int y);
    }
}
