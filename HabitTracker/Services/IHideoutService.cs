using HabitTracker.Models;

namespace HabitTracker.Services
{
    public interface IHideoutService
    {
        /// <summary>
        /// Ensures all 5 facilities exist for the user at level 1, creating any missing ones.
        /// </summary>
        Task EnsureUserFacilitiesAsync(int userId);

        /// <summary>
        /// Returns all of the user's facilities with their Facility navigation property loaded.
        /// </summary>
        Task<List<UserFacility>> GetUserFacilitiesAsync(int userId);

        /// <summary>
        /// Sums up the buff totals across all user facilities.
        /// Returns (atk, hp, armor, xpGain%, stamina).
        /// </summary>
        (int atk, int hp, int armor, double xpGain, int stamina) GetFacilityBuffs(List<UserFacility> facilities);

        /// <summary>Returns the user's Storage Room level (1 if not found).</summary>
        int GetStorageLevel(List<UserFacility> facilities);

        /// <summary>
        /// Starts an upgrade: deducts materials, sets UpgradeStartedAt.
        /// Returns (true, null) on success, (false, errorMessage) on failure.
        /// </summary>
        Task<(bool Success, string? Error)> StartUpgradeAsync(int userId, int facilityId);

        /// <summary>Auto-completes any upgrades whose timer has elapsed.</summary>
        Task CompleteReadyUpgradesAsync(int userId);
    }
}
