namespace HabitTracker.Models.ViewModels
{
    public class MarketViewModel
    {
        public User                                                          User         { get; set; } = null!;
        /// <summary>GearClass → Slot → items (IsArmoire excluded from regular shop)</summary>
        public Dictionary<string, Dictionary<string, List<GearItem>>>        ByClassAndSlot { get; set; } = new();
        public HashSet<string>                                               OwnedKeys    { get; set; } = new();
        public List<GearItem>                                                ArmoireItems { get; set; } = new();
    }
}
