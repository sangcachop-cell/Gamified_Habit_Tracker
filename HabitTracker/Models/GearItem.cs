using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Equipment catalog item. Key follows Habitica naming: {slot}_{habiticaClass}_{tier}
    /// Note: "mage" class uses "wizard" in image filenames — use HabiticaClassKey for paths.
    /// </summary>
    public class GearItem
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Habitica image key e.g. "weapon_warrior_1", "armor_wizard_3"</summary>
        [Required]
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>weapon | armor | head | shield | back | eyewear | headAccessory | body</summary>
        [Required]
        [StringLength(30)]
        public string Slot { get; set; } = string.Empty;

        /// <summary>warrior | mage | rogue | healer | special | all</summary>
        [Required]
        [StringLength(20)]
        public string GearClass { get; set; } = "all";

        public int Tier { get; set; } = 0;

        public int GoldCost { get; set; } = 0;

        public int STR { get; set; } = 0;
        public int CON { get; set; } = 0;
        public int INT { get; set; } = 0;
        public int PER { get; set; } = 0;

        public bool TwoHanded { get; set; } = false;
        public bool IsSpecial { get; set; } = false;
        public bool IsArmoire { get; set; } = false;

        // ===== IMAGE PATH HELPERS =====

        /// <summary>Shop icon PNG path for inventory/shop UI.</summary>
        public string ShopImagePath =>
            Key.Contains("_armoire_")
                ? $"/images/habitica/gear/armoire/shop/shop_{Key}.png"
                : $"/images/habitica/gear/{Slot}/shop/shop_{Key}.png";

        /// <summary>
        /// Worn overlay PNG path for avatar rendering.
        /// bodyType = "broad" | "slim" (only matters for armor slot).
        /// </summary>
        public string GetWornImagePath(string bodyType = "broad")
        {
            if (Key.Contains("_armoire_"))
                return Slot == "armor"
                    ? $"/images/habitica/gear/armoire/{bodyType}_{Key}.png"
                    : $"/images/habitica/gear/armoire/{Key}.png";
            return Slot == "armor"
                ? $"/images/habitica/gear/{Slot}/{bodyType}_{Key}.png"
                : $"/images/habitica/gear/{Slot}/{Key}.png";
        }

        // ===== NAVIGATION =====
        public virtual ICollection<UserGearItem>? OwnedByUsers { get; set; }
    }
}