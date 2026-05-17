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
            new("cave",      "Hang Động", "🦇", "#5a4a3a", 18, 15, 12,  8,
                "Hang động tối tăm khoét sâu vào đá. Có gì đó đang cựa quậy bên trong."),
            new("warehouse", "Kho Hàng",  "🏚️", "#7a5c3a", 54, 58, 14, 10,
                "Kho lưu trữ bỏ hoang. Những kệ hàng xếp dọc các bức tường đang vỡ vụn."),
            new("lake",      "Hồ Sâu",   "🌊", "#2a7abf", 88, 22, 16, 10,
                "Mặt nước yên tĩnh bao phủ bởi bí ẩn. Sương mù lơ lửng thấp trên mặt hồ."),
        };

        // ── Spawn points ─────────────────────────────────────────────────────
        public record SpawnPoint(string Id, int X, int Y, int W, int H,
                                 string Label, string Color, string RequiredExtract);

        public static readonly SpawnPoint[] Spawns =
        {
            new("A", 10, 10, 4, 4, "Điểm Xuất Phát A", "#4a90d9", "Alpha"),
            new("B", 114, 114, 4, 4, "Điểm Xuất Phát B", "#d94a4a", "Beta"),
        };

        // ── Extract points ────────────────────────────────────────────────────
        public record ExtractPoint(string Id, int X, int Y, int W, int H,
                                   string Label, string Color);

        public static readonly ExtractPoint[] Extracts =
        {
            new("Alpha", 114, 10,  4, 4, "Điểm Rút Lui α", "#00b89c"),
            new("Beta",  10,  114, 4, 4, "Điểm Rút Lui β", "#00b89c"),
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

        // ── Monster definitions ───────────────────────────────────────────────
        public record ForestMonster(
            string Id, string Name, string Icon, string Description,
            int HP, int Attack, int Armor, int Speed, string Tier);

        // Tier labels for UI and loot weighting
        public const string TIER_COMMON   = "common";
        public const string TIER_UNCOMMON = "uncommon";
        public const string TIER_RARE     = "rare";
        public const string TIER_ELITE    = "elite";
        public const string TIER_BOSS     = "boss";
        public const string TIER_RAID     = "raid_boss";

        public static ForestMonster MakeMonster(string monsterId, int playerLevel)
        {
            int lvl = Math.Max(1, playerLevel);
            int S(double b, double g) => Math.Max(1, (int)(b + lvl * g));
            int Sp(double b, double g, int cap) => Math.Min(cap, (int)(b + Math.Sqrt(lvl) * g));

            return monsterId switch
            {
                "forest_scout" => new ForestMonster(
                    "forest_scout", "Thám Tử Rừng", "🐺",
                    "Thám tử sói linh hoạt. Nhanh và có thể đoán trước — mối nguy phổ biến nhất trong rừng mở.",
                    HP: S(70, 8), Attack: S(6, 1.2), Armor: S(0, 0.3), Speed: Sp(6, 3, 100),
                    Tier: TIER_COMMON),

                "skeleton_archer" => new ForestMonster(
                    "skeleton_archer", "Xạ Thủ Xương Khô", "💀",
                    "Vệ binh xác sống bắn những mũi tên phong ấn. Yếu khi cận chiến — nhưng nó không bao giờ để bạn tiếp cận.",
                    HP: S(55, 7), Attack: S(9, 1.8), Armor: S(0, 0.1), Speed: Sp(14, 5, 100),
                    Tier: TIER_COMMON),

                "swamp_toad" => new ForestMonster(
                    "swamp_toad", "Cóc Đầm Lầy", "🐸",
                    "Loài cóc khổng lồ nhiễm độc gần như miễn nhiễm mọi đòn tấn công. Chậm chạp — nhưng một vết cắn đã đủ để quật ngã một con trâu.",
                    HP: S(120, 16), Attack: S(5, 0.9), Armor: S(0, 1.2), Speed: Sp(3, 1, 40),
                    Tier: TIER_UNCOMMON),

                "forest_brute" => new ForestMonster(
                    "forest_brute", "Quái Vật Rừng Thẳm", "👹",
                    "Quái vật hình người khổng lồ ám ảnh rừng sâu. Nó không phục kích — nó lao thẳng vào.",
                    HP: S(130, 18), Attack: S(14, 2.8), Armor: S(2, 1.0), Speed: Sp(8, 4, 70),
                    Tier: TIER_RARE),

                "corrupted_treant" => new ForestMonster(
                    "corrupted_treant", "Cổ Thụ Tha Hóa", "🌳",
                    "Cây cổ thụ bị năng lượng tối vặn vẹo. Mỗi cánh tay là một cú đập nghiền nát. Rễ của nó hút máu mọi thứ nó giết.",
                    HP: S(180, 22), Attack: S(18, 3.0), Armor: S(8, 2.0), Speed: Sp(2, 1.5, 35),
                    Tier: TIER_RARE),

                "iron_golem" => new ForestMonster(
                    "iron_golem", "Người Máy Sắt", "⚙️",
                    "Cấu trúc sắt được linh hoạt hóa — chậm chạp, không ngừng nghỉ, gần như bất khả xâm phạm bằng tấn công thông thường. Nó không dừng lại cho đến khi một trong hai thành đống vụn.",
                    HP: S(200, 28), Attack: S(12, 2.2), Armor: S(18, 3.5), Speed: Sp(2, 0.5, 25),
                    Tier: TIER_ELITE),

                "shadow_stalker" => new ForestMonster(
                    "shadow_stalker", "Kẻ Rình Bóng", "👁️",
                    "Kẻ săn mồi tồn tại giữa các chiều không gian. Nhanh hơn suy nghĩ, đánh mạnh hơn bất kỳ sinh vật tự nhiên nào cùng kích cỡ, và chỉ trở nên hiện hữu đúng khoảnh khắc trước khi ra đòn.",
                    HP: S(80, 10), Attack: S(20, 3.5), Armor: S(0, 0.5), Speed: Sp(22, 7, 100),
                    Tier: TIER_ELITE),

                "lake_serpent" => new ForestMonster(
                    "lake_serpent", "Thuồng Luồng Hồ Sâu", "🐍",
                    "Loài ăn thịt đỉnh chuỗi cổ đại của vùng nước thẳm. Nó vọt ra từ dưới nước không báo trước và cuộn siết bằng lực nghiền nát.",
                    HP: S(140, 20), Attack: S(16, 3.0), Armor: S(5, 1.4), Speed: Sp(10, 4, 75),
                    Tier: TIER_RARE),

                "bone_colossus" => new ForestMonster(
                    "bone_colossus", "Khổng Lồ Xương", "🦴",
                    "Một trăm chiến binh hợp nhất thành một gã khổng lồ lảo đảo bởi hành động tuyệt vọng cuối cùng của một tay pháp sư hắc ám. Nó sừng sững. Nó bền vững. Nó nhớ từng cái chết trong số đó.",
                    HP: S(320, 45), Attack: S(35, 6.0), Armor: S(12, 3.0), Speed: Sp(6, 3, 55),
                    Tier: TIER_BOSS),

                "void_walker" => new ForestMonster(
                    "void_walker", "Lữ Hành Hư Không", "🌀",
                    "Sinh vật bước qua vết rách trong thực tại và không bao giờ hoàn toàn bước ra. Nó tồn tại một phần bên ngoài thời gian, khiến nó vừa kinh hoàng nhanh nhẹn vừa gần như không thể gây hại theo cách thông thường.",
                    HP: S(280, 40), Attack: S(30, 5.5), Armor: S(8, 2.5), Speed: Sp(18, 9, 100),
                    Tier: TIER_BOSS),

                "ancient_warden" => new ForestMonster(
                    "ancient_warden", "Người Gác Cổ Đại", "🌲",
                    "Người canh gác nguyên thủy của trái tim rừng — cổ xưa hơn vương quốc, cổ xưa hơn con đường, cổ xưa hơn tên gọi của sự vật. Những ai tìm kiếm Trái Tim Rừng phải đối mặt với điều này trước. Phần lớn không đối mặt được gì sau đó.",
                    HP: S(900, 120), Attack: S(70, 12.0), Armor: S(25, 6.0), Speed: Sp(10, 5, 75),
                    Tier: TIER_RAID),

                _ => MakeMonster("forest_scout", playerLevel)
            };
        }

        // Pick which monster spawns based on current location
        public static string GetMonsterId(Random rng, string? locationId)
        {
            double r = rng.NextDouble();
            return locationId switch
            {
                "cave"      => r < 0.07 ? "bone_colossus"
                             : r < 0.48 ? "shadow_stalker"
                             :            "iron_golem",
                "warehouse" => r < 0.06 ? "corrupted_treant"
                             : r < 0.52 ? "iron_golem"
                             :            "shadow_stalker",
                "lake"      => r < 0.08 ? "void_walker"
                             : r < 0.60 ? "lake_serpent"
                             :            "shadow_stalker",
                _           => r < 0.002 ? "ancient_warden"
                             : r < 0.09  ? "swamp_toad"
                             : r < 0.30  ? "skeleton_archer"
                             : r < 0.75  ? "forest_scout"
                             :             "forest_brute",
            };
        }

        // ── Loot tables (per monster) ─────────────────────────────────────────
        public static class LootTables
        {
            // [NoDrop%, Common%, Uncommon%, Rare%, Epic%, Legendary%, Mythic%]
            // Must sum to 100.
            private static readonly Dictionary<string, double[]> Chances = new()
            {
                ["forest_scout"]     = new[] { 15.00, 50.00, 20.00, 10.00,  3.50,  1.45,  0.05 },
                ["skeleton_archer"]  = new[] { 12.00, 42.00, 25.00, 13.00,  5.00,  2.92,  0.08 },
                ["swamp_toad"]       = new[] { 10.00, 33.00, 28.00, 16.00,  8.00,  4.85,  0.15 },
                ["forest_brute"]     = new[] { 10.00, 32.00, 25.00, 18.00, 10.00,  4.75,  0.25 },
                ["corrupted_treant"] = new[] {  8.00, 25.00, 22.00, 22.00, 14.00,  8.80,  0.20 },
                ["iron_golem"]       = new[] {  7.00, 20.00, 18.00, 22.00, 20.00, 12.50,  0.50 },
                ["shadow_stalker"]   = new[] {  8.00, 25.00, 22.00, 20.00, 15.00,  9.70,  0.30 },
                ["lake_serpent"]     = new[] {  9.00, 30.00, 25.00, 19.00, 11.00,  5.85,  0.15 },
                ["bone_colossus"]    = new[] { 10.00, 14.00, 16.00, 22.00, 22.00, 15.00,  1.00 },
                ["void_walker"]      = new[] { 14.00,  8.00, 13.00, 18.00, 24.00, 21.00,  2.00 },
                ["ancient_warden"]   = new[] {  2.00,  4.00,  7.00, 14.00, 24.00, 35.00, 14.00 },
            };

            // Per-monster item pools per rarity. Mythic pool = that monster's exclusive mythic.
            private static readonly Dictionary<string, Dictionary<string, string[]>> Pools = new()
            {
                ["forest_scout"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "wood", "stone", "wolf_pelt", "herb_bundle" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "beast_fang", "leather_strip", "rope_coil" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "iron_dagger", "shadow_cloak" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "runic_pendant" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "phoenix_feather" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "wolf_spirit_gem" },
                },
                ["skeleton_archer"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "bone_shard", "flint", "stone" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "crystal_shard", "health_vial", "bone_shard" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "hunter_quiver", "silver_sword" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "enchanted_blade" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "elder_staff" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "ancient_bowstring" },
                },
                ["swamp_toad"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "cave_mushroom", "herb_bundle", "tattered_cloth" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "toxic_gland", "stamina_draught", "herb_bundle" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "mana_flask", "ancient_tome" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "runic_pendant", "void_crystal" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "phoenix_feather" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "eye_of_the_bog" },
                },
                ["forest_brute"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "wolf_pelt", "wood", "leather_strip" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "beast_fang", "rope_coil", "leather_strip" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "shadow_cloak", "iron_dagger", "hunter_quiver" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "enchanted_blade", "dragon_scale" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "elder_staff", "phoenix_feather" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "brute_war_mask" },
                },
                ["corrupted_treant"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "corrupted_bark", "tattered_cloth", "herb_bundle" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "corrupted_bark", "rope_coil", "crystal_shard" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "ancient_tome", "shadow_cloak" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "dragon_scale", "void_crystal" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "phoenix_feather", "infinity_satchel" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "crown_of_echoes" },
                },
                ["iron_golem"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "iron_ore", "flint", "stone" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "iron_core", "crystal_shard", "iron_ore" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "chain_mail", "enchanted_lantern" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "plate_cuirass", "iron_core" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "wardens_shield" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "soul_of_iron" },
                },
                ["shadow_stalker"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "tattered_cloth", "stone" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "shadow_essence", "leather_strip" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "shadow_cloak", "mana_flask" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "void_crystal", "enchanted_blade" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "infinity_satchel", "phoenix_feather" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "shadow_veil" },
                },
                ["lake_serpent"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "stone", "serpent_scale", "herb_bundle" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "serpent_scale", "leather_strip", "rope_coil" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "silver_sword", "hunter_quiver", "chain_mail" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "enchanted_blade", "dragon_scale" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "phoenix_feather", "wardens_shield" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "serpent_sovereign_scale" },
                },
                ["bone_colossus"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "bone_shard", "flint", "stone" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "ancient_bone", "crystal_shard", "health_vial" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "chain_mail", "ancient_tome" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "plate_cuirass", "void_crystal", "dragon_scale" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "wardens_shield", "infinity_satchel" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "colossus_skull" },
                },
                ["void_walker"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "tattered_cloth", "cave_mushroom" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "void_tendril", "shadow_essence", "crystal_shard" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "shadow_cloak", "mana_flask", "ancient_tome" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "void_crystal", "void_tendril", "enchanted_blade" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "infinity_satchel", "elder_staff" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "void_heart" },
                },
                ["ancient_warden"] = new()
                {
                    [ItemCatalogue.Rarity.Common]    = new[] { "corrupted_bark", "herb_bundle" },
                    [ItemCatalogue.Rarity.Uncommon]  = new[] { "warden_heartwood", "corrupted_bark" },
                    [ItemCatalogue.Rarity.Rare]      = new[] { "warden_heartwood", "ancient_tome" },
                    [ItemCatalogue.Rarity.Epic]      = new[] { "dragon_scale", "plate_cuirass", "warden_heartwood" },
                    [ItemCatalogue.Rarity.Legendary] = new[] { "warden_heartwood", "wardens_shield", "infinity_satchel" },
                    [ItemCatalogue.Rarity.Mythic]    = new[] { "heart_of_the_forest" }, // only source
                },
            };

            // Roll rarity for a given monster. Returns null = no drop.
            public static string? RollRarity(Random rng, string monsterId)
            {
                if (!Chances.TryGetValue(monsterId, out var c))
                    c = Chances["forest_scout"];
                double r = rng.NextDouble() * 100, acc = 0;
                acc += c[0]; if (r < acc) return null;
                acc += c[1]; if (r < acc) return ItemCatalogue.Rarity.Common;
                acc += c[2]; if (r < acc) return ItemCatalogue.Rarity.Uncommon;
                acc += c[3]; if (r < acc) return ItemCatalogue.Rarity.Rare;
                acc += c[4]; if (r < acc) return ItemCatalogue.Rarity.Epic;
                acc += c[5]; if (r < acc) return ItemCatalogue.Rarity.Legendary;
                return ItemCatalogue.Rarity.Mythic;
            }

            // Pick item from the monster's pool for that rarity. Null if pool empty.
            public static string? RollItem(Random rng, string monsterId, string rarity)
            {
                if (!Pools.TryGetValue(monsterId, out var pools)) return null;
                if (!pools.TryGetValue(rarity, out var items) || items.Length == 0) return null;
                return items[rng.Next(items.Length)];
            }
        }

        // ── Event system ──────────────────────────────────────────────────────
        public const double BaseEventChance     = 0.05;  // 5% per open-forest cell
        public const double LocationEventChance = 0.25;  // 25% per location cell

        public static double GetEventChance(int x, int y) =>
            GetZone(x, y) != null ? LocationEventChance : BaseEventChance;

        // Legacy — kept for any remaining call sites; prefer GetMonsterId()
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
            new("cave", "Hang Động", 64, 64, Border: 2, EventChancePct: 0.40,
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
                    new(28,  2, 8, 2, "Lối Ra Hang — Bắc"),
                    new( 2, 28, 2, 8, "Lối Ra Hang — Tây"),
                    new(60, 28, 2, 8, "Lối Ra Hang — Đông"),
                },
                Chests: new ChestPos[] { new(18, 20), new(42, 38), new(10, 50) }),

            // Warehouse — worn wood/concrete floor
            new("warehouse", "Kho Hàng", 64, 64, Border: 2, EventChancePct: 0.35,
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
                    new(28,  2, 8, 2, "Lối Ra Kho — Bắc"),
                    new(28, 60, 8, 2, "Lối Ra Kho — Nam"),
                    new( 2, 28, 2, 8, "Lối Ra Kho — Tây"),
                },
                Chests: new ChestPos[] { new(10, 12), new(50, 14), new(32, 48) }),

            // Lake — shallow water, reeds, dark pools
            new("lake", "Hồ Sâu", 64, 64, Border: 2, EventChancePct: 0.45,
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
                    new(28,  2, 8, 2, "Lối Ra Hồ — Bắc"),
                    new(28, 60, 8, 2, "Lối Ra Hồ — Nam"),
                    new(60, 28, 2, 8, "Lối Ra Hồ — Đông"),
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
