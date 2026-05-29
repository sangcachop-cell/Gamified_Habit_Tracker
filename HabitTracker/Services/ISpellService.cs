namespace HabitTracker.Services
{
    /// <summary>
    /// Static metadata for a castable class spell.
    /// </summary>
    public record SpellDefinition(
        string Key,           // Habitica key, e.g. "defensiveStance"
        string Name,          // Display name
        string Description,   // Tooltip / description
        string ClassName,     // "warrior" | "mage" | "rogue" | "healer"
        int    ManaCost,
        int    MinLevel,
        bool   TargetsTask,   // true = requires a taskId; false = self-cast
        bool   TargetsAllTasks = false  // true = applies to all tasks (brightness)
    );

    public interface ISpellService
    {
        /// <summary>Returns all spells for the given class, ordered by MinLevel.</summary>
        IReadOnlyList<SpellDefinition> GetSpellsForClass(string className);

        /// <summary>
        /// Returns the full spell catalog (all classes).
        /// </summary>
        IReadOnlyList<SpellDefinition> GetAllSpells();

        /// <summary>
        /// Casts a spell.
        /// - For task-targeting spells (TargetsTask=true), taskId must be provided.
        /// - For self/all-tasks spells, taskId is ignored.
        /// Handles mana deduction, all stat/economy mutations, and SaveChangesAsync.
        /// Returns (true, null, data) on success; (false, errorMessage, null) on failure.
        /// data keys vary by spell (xpGained, goldGained, hpGained, buffCONApplied, etc.)
        /// </summary>
        Task<(bool Success, string? Error, Dictionary<string, object>? Data)>
            CastAsync(int userId, string spellKey, int? taskId = null);
    }
}
