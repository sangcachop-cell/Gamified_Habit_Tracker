namespace HabitTracker.Models.ViewModels
{
    /// <summary>
    /// Data passed to the _StatCard partial view.
    /// </summary>
    public record StatCardModel(
        string Stat,          // "STR" | "CON" | "INT" | "PER"
        string Icon,          // emoji
        string Label,         // full name
        string Tooltip,       // what this stat does
        int    Base,          // user.STR etc.
        int    GearBonus,
        int    ClassBonus,
        int    LevelBonus,
        int    Buff,
        int    Total,
        bool   CanAllocate    // StatPoints > 0
    );
}
