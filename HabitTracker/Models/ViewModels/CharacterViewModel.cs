using HabitTracker.Services;

namespace HabitTracker.Models.ViewModels
{
    public class CharacterViewModel
    {
        public User           User           { get; set; } = null!;
        public EffectiveStats EffectiveStats { get; set; } = default!;
        public int            XpWithinLevel  { get; set; }
        public int            XpToNextLevel  { get; set; }

        /// <summary>
        /// True when level >= 10 and no class chosen yet — show "Select Class" prompt.
        /// </summary>
        public bool CanSelectClass => User.Level >= 10 && string.IsNullOrEmpty(User.Class);
    }
}
