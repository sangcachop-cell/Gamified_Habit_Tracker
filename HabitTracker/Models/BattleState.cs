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
        public string       Status    { get; set; } = "InProgress"; // InProgress | Victory | Defeat | Fled
        public int          Turn      { get; set; } = 1;
        public int          XPReward  { get; set; }
        public List<string> Log       { get; set; } = new();
    }
}
