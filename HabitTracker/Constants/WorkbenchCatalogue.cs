namespace HabitTracker.Constants
{
    public static class WorkbenchCatalogue
    {
        public const int WORKBENCH_FACILITY_ID = 7;

        public record Recipe(
            string Id,
            string Name,
            string InputItemId,
            int    InputQty,
            string OutputLabel,   // display string
            string OutputField,   // "Wood" | "Stone" — which User field to increment
            int    OutputQty,
            TimeSpan CraftTime,
            int    MinLevel       // workbench level required
        );

        public static readonly Recipe[] Recipes =
        {
            new("wood_to_material",
                "Process Wood",
                InputItemId:  "wood",
                InputQty:     1,
                OutputLabel:  "+10 Wood Material",
                OutputField:  "Wood",
                OutputQty:    10,
                CraftTime:    TimeSpan.FromHours(2),
                MinLevel:     1),

            new("stone_to_material",
                "Process Stone",
                InputItemId:  "stone",
                InputQty:     1,
                OutputLabel:  "+10 Stone Material",
                OutputField:  "Stone",
                OutputQty:    10,
                CraftTime:    TimeSpan.FromHours(2),
                MinLevel:     1),
        };

        // Slots available at each workbench level
        public static int SlotsForLevel(int level) => 1 + level; // Lv1=2, Lv2=3, ...

        // Recipes unlocked at given level
        public static Recipe[] RecipesForLevel(int level) =>
            Recipes.Where(r => r.MinLevel <= level).ToArray();

        public static Recipe? GetRecipe(string id) =>
            Recipes.FirstOrDefault(r => r.Id == id);
    }
}
