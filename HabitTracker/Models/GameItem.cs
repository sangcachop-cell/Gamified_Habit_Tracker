using HabitTracker.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    public class GameItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(10)]
        public string Icon { get; set; } = "📦";

        public ItemType   Type   { get; set; }
        public ItemRarity Rarity { get; set; } = ItemRarity.Common;

        /// <summary>Gold returned when player sells this item. 0 = not sellable.</summary>
        public int GoldValue { get; set; } = 0;

        /// <summary>For Food: preferred potion color (e.g. "Base", "Golden"). Null for non-food.</summary>
        [StringLength(50)]
        public string? Target { get; set; }

        /// <summary>False for quest/premium items that should not appear in random task drops.</summary>
        public bool IsDroppable { get; set; } = true;

        /// <summary>
        /// Habitica stable image path. Derives from Key — no DB column needed.
        /// Special cases: food_Potato→Potatoe, egg_Bear→BearCub.
        /// </summary>
        public string ImagePath => Type switch
        {
            ItemType.Food           => $"/images/habitica/stable/food/Pet_Food_{FoodKey()}.png",
            ItemType.Egg            => $"/images/habitica/stable/eggs/Pet_Egg_{EggKey()}.png",
            ItemType.HatchingPotion => $"/images/habitica/stable/potions/Pet_HatchingPotion_{PotionKey()}.png",
            ItemType.QuestScroll    => $"/images/habitica/quests/scrolls/inventory_quest_scroll_{Key.Replace("quest_", "")}.png",
            _                      => string.Empty
        };

        /// <summary>For Egg items: animal name used in PetKey (e.g. "BearCub", "Wolf"). Null for non-eggs.</summary>
        [NotMapped]
        public string? AnimalKey => Type == ItemType.Egg ? EggKey() : null;

        /// <summary>For HatchingPotion items: color key used in PetKey (e.g. "Base", "Golden"). Null for non-potions.</summary>
        [NotMapped]
        public string? PotionColorKey => Type == ItemType.HatchingPotion ? PotionKey() : null;

        private string FoodKey()
        {
            var name = Key.Replace("food_", "");
            return name == "Potato" ? "Potatoe" : name;
        }

        private string EggKey()
        {
            var name = Key.Replace("egg_", "");
            return name == "Bear" ? "BearCub" : name;
        }

        private string PotionKey() => Key.Replace("potion_", "");
    }
}
