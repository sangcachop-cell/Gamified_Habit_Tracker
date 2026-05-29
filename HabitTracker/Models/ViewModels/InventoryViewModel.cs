namespace HabitTracker.Models.ViewModels
{
    public class InventoryViewModel
    {
        public User                    User      { get; set; } = null!;
        public List<UserInventoryItem> Items     { get; set; } = new();
        public List<UserGearItem>      OwnedGear { get; set; } = new();
    }
}
