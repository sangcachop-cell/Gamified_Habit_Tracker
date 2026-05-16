namespace HabitTracker.Constants
{
    public static class FacilityCatalogue
    {
        public record UpgradeCost(int Wood, int Stone, TimeSpan Duration);

        public const int STORAGE_FACILITY_ID   = 6;
        public const string STORAGE_FACILITY_NAME = "Storage Room";
        public const int WORKBENCH_FACILITY_ID  = 7;
        public const string WORKBENCH_FACILITY_NAME = "Workbench";

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

        private static readonly UpgradeCost[] WorkbenchCosts =
        {
            new(50,  20,  TimeSpan.FromMinutes(10)), // 1→2
            new(100, 50,  TimeSpan.FromMinutes(30)), // 2→3
            new(200, 100, TimeSpan.FromHours(1)),    // 3→4
            new(350, 200, TimeSpan.FromHours(2)),    // 4→5
        };

        public static UpgradeCost? GetCost(int facilityId, int currentLevel)
        {
            if (currentLevel >= 5) return null;
            int idx = currentLevel - 1;
            var costs = facilityId == STORAGE_FACILITY_ID  ? StorageCosts
                      : facilityId == WORKBENCH_FACILITY_ID ? WorkbenchCosts
                      : DefaultCosts;
            return idx >= 0 && idx < costs.Length ? costs[idx] : null;
        }
    }
}
