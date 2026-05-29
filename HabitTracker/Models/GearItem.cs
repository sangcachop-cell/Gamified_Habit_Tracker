using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class GearItem
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string Slot { get; set; } = string.Empty;

        [Required, StringLength(20)]
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

        public string ShopImagePath
        {
            get
            {
                if (Key.Contains("_armoire_"))
                {
                    if (Slot == "armor")
                        return $"/fe/gear/armoire/broad_{Key}.png";

                    return $"/fe/gear/armoire/{Key}.png";
                }

                if (Slot == "armor")
                    return $"/fe/gear/armor/broad_{Key}.png";

                return $"/fe/gear/{Slot}/{Key}.png";
            }
        }

        public string GetWornImagePath(string bodyType = "broad")
        {
            if (Key.Contains("_armoire_"))
            {
                if (Slot == "armor")
                    return $"/fe/gear/armoire/{bodyType}_{Key}.png";

                return $"/fe/gear/armoire/{Key}.png";
            }

            if (Slot == "armor")
                return $"/fe/gear/armor/{bodyType}_{Key}.png";

            return $"/fe/gear/{Slot}/{Key}.png";
        }

        public virtual ICollection<UserGearItem>? OwnedByUsers { get; set; }
    }
}