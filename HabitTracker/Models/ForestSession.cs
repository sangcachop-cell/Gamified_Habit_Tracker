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
    }

    public class LootItem
    {
        public string ItemId  { get; set; } = "";
        public int    GridX   { get; set; }
        public int    GridY   { get; set; }
        public bool   Rotated { get; set; }
    }
}
