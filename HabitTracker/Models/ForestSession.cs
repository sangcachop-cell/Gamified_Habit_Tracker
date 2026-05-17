namespace HabitTracker.Models
{
    // Stored in session as JSON — not a DB entity
    public class ForestSession
    {
        public int  PlayerX             { get; set; }
        public int  PlayerY             { get; set; }
        public string SpawnId           { get; set; } = "A";     // "A" | "B"
        public string RequiredExtractId { get; set; } = "Alpha"; // "Alpha" | "Beta"
        public int  MovesSpent          { get; set; } = 0;
        public bool IsActive            { get; set; } = true;
        public DateTime StartedAt       { get; set; } = DateTime.UtcNow;

        // Combat state
        public int  PlayerCurrentHP     { get; set; } = -1;      // -1 = use full HP on first combat
        public bool PendingCombat       { get; set; } = false;
        public string PendingMonsterTier { get; set; } = "normal"; // "normal" | "rare"

        // Loot accumulates until extract or death — placeholder log messages
        public List<string> AccumulatedLoot { get; set; } = new();

        // Real items: monster body holds drops awaiting pickup; pouch holds carried items
        public List<LootItem> MonsterBody { get; set; } = new();
        public List<LootItem> Pouch       { get; set; } = new();

        // Remaining path cells [[x,y],...] after a mid-path combat — offered to player on return to map
        public List<int[]>? PendingPath { get; set; } = null;

        // Steps walked since last combat trigger — pity system gates next encounter
        // Initialized to the minimum so there's no forced grace period at run start
        public int StepsSinceLastCombat { get; set; } = 10;

        // ── Interior sub-map state ────────────────────────────────────────────
        // null = player is on the main 128×128 world map
        // "cave"|"warehouse"|"lake" = player is inside a location's 64×64 interior
        public string? CurrentMapId  { get; set; } = null;
        public int     WorldReturnX  { get; set; } = 0;   // world position to restore on ExitLocation
        public int     WorldReturnY  { get; set; } = 0;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsInInterior => CurrentMapId != null;
    }

    public class LootItem
    {
        public string ItemId    { get; set; } = "";
        public int    GridX     { get; set; }
        public int    GridY     { get; set; }
        public bool   Rotated   { get; set; }
        public string Container { get; set; } = ""; // set when item is in Pouch
    }
}
