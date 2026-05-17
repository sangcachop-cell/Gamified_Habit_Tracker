namespace HabitTracker.Constants
{
    public static class ForestMap
    {
        public const int WIDTH  = 128;
        public const int HEIGHT = 128;
        public const int CELL_PX = 6;          // pixels per cell on canvas
        public const int WATER_BORDER = 8;     // outer tiles = water (impassable)

        // ── Zones ────────────────────────────────────────────────────────────
        public record Zone(string Id, string Name, string Icon, string Color,
                           int X, int Y, int W, int H, string Description);

        public static readonly Zone[] Locations =
        {
            new("cave",      "Cave",      "🦇", "#5a4a3a", 18, 15, 12,  8,
                "A dark cavern carved into the rock. Something stirs inside."),
            new("warehouse", "Warehouse", "🏚️", "#7a5c3a", 54, 58, 14, 10,
                "An abandoned storage facility. Shelves line the crumbling walls."),
            new("lake",      "Lake",      "🌊", "#2a7abf", 88, 22, 16, 10,
                "A still body of water. Mist hangs low over the surface."),
        };

        // ── Spawn points ─────────────────────────────────────────────────────
        public record SpawnPoint(string Id, int X, int Y, int W, int H,
                                 string Label, string Color, string RequiredExtract);

        public static readonly SpawnPoint[] Spawns =
        {
            new("A", 10, 10, 4, 4, "Spawn A", "#4a90d9", "Alpha"),    // NW → must exit NE
            new("B", 114, 114, 4, 4, "Spawn B", "#d94a4a", "Beta"),   // SE → must exit SW
        };

        // ── Extract points ────────────────────────────────────────────────────
        public record ExtractPoint(string Id, int X, int Y, int W, int H,
                                   string Label, string Color);

        public static readonly ExtractPoint[] Extracts =
        {
            new("Alpha", 114, 10,  4, 4, "Extract α", "#00b89c"),  // NE
            new("Beta",  10,  114, 4, 4, "Extract β", "#00b89c"),  // SW
        };

        // ── Helpers ───────────────────────────────────────────────────────────

        public static bool IsWater(int x, int y) =>
            x < WATER_BORDER || y < WATER_BORDER ||
            x >= WIDTH - WATER_BORDER || y >= HEIGHT - WATER_BORDER;

        public static Zone? GetZone(int x, int y) =>
            Array.Find(Locations, z => x >= z.X && x < z.X + z.W &&
                                       y >= z.Y && y < z.Y + z.H);

        public static SpawnPoint? GetSpawn(int x, int y) =>
            Array.Find(Spawns, s => x >= s.X && x < s.X + s.W &&
                                    y >= s.Y && y < s.Y + s.H);

        public static ExtractPoint? GetExtract(int x, int y) =>
            Array.Find(Extracts, e => x >= e.X && x < e.X + e.W &&
                                      y >= e.Y && y < e.Y + e.H);

        public static int Distance(int x1, int y1, int x2, int y2) =>
            (int)Math.Round(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));

        public static SpawnPoint RandomSpawn() =>
            Spawns[new Random().Next(Spawns.Length)];

        public static (int cx, int cy) SpawnCenter(SpawnPoint s) =>
            (s.X + s.W / 2, s.Y + s.H / 2);

        // ── Monster factory ───────────────────────────────────────────────────
        public record ForestMonster(string Name, string Icon, int HP, int Attack, int Armor, int Speed, string Tier);

        // Normal = wave-1 (mult 1.0), Rare = wave-3 (mult 1.6) — stats scale with player level
        public static ForestMonster MakeMonster(string tier, int playerLevel)
        {
            int    lvl  = Math.Max(1, playerLevel);
            double mult = tier == "rare" ? 1.6 : 1.0;
            int    spd  = Math.Min(100, (int)(5 + Math.Sqrt(lvl) * 3) + (tier == "rare" ? 4 : 0));
            return new ForestMonster(
                tier == "rare" ? "Forest Brute" : "Forest Scout",
                tier == "rare" ? "👹" : "🐺",
                Math.Max(20, (int)((80 + lvl * 5) * mult)),
                Math.Max(3,  (int)((5  + lvl)     * mult)),
                Math.Max(0,  (int)((lvl * 0.5)    * mult)),
                spd,
                tier
            );
        }

        // ── Loot tables ───────────────────────────────────────────────────────
        public static class LootTables
        {
            // Forest loot pool per rarity — add item IDs here as new items are created
            public static readonly Dictionary<string, string[]> Forest = new()
            {
                [ItemCatalogue.Rarity.Common]    = new[] { "wood", "stone" },
                [ItemCatalogue.Rarity.Uncommon]  = new string[] { },
                [ItemCatalogue.Rarity.Rare]      = new string[] { },
                [ItemCatalogue.Rarity.Epic]      = new string[] { },
                [ItemCatalogue.Rarity.Legendary] = new string[] { },
                [ItemCatalogue.Rarity.Mythic]    = new string[] { },
            };

            // Returns rarity string or null (no drop). Rare monsters shift weight up.
            public static string? RollRarity(Random rng, string monsterTier)
            {
                double r = rng.NextDouble() * 100;
                if (monsterTier == "rare")
                {
                    if (r < 0.3)  return ItemCatalogue.Rarity.Mythic;
                    if (r < 2.3)  return ItemCatalogue.Rarity.Legendary;
                    if (r < 7.3)  return ItemCatalogue.Rarity.Epic;
                    if (r < 22.3) return ItemCatalogue.Rarity.Rare;
                    if (r < 47.3) return ItemCatalogue.Rarity.Uncommon;
                    if (r < 82.3) return ItemCatalogue.Rarity.Common;
                    return null; // 17.7% no drop
                }
                else
                {
                    if (r < 0.2)  return ItemCatalogue.Rarity.Mythic;
                    if (r < 1.5)  return ItemCatalogue.Rarity.Legendary;
                    if (r < 5.0)  return ItemCatalogue.Rarity.Epic;
                    if (r < 15.0) return ItemCatalogue.Rarity.Rare;
                    if (r < 35.0) return ItemCatalogue.Rarity.Uncommon;
                    if (r < 85.0) return ItemCatalogue.Rarity.Common;
                    return null; // 15% no drop
                }
            }

            // Equal chance for each item in rarity pool; null if pool empty
            public static string? RollItem(Random rng, string rarity)
            {
                if (!Forest.TryGetValue(rarity, out var items) || items.Length == 0) return null;
                return items[rng.Next(items.Length)];
            }
        }

        // ── Event system ──────────────────────────────────────────────────────
        public const double BaseEventChance     = 0.05;  // 5% per open-forest cell
        public const double LocationEventChance = 0.25;  // 25% per location cell

        public static double GetEventChance(int x, int y) =>
            GetZone(x, y) != null ? LocationEventChance : BaseEventChance;

        // "common" in open forest, "rare" inside named locations
        public static string GetEventTier(int x, int y) =>
            GetZone(x, y) != null ? "rare" : "common";

        // ── Location interior sub-maps (64×64) ───────────────────────────────
        public record ExitZone(int X, int Y, int W, int H, string Label);
        public record ChestPos(int X, int Y);

        public record LocationInterior(
            string     Id,
            string     Name,
            int        Width,           // 64
            int        Height,          // 64
            int        Border,          // impassable edge width (cells)
            double     EventChancePct,  // per-step combat probability
            string[]   TerrainColors,   // 7 hex strings; index = (x*13+y*7)%7
            ExitZone[] Exits,
            ChestPos[] Chests
        );

        public static readonly LocationInterior[] Interiors =
        {
            // Cave — dark stone, wet rock, moss patches
            new("cave", "Cave", 64, 64, Border: 2, EventChancePct: 0.40,
                TerrainColors: new[] {
                    "#2a2a2a", // v=0 dark stone
                    "#1e1e1e", // v=1 black rock
                    "#303030", // v=2 mid grey stone
                    "#383228", // v=3 mossy dark
                    "#2e2e2e", // v=4 stone
                    "#252520", // v=5 dark with hint of green
                    "#3a3530", // v=6 warmer stone
                },
                // Exits at first/last passable row/col (border=2 → passable starts at 2, ends at 61)
                Exits: new ExitZone[] {
                    new(28,  2, 8, 2, "Cave Exit — North"),  // y=2..3
                    new( 2, 28, 2, 8, "Cave Exit — West"),   // x=2..3
                    new(60, 28, 2, 8, "Cave Exit — East"),   // x=60..61
                },
                Chests: new ChestPos[] { new(18, 20), new(42, 38), new(10, 50) }),

            // Warehouse — worn wood/concrete floor
            new("warehouse", "Warehouse", 64, 64, Border: 2, EventChancePct: 0.35,
                TerrainColors: new[] {
                    "#5a4a3a", // v=0 worn wood floor
                    "#4a3a2a", // v=1 dark plank
                    "#6b5a40", // v=2 lighter plank
                    "#4e4238", // v=3 stained concrete
                    "#5c5040", // v=4 concrete mid
                    "#625040", // v=5 dusty floor
                    "#4a3e30", // v=6 very dark plank
                },
                Exits: new ExitZone[] {
                    new(28,  2, 8, 2, "Warehouse Exit — North"),  // y=2..3
                    new(28, 60, 8, 2, "Warehouse Exit — South"),  // y=60..61
                    new( 2, 28, 2, 8, "Warehouse Exit — West"),   // x=2..3
                },
                Chests: new ChestPos[] { new(10, 12), new(50, 14), new(32, 48) }),

            // Lake — shallow water, reeds, dark pools
            new("lake", "Lake", 64, 64, Border: 2, EventChancePct: 0.45,
                TerrainColors: new[] {
                    "#2a4a5a", // v=0 shallow water
                    "#1e3a50", // v=1 deeper water
                    "#3a5a6a", // v=2 pool edge
                    "#4a6a50", // v=3 reeds/moss
                    "#2a5060", // v=4 water mid
                    "#1c3848", // v=5 dark deep
                    "#3a5a48", // v=6 wet mud/reed
                },
                Exits: new ExitZone[] {
                    new(28,  2, 8, 2, "Lake Exit — North"),  // y=2..3
                    new(28, 60, 8, 2, "Lake Exit — South"),  // y=60..61
                    new(60, 28, 2, 8, "Lake Exit — East"),   // x=60..61
                },
                Chests: new ChestPos[] { new(15, 30), new(48, 18), new(30, 52) }),
        };

        public static LocationInterior? GetInterior(string id) =>
            Array.Find(Interiors, i => i.Id == id);

        public static bool IsInteriorWater(LocationInterior map, int x, int y) =>
            x < map.Border || y < map.Border ||
            x >= map.Width  - map.Border ||
            y >= map.Height - map.Border;

        public static (int x, int y) InteriorSpawn(LocationInterior map) =>
            (map.Width / 2, map.Height / 2);

        public static ExitZone? GetExitZone(LocationInterior map, int x, int y) =>
            Array.Find(map.Exits, ez => x >= ez.X && x < ez.X + ez.W &&
                                        y >= ez.Y && y < ez.Y + ez.H);
    }
}
