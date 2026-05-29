using HabitTracker.Models;

namespace HabitTracker.Services
{
    /// <summary>
    /// Breakdown of a user's effective stats after gear, class bonus, level bonus, and buffs.
    /// </summary>
    public record EffectiveStats(
        // Totals
        int STR, int CON, int INT, int PER,
        int MaxHP, int MaxMana,
        // Gear contribution
        int GearSTR, int GearCON, int GearINT, int GearPER,
        // Class bonus (50% of gear stat when gear.GearClass == user.Class)
        int ClassBonusSTR, int ClassBonusCON, int ClassBonusINT, int ClassBonusPER,
        // Level bonus: floor(level / 2) applied equally to all stats
        int LevelBonus,
        // Active buff (zero if expired)
        int BuffSTR, int BuffCON, int BuffINT, int BuffPER
    );

    public interface ICharacterService
    {
        /// <summary>
        /// Computes fully effective stats for a user.
        /// Caller MUST have loaded user.OwnedGear with ThenInclude(ug => ug.GearItem).
        /// Pure computation — no DB I/O, does not call SaveChanges.
        /// </summary>
        Task<EffectiveStats> GetEffectiveStatsAsync(User user);

        /// <summary>
        /// Spends 1 StatPoint into the named stat (STR / CON / INT / PER).
        /// Loads user fresh, validates, saves to DB internally.
        /// Returns (true, null) on success; (false, errorMessage) on failure.
        /// </summary>
        Task<(bool Success, string? Error)> AllocateStatAsync(int userId, string stat);

        /// <summary>
        /// Permanently assigns a class to a user.
        /// Requires: Level >= 10, Class == null, className in CharacterClass.All.
        /// Saves to DB internally.
        /// Returns (true, null) on success; (false, errorMessage) on failure.
        /// </summary>
        Task<(bool Success, string? Error)> ChooseClassAsync(int userId, string className);

        /// <summary>
        /// Resets user progression to level 1. Keeps cosmetics, achievements, owned gear.
        /// Requires: Level >= 10. Saves to DB internally.
        /// </summary>
        Task RebirthAsync(int userId);
    }
}
