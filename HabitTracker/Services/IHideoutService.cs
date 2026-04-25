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
    }
}
