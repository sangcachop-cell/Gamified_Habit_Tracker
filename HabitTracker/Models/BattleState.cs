namespace HabitTracker.Models
{
    // Stored in session as JSON — not a DB entity
    public class BattleState
    {
        // Player snapshot (taken at battle start from User computed props)
        public string PlayerName      { get; set; } = "";
        public int    PlayerCurrentHP { get; set; }
        public int    PlayerMaxHP     { get; set; }
        public int    PlayerAttack    { get; set; }
        public int    PlayerArmor     { get; set; }
        public int    PlayerSpeed     { get; set; }

        // Monster
        public string MonsterName        { get; set; } = "";
        public string MonsterIcon        { get; set; } = "👾";
        public string MonsterDescription { get; set; } = "";
        public int    MonsterCurrentHP   { get; set; }
        public int    MonsterMaxHP       { get; set; }
        public int    MonsterAttack      { get; set; }
        public int    MonsterArmor       { get; set; }
        public int    MonsterSpeed       { get; set; }

        // Battle meta
        public string       Status             { get; set; } = "InProgress"; // InProgress | WaveCleared | Victory | Defeat
        public int          Turn               { get; set; } = 1;
        public int          WaveNumber         { get; set; } = 1;
        public int          TotalWaves         { get; set; } = 5;
        public int          PlayerLevel        { get; set; }     // snapshotted at battle start — used for wave scaling
        public int          XPReward           { get; set; }     // XP for the current wave
        public int          TotalXPReward      { get; set; }     // XP accumulated across cleared waves
        public int          ConsecutiveDefends       { get; set; } = 0;
        public double       PlayerDamageReductionPct { get; set; } = 0;
        public List<string> Log                { get; set; } = new();
    }
}
