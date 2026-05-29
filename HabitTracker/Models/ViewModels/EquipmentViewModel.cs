using HabitTracker.Services;

namespace HabitTracker.Models.ViewModels
{
    public class EquipmentViewModel
    {
        public User User { get; set; } = null!;

        // Owned GearItems grouped by slot name
        public Dictionary<string, List<GearItem>> OwnedBySlot { get; set; } = new();

        // Full gear catalog for the shop, filtered to user's class + "all", grouped by slot
        public Dictionary<string, List<GearItem>> ShopBySlot { get; set; } = new();

        // Keys the user already owns — used to show "Owned" badge in shop
        public HashSet<string> OwnedKeys { get; set; } = new();

        // Effective stats (with equipped gear applied)
        public EffectiveStats Stats { get; set; } = null!;

        // Current battle-gear slot → key map
        public Dictionary<string, string?> EquippedKeys { get; set; } = new();

        // Current costume slot → key map
        public Dictionary<string, string?> CostumeKeys { get; set; } = new();
    }
}
