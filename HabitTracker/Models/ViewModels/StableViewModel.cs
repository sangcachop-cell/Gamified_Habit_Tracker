namespace HabitTracker.Models.ViewModels
{
    public class StableViewModel
    {
        public User User { get; set; } = null!;

        public List<UserInventoryItem> EggsInInventory    { get; set; } = new();
        public List<UserInventoryItem> PotionsInInventory { get; set; } = new();
        public List<UserInventoryItem> FoodInInventory    { get; set; } = new();

        public List<UserPet> OwnedPets   { get; set; } = new();
        public List<UserPet> OwnedMounts { get; set; } = new();

        public List<AnimalGroup> PetsGrid   { get; set; } = new();
        public List<AnimalGroup> MountsGrid { get; set; } = new();

        public int TotalPetsInCatalog    { get; set; }
        public int TotalMountsInCatalog  { get; set; }

        // Pets collected = pets still in pet form + pets that evolved to mounts
        public int CollectedPetsCount => OwnedPets.Count + OwnedMounts.Count;
    }
}
