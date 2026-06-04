using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class StableService : IStableService
    {
        private readonly AppDbContext _context;
        private readonly IPetCatalogService _catalog;
        private readonly IAchievementService _achievements;

        public StableService(AppDbContext context, IPetCatalogService catalog, IAchievementService achievements)
        {
            _context      = context;
            _catalog      = catalog;
            _achievements = achievements;
        }

        public async Task<StableResult> HatchAsync(int userId, int eggGameItemId, int potionGameItemId)
        {
            var egg = await _context.GameItems.FindAsync(eggGameItemId);
            if (egg == null || egg.Type != ItemType.Egg)
                return Fail("Invalid egg.");

            var potion = await _context.GameItems.FindAsync(potionGameItemId);
            if (potion == null || potion.Type != ItemType.HatchingPotion)
                return Fail("Invalid potion.");

            var eggInv = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.UserId == userId && i.GameItemId == eggGameItemId);
            if (eggInv == null || eggInv.Quantity < 1)
                return Fail("No egg in inventory.");

            var potionInv = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.UserId == userId && i.GameItemId == potionGameItemId);
            if (potionInv == null || potionInv.Quantity < 1)
                return Fail("No potion in inventory.");

            var animalKey = egg.AnimalKey;
            var colorKey  = potion.PotionColorKey;
            if (string.IsNullOrEmpty(animalKey) || string.IsNullOrEmpty(colorKey))
                return Fail("Invalid item keys.");

            // Quest eggs can only be hatched with drop potions
            if (!_catalog.IsValidHatch(animalKey, colorKey))
                return Fail($"Quest eggs can only be hatched with standard potions.");

            var petKey = $"{animalKey}-{colorKey}";
            var exists = await _context.UserPets
                .AnyAsync(p => p.UserId == userId && p.PetKey == petKey);
            if (exists)
                return Fail($"{petKey} already hatched.");

            DeductInventory(eggInv);
            DeductInventory(potionInv);

            _context.UserPets.Add(new UserPet
            {
                UserId        = userId,
                PetKey        = petKey,
                FeedingPoints = 5,
                IsMount       = false,
                HatchedAt     = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            var pet = new UserPet { PetKey = petKey };
            return new StableResult
            {
                Success        = true,
                HatchedPetKey  = petKey,
                HatchedPetName = $"{animalKey} ({colorKey})",
                PetImagePath   = pet.PetImagePath
            };
        }

        public async Task<StableResult> FeedAsync(int userId, string petKey, int foodGameItemId)
        {
            var userPet = await _context.UserPets
                .FirstOrDefaultAsync(p => p.UserId == userId && p.PetKey == petKey);
            if (userPet == null)
                return Fail("Pet not found.");
            if (userPet.IsMount)
                return Fail("Cannot feed a mount.");

            var food = await _context.GameItems.FindAsync(foodGameItemId);
            if (food == null || food.Type != ItemType.Food)
                return Fail("Invalid food.");

            var foodInv = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.UserId == userId && i.GameItemId == foodGameItemId);
            if (foodInv == null || foodInv.Quantity < 1)
                return Fail("No food in inventory.");

            // Preferred food (Target matches pet color) = +5 pts, otherwise +2 (Habitica feed.js)
            int pts = (!string.IsNullOrEmpty(food.Target) && food.Target == userPet.ColorName) ? 5 : 2;

            DeductInventory(foodInv);
            userPet.FeedingPoints += pts;

            bool evolved = false;
            if (userPet.FeedingPoints >= 50)
            {
                userPet.FeedingPoints = 50;

                // Wacky pets cap at 50 but cannot evolve into mounts
                if (_catalog.CanBecomeMount(petKey))
                {
                    userPet.IsMount = true;
                    evolved         = true;

                    var user = await _context.Users.FindAsync(userId);
                    if (user != null && user.ActivePetKey == petKey)
                        user.ActivePetKey = null;
                }
            }

            await _context.SaveChangesAsync();

            if (evolved)
                await _achievements.CheckStableAsync(userId);

            return new StableResult
            {
                Success          = true,
                Evolved          = evolved,
                NewFeedingPoints = userPet.FeedingPoints,
                MountIconPath    = evolved ? userPet.MountIconPath : null,
                NewFoodQuantity  = foodInv.Quantity
            };
        }

        public async Task<StableResult> SetActivePetAsync(int userId, string? petKey)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Fail("User not found.");

            if (!string.IsNullOrEmpty(petKey))
            {
                var owned = await _context.UserPets
                    .AnyAsync(p => p.UserId == userId && p.PetKey == petKey && !p.IsMount);
                if (!owned) return Fail("Pet not owned.");
            }

            user.ActivePetKey = string.IsNullOrEmpty(petKey) ? null : petKey;
            if (!string.IsNullOrEmpty(petKey))
                user.ActiveMountKey = null;
            await _context.SaveChangesAsync();

            return new StableResult { Success = true, NewActivePetKey = user.ActivePetKey };
        }

        public async Task<StableResult> SetActiveMountAsync(int userId, string? mountKey)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Fail("User not found.");

            if (!string.IsNullOrEmpty(mountKey))
            {
                var owned = await _context.UserPets
                    .AnyAsync(p => p.UserId == userId && p.PetKey == mountKey && p.IsMount);
                if (!owned) return Fail("Mount not owned.");
            }

            user.ActiveMountKey = string.IsNullOrEmpty(mountKey) ? null : mountKey;
            if (!string.IsNullOrEmpty(mountKey))
                user.ActivePetKey = null;
            await _context.SaveChangesAsync();

            return new StableResult { Success = true, NewActiveMountKey = user.ActiveMountKey };
        }

        public async Task<StableViewModel> GetStableViewModelAsync(int userId)
        {
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId) ?? new User();

            var eggs = await _context.UserInventoryItems.AsNoTracking()
                .Include(i => i.GameItem)
                .Where(i => i.UserId == userId && i.GameItem!.Type == ItemType.Egg && i.Quantity > 0)
                .OrderBy(i => i.GameItem!.Name)
                .ToListAsync();

            var potions = await _context.UserInventoryItems.AsNoTracking()
                .Include(i => i.GameItem)
                .Where(i => i.UserId == userId && i.GameItem!.Type == ItemType.HatchingPotion && i.Quantity > 0)
                .OrderBy(i => i.GameItem!.Rarity).ThenBy(i => i.GameItem!.Name)
                .ToListAsync();

            var food = await _context.UserInventoryItems.AsNoTracking()
                .Include(i => i.GameItem)
                .Where(i => i.UserId == userId && i.GameItem!.Type == ItemType.Food && i.Quantity > 0)
                .OrderBy(i => i.GameItem!.Name)
                .ToListAsync();

            // Single query for all user pets (replaces previous 2 queries)
            var allUserPets = await _context.UserPets.AsNoTracking()
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var ownedPets   = allUserPets.Where(p => !p.IsMount).OrderBy(p => p.PetKey).ToList();
            var ownedMounts = allUserPets.Where(p =>  p.IsMount).OrderBy(p => p.PetKey).ToList();

            var ownedPetSet   = ownedPets.Select(p => p.PetKey).ToHashSet(StringComparer.Ordinal);
            var ownedMountSet = ownedMounts.Select(p => p.PetKey).ToHashSet(StringComparer.Ordinal);
            var feedingMap    = allUserPets.ToDictionary(p => p.PetKey, p => p.FeedingPoints, StringComparer.Ordinal);

            string? activePetKey   = user.ActivePetKey;
            string? activeMountKey = user.ActiveMountKey;

            var petsGrid = _catalog.GetAnimalGroups()
                .Select(g => g with
                {
                    Slots = g.Slots.Select(s => s with
                    {
                        IsOwned       = ownedPetSet.Contains(s.PetKey) || ownedMountSet.Contains(s.PetKey),
                        IsMount       = ownedMountSet.Contains(s.PetKey),
                        FeedingPoints = feedingMap.GetValueOrDefault(s.PetKey, 0),
                        IsActivePet   = s.PetKey == activePetKey,
                        IsActiveMount = false
                    }).ToList().AsReadOnly()
                })
                .ToList();

            var mountsGrid = _catalog.GetAnimalGroupsForMounts()
                .Select(g => g with
                {
                    Slots = g.Slots.Select(s => s with
                    {
                        IsOwned       = ownedMountSet.Contains(s.PetKey),
                        IsMount       = ownedMountSet.Contains(s.PetKey),
                        FeedingPoints = feedingMap.GetValueOrDefault(s.PetKey, 0),
                        IsActivePet   = false,
                        IsActiveMount = s.PetKey == activeMountKey
                    }).ToList().AsReadOnly()
                })
                .ToList();

            return new StableViewModel
            {
                User               = user,
                EggsInInventory    = eggs,
                PotionsInInventory = potions,
                FoodInInventory    = food,
                OwnedPets          = ownedPets,
                OwnedMounts        = ownedMounts,
                PetsGrid           = petsGrid,
                MountsGrid         = mountsGrid,
                TotalPetsInCatalog   = _catalog.AllEntries.Count,
                TotalMountsInCatalog = _catalog.AllEntries.Count(e => e.CanBecomeMount)
            };
        }

        private static StableResult Fail(string error) => new() { Success = false, Error = error };

        private static void DeductInventory(UserInventoryItem inv)
        {
            inv.Quantity--;
        }
    }
}
