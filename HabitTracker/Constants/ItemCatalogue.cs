namespace HabitTracker.Constants
{
    public static class ItemCatalogue
    {
        public record ItemDef(
            string Name,
            string Icon,
            string Description,
            int    Width,       // unrotated width  (columns)
            int    Height,      // unrotated height (rows)
            string Category,
            string TileColor    // CSS hex for grid tile background
        );

        public static readonly Dictionary<string, ItemDef> Items = new()
        {
            ["bread"] = new ItemDef(
                Name:        "Bread",
                Icon:        "🍞",
                Description: "A hearty loaf of bread. Restores 30 HP when consumed during battle.",
                Width:       1,
                Height:      1,
                Category:    "Food",
                TileColor:   "#7c4f1e"
            ),
            ["water_bottle"] = new ItemDef(
                Name:        "Water Bottle",
                Icon:        "🧴",
                Description: "A full bottle of water. Allows you to flee from any battle.",
                Width:       2,
                Height:      1,
                Category:    "Utility",
                TileColor:   "#005f6b"
            ),
        };

        // Items with equal W and H can't be meaningfully rotated
        public static bool CanRotate(string itemId) =>
            Items.TryGetValue(itemId, out var d) && d.Width != d.Height;

        // Container sizes
        public static (int Cols, int Rows) ContainerSize(string containerType) =>
            containerType == "Backpack" ? (4, 2) : (5, 4);

        public const int CELL_PX = 64;          // pixel size of one grid cell
        public const string STORAGE  = "Storage";
        public const string BACKPACK = "Backpack";
    }
}
