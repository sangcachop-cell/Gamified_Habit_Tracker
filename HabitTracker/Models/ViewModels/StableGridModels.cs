namespace HabitTracker.Models.ViewModels
{
    public enum PetCategory { Drop, Premium, Quest, Wacky, Special }

    public record AnimalGroup(
        string AnimalKey,
        string DisplayName,
        PetCategory Category,
        IReadOnlyList<PetSlotEntry> Slots);

    public record PetSlotEntry(
        string PetKey,
        string ColorKey,
        string ColorDisplayName,
        PetCategory SlotCategory,
        bool CanBecomeMount,
        string PetImagePath,
        string? MountIconPath,
        bool IsOwned,
        bool IsMount,
        int FeedingPoints,
        bool IsActivePet,
        bool IsActiveMount);
}
