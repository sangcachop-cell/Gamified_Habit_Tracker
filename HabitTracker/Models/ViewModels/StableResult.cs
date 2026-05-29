namespace HabitTracker.Models.ViewModels
{
    public class StableResult
    {
        public bool    Success          { get; init; }
        public string? Error            { get; init; }

        // Hatch
        public string? HatchedPetKey   { get; init; }
        public string? HatchedPetName  { get; init; }
        public string? PetImagePath    { get; init; }

        // Feed
        public bool    Evolved          { get; init; }
        public int     NewFeedingPoints { get; init; }
        public string? MountIconPath    { get; init; }
        public int     NewFoodQuantity  { get; init; }

        // SetActive
        public string? NewActivePetKey   { get; init; }
        public string? NewActiveMountKey { get; init; }
    }
}
