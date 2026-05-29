using HabitTracker.Models;
using HabitTracker.Services;

namespace HabitTracker.Models.ViewModels
{
    public class SpellsViewModel
    {
        public User                            User           { get; set; } = null!;
        public EffectiveStats                  EffectiveStats { get; set; } = default!;
        public IReadOnlyList<SpellDefinition>  Spells         { get; set; } = Array.Empty<SpellDefinition>();

        /// <summary>All active non-Reward tasks — for task-targeting spell dropdowns.</summary>
        public List<HabitTask>                 Tasks          { get; set; } = new();

        public int  MaxMana   => EffectiveStats.MaxMana;
        public bool HasClass  => !string.IsNullOrEmpty(User.Class);

        /// <summary>True if user meets level gate and has enough mana to cast this spell.</summary>
        public bool CanCast(SpellDefinition spell) =>
            User.Level >= spell.MinLevel && User.Mana >= spell.ManaCost;

        /// <summary>Human-readable reason why a spell cannot be cast (null = can cast).</summary>
        public string? CantCastReason(SpellDefinition spell)
        {
            if (User.Level < spell.MinLevel)
                return $"Requires level {spell.MinLevel}";
            if (User.Mana < spell.ManaCost)
                return $"Need {spell.ManaCost} MP (have {(int)User.Mana})";
            return null;
        }
    }
}
