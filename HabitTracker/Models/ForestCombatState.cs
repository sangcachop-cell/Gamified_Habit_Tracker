namespace HabitTracker.Models
{
    // Single-fight combat state for Forest encounters — stored in session as JSON
    public class ForestCombatState
    {
        public string MonsterTier   { get; set; } = "normal"; // "normal" | "rare"
        public string MonsterName   { get; set; } = "";
        public string MonsterIcon   { get; set; } = "🐺";
        public int MonsterMaxHP     { get; set; }
        public int MonsterCurrentHP { get; set; }
        public int MonsterAttack    { get; set; }
        public int MonsterArmor     { get; set; }

        public int MonsterSpeed     { get; set; }

        public int PlayerMaxHP      { get; set; }
        public int PlayerCurrentHP  { get; set; }
        public int PlayerAttack     { get; set; }
        public int PlayerArmor      { get; set; }
        public int PlayerSpeed      { get; set; }
        public double PlayerDamageReductionPct { get; set; }

        public int ConsecutiveDefends { get; set; } = 0;
        public List<string> Log     { get; set; } = new();
    }
}
