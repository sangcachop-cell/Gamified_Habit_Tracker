using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services.Implementations
{
    public class PetCatalogService : IPetCatalogService
    {
        // ── Static catalog data ──────────────────────────────────────────────

        public static readonly string[] DropAnimals =
        {
            "Wolf", "TigerCub", "PandaCub", "LionCub", "Fox",
            "FlyingPig", "Dragon", "Cactus", "BearCub"
        };

        public static readonly string[] QuestAnimals =
        {
            "Axolotl", "Alligator", "Alpaca", "Armadillo", "Badger", "Beetle",
            "Bunny", "Butterfly", "Cat", "Chameleon", "Cheetah", "Cow", "Crab",
            "Cuttlefish", "Deer", "Dog", "Dolphin", "Egg", "Falcon", "Ferret",
            "Frog", "Giraffe", "Gryphon", "GuineaPig", "Hedgehog", "Hippo",
            "Horse", "Kangaroo", "Monkey", "Nudibranch", "Octopus", "Otter",
            "Owl", "Parrot", "Peacock", "Penguin", "Platypus",
            "Pterodactyl", "Raccoon", "Rat", "Robot", "Rock", "Rooster",
            "Sabretooth", "Seahorse", "SeaSerpent", "Sheep", "Slime", "Sloth",
            "Snail", "Snake", "Spider", "Squirrel", "Treeling", "TRex",
            "Triceratops", "Turtle", "Unicorn", "Velociraptor", "Whale", "Yarn"
        };

        public static readonly string[] DropColors =
        {
            "Base", "White", "Desert", "Red", "Shade", "Skeleton",
            "Zombie", "CottonCandyPink", "CottonCandyBlue", "Golden"
        };

        // 51 premium colors — all seeded potion IDs 123–174 except TeaShop (wacky)
        public static readonly string[] PremiumColors =
        {
            "Amber", "Aquatic", "Aurora", "AutumnLeaf", "Balloon", "BirchBark",
            "BlackPearl", "Bronze", "Celestial", "Cupid", "Ember", "Fairy",
            "Floral", "Fluorite", "Frost", "Ghost", "Gingerbread", "Glass",
            "Glow", "Holly", "IcySnow", "Jade", "Koi", "Moonglow", "MossyStone",
            "Onyx", "Opal", "Peppermint", "PinkMarble", "PolkaDot", "Porcelain",
            "Rainbow", "RoseGold", "RoseQuartz", "RoyalPurple", "Ruby",
            "SandSculpture", "Shadow", "Shimmer", "Silver", "SolarSystem",
            "Spooky", "StainedGlass", "StarryNight", "Sunset", "Sunshine",
            "Thunderstorm", "Turquoise", "Vampire", "Watery"
        };

        // Wacky potions — pet images exist, no mount (except Windup)
        public static readonly string[] WackyColors =
        {
            "Veggie", "Dessert", "VirtualPet", "TeaShop",
            "Fungi", "Cryptid", "Alien", "Windup"
        };

        private static readonly HashSet<string> _wackyCanMount = new(StringComparer.Ordinal)
        {
            "Windup"
        };

        private static readonly HashSet<string> _dropAnimalSet =
            new(DropAnimals, StringComparer.Ordinal);

        private static readonly HashSet<string> _questAnimalSet =
            new(QuestAnimals, StringComparer.Ordinal);

        private static readonly HashSet<string> _dropColorSet =
            new(DropColors, StringComparer.Ordinal);

        private static readonly HashSet<string> _premiumColorSet =
            new(PremiumColors, StringComparer.Ordinal);

        private static readonly HashSet<string> _wackyColorSet =
            new(WackyColors, StringComparer.Ordinal);

        // Special pets — hardcoded entries not obtainable via egg+potion hatching
        // CanBecomeMount is set based on presence of local mount icon images
        private static readonly IReadOnlyList<PetCatalogEntry> SpecialPets = new List<PetCatalogEntry>
        {
            // Veteran pets (no mount icons exist for these)
            MakeSpecial("Wolf",     "Veteran",  canMount: false),
            MakeSpecial("Tiger",    "Veteran",  canMount: false),
            MakeSpecial("Lion",     "Veteran",  canMount: false),
            MakeSpecial("Bear",     "Veteran",  canMount: false),
            MakeSpecial("Fox",      "Veteran",  canMount: false),
            MakeSpecial("Dragon",   "Veteran",  canMount: false),
            MakeSpecial("Cactus",   "Veteran",  canMount: false),

            // Holiday — Thanksgiving
            MakeSpecial("Turkey",       "Base",         canMount: true),
            MakeSpecial("Turkey",       "Gilded",       canMount: true),

            // Holiday — Habitoween
            MakeSpecial("JackOLantern", "Base",         canMount: false),
            MakeSpecial("JackOLantern", "Glow",         canMount: false),
            MakeSpecial("JackOLantern", "Ghost",        canMount: false),
            MakeSpecial("JackOLantern", "RoyalPurple",  canMount: false),

            // World quest / time travel
            MakeSpecial("Mammoth",       "Base",    canMount: true),
            MakeSpecial("MagicalBee",    "Base",    canMount: true),
            MakeSpecial("Phoenix",       "Base",    canMount: true),
            MakeSpecial("MantisShrimp",  "Base",    canMount: true),
            MakeSpecial("Hippogriff",    "Hopeful", canMount: true),

            // Event / naming day
            MakeSpecial("Gryphon",    "RoyalPurple", canMount: true),
            MakeSpecial("Orca",       "Base",        canMount: true),
            MakeSpecial("BearCub",    "Polar",       canMount: true),

            // Rare / subscriber
            MakeSpecial("Dragon",     "Hydra",        canMount: false),
            MakeSpecial("Jackalope",  "RoyalPurple",  canMount: true),
        };

        // ── Instance members ─────────────────────────────────────────────────

        private readonly IReadOnlyList<PetCatalogEntry> _catalog;
        private readonly Dictionary<string, PetCatalogEntry> _index;

        public IReadOnlyList<PetCatalogEntry> AllEntries => _catalog;

        public PetCatalogService()
        {
            _catalog = BuildCatalog();
            _index = _catalog.ToDictionary(e => e.PetKey, StringComparer.Ordinal);
        }

        public bool IsValidHatch(string animalKey, string colorKey)
        {
            if (_questAnimalSet.Contains(animalKey))
                return _dropColorSet.Contains(colorKey);

            if (_dropAnimalSet.Contains(animalKey))
                return _dropColorSet.Contains(colorKey)
                    || _premiumColorSet.Contains(colorKey)
                    || _wackyColorSet.Contains(colorKey);

            return false;
        }

        public bool CanBecomeMount(string petKey)
        {
            return _index.TryGetValue(petKey, out var entry) && entry.CanBecomeMount;
        }

        public PetCatalogEntry? GetPetInfo(string petKey)
        {
            _index.TryGetValue(petKey, out var entry);
            return entry;
        }

        public IReadOnlyList<AnimalGroup> GetAnimalGroups()
        {
            var groups = new List<AnimalGroup>();

            // Drop animals first (9), then quest animals (70), then special
            foreach (var animal in DropAnimals)
            {
                var slots = _catalog
                    .Where(e => e.AnimalKey == animal && e.Category != PetCategory.Special)
                    .OrderBy(SlotOrder)
                    .Select(MakeSlot)
                    .ToList();
                if (slots.Count > 0)
                    groups.Add(new AnimalGroup(animal, FormatAnimalName(animal), PetCategory.Drop, slots.AsReadOnly()));
            }

            foreach (var animal in QuestAnimals)
            {
                var slots = _catalog
                    .Where(e => e.AnimalKey == animal && e.Category != PetCategory.Special)
                    .OrderBy(SlotOrder)
                    .Select(MakeSlot)
                    .ToList();
                if (slots.Count > 0)
                    groups.Add(new AnimalGroup(animal, FormatAnimalName(animal), PetCategory.Quest, slots.AsReadOnly()));
            }

            // Special animals (animals not in drop/quest lists)
            var specialByAnimal = SpecialPets
                .GroupBy(e => e.AnimalKey)
                .OrderBy(g => g.Key);

            foreach (var grp in specialByAnimal)
            {
                var slots = grp.OrderBy(e => e.ColorKey).Select(MakeSlot).ToList();
                groups.Add(new AnimalGroup(grp.Key, FormatAnimalName(grp.Key), PetCategory.Special, slots.AsReadOnly()));
            }

            return groups.AsReadOnly();
        }

        public IReadOnlyList<AnimalGroup> GetAnimalGroupsForMounts()
        {
            var groups = new List<AnimalGroup>();

            foreach (var animal in DropAnimals)
            {
                var slots = _catalog
                    .Where(e => e.AnimalKey == animal && e.CanBecomeMount && e.Category != PetCategory.Special)
                    .OrderBy(SlotOrder)
                    .Select(MakeSlot)
                    .ToList();
                if (slots.Count > 0)
                    groups.Add(new AnimalGroup(animal, FormatAnimalName(animal), PetCategory.Drop, slots.AsReadOnly()));
            }

            foreach (var animal in QuestAnimals)
            {
                var slots = _catalog
                    .Where(e => e.AnimalKey == animal && e.CanBecomeMount && e.Category != PetCategory.Special)
                    .OrderBy(SlotOrder)
                    .Select(MakeSlot)
                    .ToList();
                if (slots.Count > 0)
                    groups.Add(new AnimalGroup(animal, FormatAnimalName(animal), PetCategory.Quest, slots.AsReadOnly()));
            }

            var specialMountsByAnimal = SpecialPets
                .Where(e => e.CanBecomeMount)
                .GroupBy(e => e.AnimalKey)
                .OrderBy(g => g.Key);

            foreach (var grp in specialMountsByAnimal)
            {
                var slots = grp.OrderBy(e => e.ColorKey).Select(MakeSlot).ToList();
                groups.Add(new AnimalGroup(grp.Key, FormatAnimalName(grp.Key), PetCategory.Special, slots.AsReadOnly()));
            }

            return groups.AsReadOnly();
        }

        public IReadOnlyList<string> GetValidPotionColors(string animalKey)
        {
            if (_questAnimalSet.Contains(animalKey))
                return DropColors;

            if (_dropAnimalSet.Contains(animalKey))
                return DropColors.Concat(PremiumColors).Concat(WackyColors).ToArray();

            return Array.Empty<string>();
        }

        // ── Catalog construction ──────────────────────────────────────────────

        private static IReadOnlyList<PetCatalogEntry> BuildCatalog()
        {
            var entries = new List<PetCatalogEntry>(1400);

            foreach (var a in DropAnimals)
            {
                foreach (var c in DropColors)
                    entries.Add(Make(a, c, PetCategory.Drop, canMount: true));
                foreach (var c in PremiumColors)
                    entries.Add(Make(a, c, PetCategory.Premium, canMount: true));
                foreach (var c in WackyColors)
                    entries.Add(Make(a, c, PetCategory.Wacky, canMount: _wackyCanMount.Contains(c)));
            }

            foreach (var a in QuestAnimals)
            {
                foreach (var c in DropColors)
                    entries.Add(Make(a, c, PetCategory.Quest, canMount: true));
            }

            entries.AddRange(SpecialPets);

            return entries.AsReadOnly();
        }

        private static PetCatalogEntry Make(string animal, string color, PetCategory cat, bool canMount)
        {
            var key = $"{animal}-{color}";
            return new PetCatalogEntry(
                PetKey: key,
                AnimalKey: animal,
                ColorKey: color,
                Category: cat,
                CanBecomeMount: canMount,
                PetImagePath: $"/fe/stable/pets/Pet-{key}.png",
                MountIconPath: canMount ? $"/fe/stable/mounts/icon/Mount_Icon_{key}.png" : null);
        }

        private static PetCatalogEntry MakeSpecial(string animal, string color, bool canMount)
        {
            var key = $"{animal}-{color}";
            return new PetCatalogEntry(
                PetKey: key,
                AnimalKey: animal,
                ColorKey: color,
                Category: PetCategory.Special,
                CanBecomeMount: canMount,
                PetImagePath: $"/fe/stable/pets/Pet-{key}.png",
                MountIconPath: canMount ? $"/fe/stable/mounts/icon/Mount_Icon_{key}.png" : null);
        }

        // Slot for view rendering — owned/mount fields start false, populated by StableService
        private static PetSlotEntry MakeSlot(PetCatalogEntry e) => new(
            PetKey: e.PetKey,
            ColorKey: e.ColorKey,
            ColorDisplayName: FormatColorName(e.ColorKey),
            SlotCategory: e.Category,
            CanBecomeMount: e.CanBecomeMount,
            PetImagePath: e.PetImagePath,
            MountIconPath: e.MountIconPath,
            IsOwned: false,
            IsMount: false,
            FeedingPoints: 0,
            IsActivePet: false,
            IsActiveMount: false);

        // Order: drop colors first, then premium, then wacky
        private static int SlotOrder(PetCatalogEntry e) => e.Category switch
        {
            PetCategory.Drop => 0,
            PetCategory.Premium => 1,
            PetCategory.Wacky => 2,
            _ => 3
        };

        public static string FormatAnimalName(string key)
        {
            // Insert space before each uppercase letter after the first
            return System.Text.RegularExpressions.Regex.Replace(key, "([A-Z])", " $1").Trim();
        }

        private static string FormatColorName(string key)
        {
            return System.Text.RegularExpressions.Regex.Replace(key, "([A-Z])", " $1").Trim();
        }
    }
}
