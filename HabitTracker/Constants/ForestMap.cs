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

        // ── Event system ──────────────────────────────────────────────────────
        public const double BaseEventChance     = 0.05;  // 5% per open-forest cell
        public const double LocationEventChance = 0.25;  // 25% per location cell

        public static double GetEventChance(int x, int y) =>
            GetZone(x, y) != null ? LocationEventChance : BaseEventChance;

        // "common" in open forest, "rare" inside named locations
        public static string GetEventTier(int x, int y) =>
            GetZone(x, y) != null ? "rare" : "common";
    }
}
