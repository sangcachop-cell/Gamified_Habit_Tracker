namespace HabitTracker.Models.ViewModels
{
    public class EconomyResult
    {
        public int     XpGained       { get; set; }
        public double  GoldGained     { get; set; }
        public double  ManaGained     { get; set; }
        public bool    IsCrit         { get; set; }
        public bool    Died           { get; set; }
        public string? DroppedItemIcon { get; set; }
        public string? DroppedItemName { get; set; }
    }
}
