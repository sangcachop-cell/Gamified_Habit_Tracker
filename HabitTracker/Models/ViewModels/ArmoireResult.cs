namespace HabitTracker.Models.ViewModels
{
    public enum ArmoireRewardType { Gear, Food, Experience }

    public class ArmoireResult
    {
        public bool              Success      { get; init; }
        public string?           Error        { get; init; }
        public ArmoireRewardType RewardType   { get; init; }

        // Gear reward
        public string?           GearKey      { get; init; }
        public string?           GearName     { get; init; }
        public string?           GearImgPath  { get; init; }

        // Food reward
        public string?           FoodIcon     { get; init; }
        public string?           FoodName     { get; init; }

        // XP reward
        public int               XpGained     { get; init; }
    }
}
