namespace HabitTracker.Constants
{
    public static class FacilityCatalogue
    {
        public record UpgradeCost(int Wood, int Stone, TimeSpan Duration);

        public const int STORAGE_FACILITY_ID = 6;
        public const string STORAGE_FACILITY_NAME = "Storage Room";

        // [currentLevel - 1] → cost to upgrade to next level
        private static readonly UpgradeCost[] StorageCosts =
        {
            new(30,  0,   TimeSpan.FromMinutes(5)),   // 1→2
            new(60,  20,  TimeSpan.FromMinutes(15)),  // 2→3
            new(100, 50,  TimeSpan.FromMinutes(30)),  // 3→4
            new(150, 100, TimeSpan.FromHours(1)),     // 4→5
        };

        private static readonly UpgradeCost[] DefaultCosts =
        {
            new(20, 0,  TimeSpan.FromMinutes(5)),    // 1→2
            new(40, 15, TimeSpan.FromMinutes(15)),   // 2→3
            new(80, 40, TimeSpan.FromMinutes(30)),   // 3→4
            new(120, 80, TimeSpan.FromHours(1)),     // 4→5
        };

        public static UpgradeCost? GetCost(int facilityId, int currentLevel)
        {
            if (currentLevel >= 5) return null;
            int idx = currentLevel - 1;
            var costs = facilityId == STORAGE_FACILITY_ID ? StorageCosts : DefaultCosts;
            return idx >= 0 && idx < costs.Length ? costs[idx] : null;
        }
    }
}
