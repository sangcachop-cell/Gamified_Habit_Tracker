namespace HabitTracker.Models.ViewModels
{
    public class ScoreResultViewModel
    {
        public bool Success       { get; set; }
        public int  XpGained      { get; set; }
        public int  NewXP         { get; set; }
        public int  NewLevel      { get; set; }
        public bool LeveledUp     { get; set; }
        public int  NewStreak     { get; set; }
        public double NewValue    { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string> NewBadges { get; set; } = new();

        // Habit counters
        public int CounterUp   { get; set; }
        public int CounterDown { get; set; }

        // Daily/Todo completion state
        public bool IsCompleted { get; set; }

        // Economy (Phase 2)
        public double  GoldGained      { get; set; }
        public double  NewGold         { get; set; }
        public double  NewHP           { get; set; }
        public double  NewMana         { get; set; }
        public bool    IsCrit          { get; set; }
        public bool    Died            { get; set; }
        public bool    NotEnoughGold   { get; set; }
        public string? DroppedItemIcon { get; set; }
        public string? DroppedItemName { get; set; }

        // Phase 3 — Stat Points
        public int StatPointsGained { get; set; }
        public int NewStatPoints    { get; set; }
    }
}
