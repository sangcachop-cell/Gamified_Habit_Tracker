namespace HabitTracker.Constants
{
    public static class ItemCatalogue
    {
        public record ItemDef(
            string Name,
            string Icon,
            string Description,
            int    Width,
            int    Height,
            string Category,
            string TileColor
        );

        // Equipment item definition — for items that occupy a slot on the character
        public record EquipDef(
            string Name,
            string Icon,
            string Description,
            string Slot,             // "BackpackSlot" | "ArmorSlot" | "RigSlot"
            double DamageReduction,  // % incoming damage reduction (armor)
            int?   ContainerCols,    // grid container size (backpack/rig)
            int?   ContainerRows,
            string? ContainerType   // e.g. "EquippedBackpack" | "EquippedRig" | null
        );

        public static readonly Dictionary<string, ItemDef> Items = new()
        {
            ["bread"] = new ItemDef(
                Name: "Bread", Icon: "🍞",
                Description: "A hearty loaf of bread. Restores 30 HP when consumed during battle.",
                Width: 1, Height: 1, Category: "Food", TileColor: "#7c4f1e"),

            ["water_bottle"] = new ItemDef(
                Name: "Water Bottle", Icon: "🧴",
                Description: "A full bottle of water. Allows you to flee from any battle.",
                Width: 2, Height: 1, Category: "Utility", TileColor: "#005f6b"),

            ["simple_backpack"] = new ItemDef(
                Name: "Simple Backpack", Icon: "🎒",
                Description: "A sturdy pack. Equip to unlock a 4×4 storage grid.",
                Width: 2, Height: 2, Category: "Equipment", TileColor: "#3d2b6b"),

            ["simple_armor"] = new ItemDef(
                Name: "Simple Armor", Icon: "🛡️",
                Description: "Light plating. Equip to reduce incoming damage by 5%.",
                Width: 1, Height: 2, Category: "Equipment", TileColor: "#2b4a6b"),

            ["simple_rig"] = new ItemDef(
                Name: "Simple Rig", Icon: "🦺",
                Description: "A tactical rig. Equip to unlock a 4×2 quick-access grid.",
                Width: 2, Height: 1, Category: "Equipment", TileColor: "#6b3a2b"),
        };

        public static readonly Dictionary<string, EquipDef> Equipment = new()
        {
            ["simple_backpack"] = new EquipDef(
                Name: "Simple Backpack", Icon: "🎒",
                Description: "Unlocks a 4×4 storage grid.",
                Slot: SLOT_BACKPACK, DamageReduction: 0,
                ContainerCols: 4, ContainerRows: 4, ContainerType: EQUIPPED_BACKPACK),

            ["simple_armor"] = new EquipDef(
                Name: "Simple Armor", Icon: "🛡️",
                Description: "Reduces incoming damage by 5%.",
                Slot: SLOT_ARMOR, DamageReduction: 5.0,
                ContainerCols: null, ContainerRows: null, ContainerType: null),

            ["simple_rig"] = new EquipDef(
                Name: "Simple Rig", Icon: "🦺",
                Description: "Unlocks a 4×2 quick-access grid (4 slots of 2×1).",
                Slot: SLOT_RIG, DamageReduction: 0,
                ContainerCols: 4, ContainerRows: 2, ContainerType: EQUIPPED_RIG),
        };

        public static bool CanRotate(string itemId) =>
            Items.TryGetValue(itemId, out var d) && d.Width != d.Height;

        public static bool IsEquippable(string itemId) => Equipment.ContainsKey(itemId);

        public static (int Cols, int Rows) ContainerSize(string containerType) => containerType switch {
            "Backpack"         => (4, 1),
            "EquippedBackpack" => (4, 4),
            "EquippedRig"      => (4, 2),
            _                  => (10, 10)
        };

        // Dynamic storage size based on Storage Room facility level
        public static (int Cols, int Rows) StorageSizeForLevel(int level) =>
            (10, 10 + (level - 1) * 3);

        // Exact item size required to enter a container (null = no restriction)
        public static (int W, int H)? SlotConstraint(string containerType) => containerType switch {
            "Backpack"    => (1, 1),
            "EquippedRig" => (1, 2),
            _             => null
        };

        public const int CELL_PX = 64;
        public const string STORAGE          = "Storage";
        public const string BACKPACK         = "Backpack";
        public const string HIDEOUT_STORAGE  = "HideoutStorage";
        public const string EQUIPPED_BACKPACK = "EquippedBackpack";
        public const string EQUIPPED_RIG     = "EquippedRig";
        public const string SLOT_BACKPACK    = "BackpackSlot";
        public const string SLOT_ARMOR       = "ArmorSlot";
        public const string SLOT_RIG         = "RigSlot";
    }
}
