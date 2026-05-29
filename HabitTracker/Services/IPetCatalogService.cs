using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services
{
    public record PetCatalogEntry(
        string PetKey,
        string AnimalKey,
        string ColorKey,
        PetCategory Category,
        bool CanBecomeMount,
        string PetImagePath,
        string? MountIconPath);

    public interface IPetCatalogService
    {
        IReadOnlyList<PetCatalogEntry> AllEntries { get; }

        bool IsValidHatch(string animalKey, string colorKey);
        bool CanBecomeMount(string petKey);
        PetCatalogEntry? GetPetInfo(string petKey);

        IReadOnlyList<AnimalGroup> GetAnimalGroups();
        IReadOnlyList<AnimalGroup> GetAnimalGroupsForMounts();
        IReadOnlyList<string> GetValidPotionColors(string animalKey);
    }
}
