using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class SyncAllStableItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDroppable",
                table: "GameItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Icon", "IsDroppable" },
                values: new object[] { "🥚", false });

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 16,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 18,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 20,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 22,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 24,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 25,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 26,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 27,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 28,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 29,
                column: "IsDroppable",
                value: true);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 30,
                column: "IsDroppable",
                value: true);

            migrationBuilder.InsertData(
                table: "GameItems",
                columns: new[] { "Id", "GoldValue", "Icon", "IsDroppable", "Key", "Name", "Rarity", "Target", "Type" },
                values: new object[,]
                {
                    { 31, 3, "🥚", false, "egg_Alligator", "Alligator Egg", 0, null, 1 },
                    { 32, 3, "🥚", false, "egg_Alpaca", "Alpaca Egg", 0, null, 1 },
                    { 33, 3, "🥚", false, "egg_Armadillo", "Armadillo Egg", 0, null, 1 },
                    { 34, 3, "🥚", false, "egg_Badger", "Badger Egg", 0, null, 1 },
                    { 35, 3, "🥚", false, "egg_Beetle", "Beetle Egg", 0, null, 1 },
                    { 36, 3, "🥚", false, "egg_Bunny", "Bunny Egg", 0, null, 1 },
                    { 37, 3, "🥚", false, "egg_Butterfly", "Butterfly Egg", 0, null, 1 },
                    { 38, 3, "🥚", false, "egg_Cat", "Cat Egg", 0, null, 1 },
                    { 39, 3, "🥚", false, "egg_Chameleon", "Chameleon Egg", 0, null, 1 },
                    { 40, 3, "🥚", false, "egg_Cheetah", "Cheetah Egg", 0, null, 1 },
                    { 41, 3, "🥚", false, "egg_Cow", "Cow Egg", 0, null, 1 },
                    { 42, 3, "🥚", false, "egg_Crab", "Crab Egg", 0, null, 1 },
                    { 43, 3, "🥚", false, "egg_Cuttlefish", "Cuttlefish Egg", 0, null, 1 },
                    { 44, 3, "🥚", false, "egg_Deer", "Deer Egg", 0, null, 1 },
                    { 45, 3, "🥚", false, "egg_Dog", "Dog Egg", 0, null, 1 },
                    { 46, 3, "🥚", false, "egg_Dolphin", "Dolphin Egg", 0, null, 1 },
                    { 47, 3, "🥚", false, "egg_Egg", "Egg Egg", 0, null, 1 },
                    { 48, 3, "🥚", false, "egg_Falcon", "Falcon Egg", 0, null, 1 },
                    { 49, 3, "🥚", false, "egg_Ferret", "Ferret Egg", 0, null, 1 },
                    { 50, 3, "🥚", false, "egg_Frog", "Frog Egg", 0, null, 1 },
                    { 51, 3, "🥚", false, "egg_Giraffe", "Giraffe Egg", 0, null, 1 },
                    { 52, 3, "🥚", false, "egg_Gryphon", "Gryphon Egg", 0, null, 1 },
                    { 53, 3, "🥚", false, "egg_GuineaPig", "Guinea Pig Egg", 0, null, 1 },
                    { 54, 3, "🥚", false, "egg_Hedgehog", "Hedgehog Egg", 0, null, 1 },
                    { 55, 3, "🥚", false, "egg_Hippo", "Hippo Egg", 0, null, 1 },
                    { 56, 3, "🥚", false, "egg_Horse", "Horse Egg", 0, null, 1 },
                    { 57, 3, "🥚", false, "egg_Kangaroo", "Kangaroo Egg", 0, null, 1 },
                    { 58, 3, "🥚", false, "egg_Monkey", "Monkey Egg", 0, null, 1 },
                    { 59, 3, "🥚", false, "egg_Nudibranch", "Nudibranch Egg", 0, null, 1 },
                    { 60, 3, "🥚", false, "egg_Octopus", "Octopus Egg", 0, null, 1 },
                    { 61, 3, "🥚", false, "egg_Otter", "Otter Egg", 0, null, 1 },
                    { 62, 3, "🥚", false, "egg_Owl", "Owl Egg", 0, null, 1 },
                    { 63, 3, "🥚", false, "egg_Parrot", "Parrot Egg", 0, null, 1 },
                    { 64, 3, "🥚", false, "egg_Peacock", "Peacock Egg", 0, null, 1 },
                    { 65, 3, "🥚", false, "egg_Penguin", "Penguin Egg", 0, null, 1 },
                    { 66, 3, "🥚", false, "egg_Platypus", "Platypus Egg", 0, null, 1 },
                    { 67, 3, "🥚", false, "egg_PolarBear", "Polar Bear Egg", 0, null, 1 },
                    { 68, 3, "🥚", false, "egg_Pterodactyl", "Pterodactyl Egg", 0, null, 1 },
                    { 69, 3, "🥚", false, "egg_Raccoon", "Raccoon Egg", 0, null, 1 },
                    { 70, 3, "🥚", false, "egg_Rat", "Rat Egg", 0, null, 1 },
                    { 71, 3, "🥚", false, "egg_Robot", "Robot Egg", 0, null, 1 },
                    { 72, 3, "🥚", false, "egg_Rock", "Rock Egg", 0, null, 1 },
                    { 73, 3, "🥚", false, "egg_Rooster", "Rooster Egg", 0, null, 1 },
                    { 74, 3, "🥚", false, "egg_Sabretooth", "Sabretooth Egg", 0, null, 1 },
                    { 75, 3, "🥚", false, "egg_Seahorse", "Seahorse Egg", 0, null, 1 },
                    { 76, 3, "🥚", false, "egg_SeaSerpent", "Sea Serpent Egg", 0, null, 1 },
                    { 77, 3, "🥚", false, "egg_Sheep", "Sheep Egg", 0, null, 1 },
                    { 78, 3, "🥚", false, "egg_Slime", "Slime Egg", 0, null, 1 },
                    { 79, 3, "🥚", false, "egg_Sloth", "Sloth Egg", 0, null, 1 },
                    { 80, 3, "🥚", false, "egg_Snail", "Snail Egg", 0, null, 1 },
                    { 81, 3, "🥚", false, "egg_Snake", "Snake Egg", 0, null, 1 },
                    { 82, 3, "🥚", false, "egg_Spider", "Spider Egg", 0, null, 1 },
                    { 83, 3, "🥚", false, "egg_Squirrel", "Squirrel Egg", 0, null, 1 },
                    { 84, 3, "🥚", false, "egg_Treeling", "Treeling Egg", 0, null, 1 },
                    { 85, 3, "🥚", false, "egg_TRex", "T-Rex Egg", 0, null, 1 },
                    { 86, 3, "🥚", false, "egg_Triceratops", "Triceratops Egg", 0, null, 1 },
                    { 87, 3, "🥚", false, "egg_Turtle", "Turtle Egg", 0, null, 1 },
                    { 88, 3, "🥚", false, "egg_Unicorn", "Unicorn Egg", 0, null, 1 },
                    { 89, 3, "🥚", false, "egg_Velociraptor", "Velociraptor Egg", 0, null, 1 },
                    { 90, 3, "🥚", false, "egg_Whale", "Whale Egg", 0, null, 1 },
                    { 91, 3, "🥚", false, "egg_Yarn", "Yarn Egg", 0, null, 1 },
                    { 92, 0, "🪑", false, "food_Saddle", "Saddle", 0, null, 0 },
                    { 93, 1, "🎂", false, "food_Cake_Base", "Cake (Base)", 0, "Base", 0 },
                    { 94, 1, "🎂", false, "food_Cake_CottonCandyBlue", "Cake (Cotton Candy Blue)", 0, "CottonCandyBlue", 0 },
                    { 95, 1, "🎂", false, "food_Cake_CottonCandyPink", "Cake (Cotton Candy Pink)", 0, "CottonCandyPink", 0 },
                    { 96, 1, "🎂", false, "food_Cake_Desert", "Cake (Desert)", 0, "Desert", 0 },
                    { 97, 1, "🎂", false, "food_Cake_Golden", "Cake (Golden)", 0, "Golden", 0 },
                    { 98, 1, "🎂", false, "food_Cake_Red", "Cake (Red)", 0, "Red", 0 },
                    { 99, 1, "🎂", false, "food_Cake_Shade", "Cake (Shade)", 0, "Shade", 0 },
                    { 100, 1, "🎂", false, "food_Cake_Skeleton", "Cake (Skeleton)", 0, "Skeleton", 0 },
                    { 101, 1, "🎂", false, "food_Cake_White", "Cake (White)", 0, "White", 0 },
                    { 102, 1, "🎂", false, "food_Cake_Zombie", "Cake (Zombie)", 0, "Zombie", 0 },
                    { 103, 1, "🍭", false, "food_Candy_Base", "Candy (Base)", 0, "Base", 0 },
                    { 104, 1, "🍭", false, "food_Candy_CottonCandyBlue", "Candy (Cotton Candy Blue)", 0, "CottonCandyBlue", 0 },
                    { 105, 1, "🍭", false, "food_Candy_CottonCandyPink", "Candy (Cotton Candy Pink)", 0, "CottonCandyPink", 0 },
                    { 106, 1, "🍭", false, "food_Candy_Desert", "Candy (Desert)", 0, "Desert", 0 },
                    { 107, 1, "🍭", false, "food_Candy_Golden", "Candy (Golden)", 0, "Golden", 0 },
                    { 108, 1, "🍭", false, "food_Candy_Red", "Candy (Red)", 0, "Red", 0 },
                    { 109, 1, "🍭", false, "food_Candy_Shade", "Candy (Shade)", 0, "Shade", 0 },
                    { 110, 1, "🍭", false, "food_Candy_Skeleton", "Candy (Skeleton)", 0, "Skeleton", 0 },
                    { 111, 1, "🍭", false, "food_Candy_White", "Candy (White)", 0, "White", 0 },
                    { 112, 1, "🍭", false, "food_Candy_Zombie", "Candy (Zombie)", 0, "Zombie", 0 },
                    { 113, 1, "🥧", false, "food_Pie_Base", "Pie (Base)", 0, "Base", 0 },
                    { 114, 1, "🥧", false, "food_Pie_CottonCandyBlue", "Pie (Cotton Candy Blue)", 0, "CottonCandyBlue", 0 },
                    { 115, 1, "🥧", false, "food_Pie_CottonCandyPink", "Pie (Cotton Candy Pink)", 0, "CottonCandyPink", 0 },
                    { 116, 1, "🥧", false, "food_Pie_Desert", "Pie (Desert)", 0, "Desert", 0 },
                    { 117, 1, "🥧", false, "food_Pie_Golden", "Pie (Golden)", 0, "Golden", 0 },
                    { 118, 1, "🥧", false, "food_Pie_Red", "Pie (Red)", 0, "Red", 0 },
                    { 119, 1, "🥧", false, "food_Pie_Shade", "Pie (Shade)", 0, "Shade", 0 },
                    { 120, 1, "🥧", false, "food_Pie_Skeleton", "Pie (Skeleton)", 0, "Skeleton", 0 },
                    { 121, 1, "🥧", false, "food_Pie_White", "Pie (White)", 0, "White", 0 },
                    { 122, 1, "🥧", false, "food_Pie_Zombie", "Pie (Zombie)", 0, "Zombie", 0 },
                    { 123, 2, "🧪", false, "potion_Amber", "Amber Potion", 2, null, 2 },
                    { 124, 2, "🧪", false, "potion_Aquatic", "Aquatic Potion", 2, null, 2 },
                    { 125, 2, "🧪", false, "potion_Aurora", "Aurora Potion", 2, null, 2 },
                    { 126, 2, "🧪", false, "potion_AutumnLeaf", "Autumn Leaf Potion", 2, null, 2 },
                    { 127, 2, "🧪", false, "potion_Balloon", "Balloon Potion", 2, null, 2 },
                    { 128, 2, "🧪", false, "potion_BirchBark", "Birch Bark Potion", 2, null, 2 },
                    { 129, 2, "🧪", false, "potion_BlackPearl", "Black Pearl Potion", 2, null, 2 },
                    { 130, 2, "🧪", false, "potion_Bronze", "Bronze Potion", 2, null, 2 },
                    { 131, 2, "🧪", false, "potion_Celestial", "Celestial Potion", 2, null, 2 },
                    { 132, 2, "🧪", false, "potion_Cupid", "Cupid Potion", 2, null, 2 },
                    { 133, 2, "🧪", false, "potion_Ember", "Ember Potion", 2, null, 2 },
                    { 134, 2, "🧪", false, "potion_Fairy", "Fairy Potion", 2, null, 2 },
                    { 135, 2, "🧪", false, "potion_Floral", "Floral Potion", 2, null, 2 },
                    { 136, 2, "🧪", false, "potion_Fluorite", "Fluorite Potion", 2, null, 2 },
                    { 137, 2, "🧪", false, "potion_Frost", "Frost Potion", 2, null, 2 },
                    { 138, 2, "🧪", false, "potion_Ghost", "Ghost Potion", 2, null, 2 },
                    { 139, 2, "🧪", false, "potion_Gingerbread", "Gingerbread Potion", 2, null, 2 },
                    { 140, 2, "🧪", false, "potion_Glass", "Glass Potion", 2, null, 2 },
                    { 141, 2, "🧪", false, "potion_Glow", "Glow Potion", 2, null, 2 },
                    { 142, 2, "🧪", false, "potion_Holly", "Holly Potion", 2, null, 2 },
                    { 143, 2, "🧪", false, "potion_IcySnow", "Icy Snow Potion", 2, null, 2 },
                    { 144, 2, "🧪", false, "potion_Jade", "Jade Potion", 2, null, 2 },
                    { 145, 2, "🧪", false, "potion_Koi", "Koi Potion", 2, null, 2 },
                    { 146, 2, "🧪", false, "potion_Moonglow", "Moonglow Potion", 2, null, 2 },
                    { 147, 2, "🧪", false, "potion_MossyStone", "Mossy Stone Potion", 2, null, 2 },
                    { 148, 2, "🧪", false, "potion_Onyx", "Onyx Potion", 2, null, 2 },
                    { 149, 2, "🧪", false, "potion_Opal", "Opal Potion", 2, null, 2 },
                    { 150, 2, "🧪", false, "potion_Peppermint", "Peppermint Potion", 2, null, 2 },
                    { 151, 2, "🧪", false, "potion_PinkMarble", "Pink Marble Potion", 2, null, 2 },
                    { 152, 2, "🧪", false, "potion_PolkaDot", "Polka Dot Potion", 2, null, 2 },
                    { 153, 2, "🧪", false, "potion_Porcelain", "Porcelain Potion", 2, null, 2 },
                    { 154, 2, "🧪", false, "potion_Purple", "Purple Potion", 2, null, 2 },
                    { 155, 2, "🧪", false, "potion_Rainbow", "Rainbow Potion", 2, null, 2 },
                    { 156, 2, "🧪", false, "potion_RoseGold", "Rose Gold Potion", 2, null, 2 },
                    { 157, 2, "🧪", false, "potion_RoseQuartz", "Rose Quartz Potion", 2, null, 2 },
                    { 158, 2, "🧪", false, "potion_RoyalPurple", "Royal Purple Potion", 2, null, 2 },
                    { 159, 2, "🧪", false, "potion_Ruby", "Ruby Potion", 2, null, 2 },
                    { 160, 2, "🧪", false, "potion_SandSculpture", "Sand Sculpture Potion", 2, null, 2 },
                    { 161, 2, "🧪", false, "potion_Shadow", "Shadow Potion", 2, null, 2 },
                    { 162, 2, "🧪", false, "potion_Shimmer", "Shimmer Potion", 2, null, 2 },
                    { 163, 2, "🧪", false, "potion_Silver", "Silver Potion", 2, null, 2 },
                    { 164, 2, "🧪", false, "potion_SolarSystem", "Solar System Potion", 2, null, 2 },
                    { 165, 2, "🧪", false, "potion_Spooky", "Spooky Potion", 2, null, 2 },
                    { 166, 2, "🧪", false, "potion_StainedGlass", "Stained Glass Potion", 2, null, 2 },
                    { 167, 2, "🧪", false, "potion_StarryNight", "Starry Night Potion", 2, null, 2 },
                    { 168, 2, "🧪", false, "potion_Sunset", "Sunset Potion", 2, null, 2 },
                    { 169, 2, "🧪", false, "potion_Sunshine", "Sunshine Potion", 2, null, 2 },
                    { 170, 2, "🧪", false, "potion_TeaShop", "Tea Shop Potion", 2, null, 2 },
                    { 171, 2, "🧪", false, "potion_Thunderstorm", "Thunderstorm Potion", 2, null, 2 },
                    { 172, 2, "🧪", false, "potion_Turquoise", "Turquoise Potion", 2, null, 2 },
                    { 173, 2, "🧪", false, "potion_Vampire", "Vampire Potion", 2, null, 2 },
                    { 174, 2, "🧪", false, "potion_Watery", "Watery Potion", 2, null, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DropColumn(
                name: "IsDroppable",
                table: "GameItems");

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "Icon",
                value: "🦎");
        }
    }
}
