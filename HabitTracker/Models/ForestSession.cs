namespace HabitTracker.Models
{
    // Stored in session as JSON — not a DB entity
    public class ForestSession
    {
        public int  PlayerX         { get; set; }
        public int  PlayerY         { get; set; }
        public string SpawnId       { get; set; } = "A";   // "A" | "B"
        public string RequiredExtractId { get; set; } = "Alpha"; // "Alpha" | "Beta"
        public int  MovesSpent      { get; set; } = 0;
        public bool IsActive        { get; set; } = true;
        public DateTime StartedAt   { get; set; } = DateTime.UtcNow;
    }
}
