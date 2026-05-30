using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase8BossQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BossQuests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Completion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LevelRequired = table.Column<int>(type: "int", nullable: false),
                    PrerequisiteQuestKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GoldCost = table.Column<int>(type: "int", nullable: false),
                    IsBossQuest = table.Column<bool>(type: "bit", nullable: false),
                    BossHp = table.Column<double>(type: "float", nullable: true),
                    BossStr = table.Column<double>(type: "float", nullable: false),
                    BossDef = table.Column<double>(type: "float", nullable: false),
                    RageValue = table.Column<double>(type: "float", nullable: true),
                    RageHealing = table.Column<double>(type: "float", nullable: true),
                    RageMpDrain = table.Column<double>(type: "float", nullable: true),
                    RageProgressDrain = table.Column<double>(type: "float", nullable: true),
                    RageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RageEffect = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CollectJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DropGold = table.Column<int>(type: "int", nullable: false),
                    DropExp = table.Column<int>(type: "int", nullable: false),
                    DropItemsJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BossQuests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartyQuests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    BossQuestId = table.Column<int>(type: "int", nullable: false),
                    LeaderUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BossHpRemaining = table.Column<double>(type: "float", nullable: false),
                    RageMeter = table.Column<double>(type: "float", nullable: false),
                    CollectProgressJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyQuests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyQuests_BossQuests_BossQuestId",
                        column: x => x.BossQuestId,
                        principalTable: "BossQuests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyQuests_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyQuests_Users_LeaderUserId",
                        column: x => x.LeaderUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyQuestMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyQuestId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDamageDealt = table.Column<double>(type: "float", nullable: false),
                    TotalRageGiven = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyQuestMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyQuestMembers_PartyQuests_PartyQuestId",
                        column: x => x.PartyQuestId,
                        principalTable: "PartyQuests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyQuestMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BossQuests",
                columns: new[] { "Id", "BossDef", "BossHp", "BossStr", "Category", "CollectJson", "Completion", "DropExp", "DropGold", "DropItemsJson", "GoldCost", "Group", "IsBossQuest", "Key", "LevelRequired", "Notes", "PrerequisiteQuestKey", "RageEffect", "RageHealing", "RageMpDrain", "RageName", "RageProgressDrain", "RageValue", "Text" },
                values: new object[,]
                {
                    { 1, 1.0, 1100.0, 2.5, "pet", null, "", 725, 73, "[{\"type\":\"eggs\",\"key\":\"Alligator\",\"count\":3}]", 4, null, true, "alligator", 0, "", null, null, null, null, null, null, null, "The Insta-Gator" },
                    { 2, 1.0, 800.0, 2.0, "pet", null, "", 900, 90, "[{\"type\":\"eggs\",\"key\":\"Alpaca\",\"count\":3}]", 4, null, true, "alpaca", 0, "", null, null, 0.29999999999999999, null, "Alpaca Rage", null, 50.0, "The Overpacked Alpaca" },
                    { 3, 1.0, 600.0, 1.5, "pet", null, "", 350, 43, "[{\"type\":\"eggs\",\"key\":\"Armadillo\",\"count\":3}]", 4, null, true, "armadillo", 0, "", null, null, null, null, null, null, null, "The Indulgent Armadillo" },
                    { 4, 1.0, 500.0, 1.5, "pet", null, "", 275, 37, "[{\"type\":\"eggs\",\"key\":\"Axolotl\",\"count\":3}]", 4, null, true, "axolotl", 0, "", null, null, 0.29999999999999999, null, "Axolotl Rage", null, 50.0, "The Magical Axolotl" },
                    { 5, 1.0, 600.0, 1.5, "pet", null, "", 350, 43, "[{\"type\":\"eggs\",\"key\":\"Badger\",\"count\":3}]", 4, null, true, "badger", 0, "", null, null, null, null, null, null, null, "Stop Badgering Me!" },
                    { 6, 1.0, 500.0, 1.5, "pet", null, "", 275, 37, "[{\"type\":\"eggs\",\"key\":\"Beetle\",\"count\":3}]", 4, null, true, "beetle", 0, "", null, null, null, null, null, null, null, "The CRITICAL BUG" },
                    { 7, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Bunny\",\"count\":3}]", 4, null, true, "bunny", 0, "", null, null, null, null, null, null, null, "The Killer Bunny" },
                    { 8, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Butterfly\",\"count\":3}]", 4, null, true, "butterfly", 0, "", null, null, null, null, null, null, null, "Bye, Bye, Butterfry" },
                    { 9, 1.0, 600.0, 1.5, "pet", null, "", 500, 55, "[{\"type\":\"eggs\",\"key\":\"Cat\",\"count\":3}]", 4, null, true, "cat", 0, "", null, null, null, 0.33000000000000002, "Cat Rage", null, 50.0, "A Purrplexing Predicament" },
                    { 10, 1.0, 400.0, 1.5, "pet", null, "", 250, 35, "[{\"type\":\"eggs\",\"key\":\"Chameleon\",\"count\":3}]", 4, null, true, "chameleon", 0, "", null, null, null, null, null, null, null, "The Chaotic Chameleon" },
                    { 11, 1.0, 600.0, 1.5, "pet", null, "", 350, 43, "[{\"type\":\"eggs\",\"key\":\"Cheetah\",\"count\":3}]", 4, null, true, "cheetah", 0, "", null, null, null, null, null, null, null, "Such a Cheetah" },
                    { 12, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Cow\",\"count\":3}]", 4, null, true, "cow", 0, "", null, null, null, null, null, null, null, "The Mootant Cow" },
                    { 13, 1.0, 1200.0, 2.5, "pet", null, "", 900, 90, "[{\"type\":\"eggs\",\"key\":\"Crab\",\"count\":3}]", 4, null, true, "crab", 0, "", null, null, null, 0.33000000000000002, "Crab Rage", null, 50.0, "The Fiddling Crab" },
                    { 14, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Seahorse\",\"count\":3}]", 4, null, true, "dilatory_derby", 0, "", null, null, null, null, null, null, null, "The Dilatory Derby" },
                    { 15, 1.0, 600.0, 1.5, "pet", null, "", 500, 55, "[{\"type\":\"eggs\",\"key\":\"Dog\",\"count\":3}]", 4, null, true, "dog", 0, "", null, null, null, 0.29999999999999999, "Dog Rage", null, 50.0, "Triple Dog Dare!" },
                    { 16, 1.0, 300.0, 1.25, "pet", null, "", 110, 22, "[{\"type\":\"eggs\",\"key\":\"Dolphin\",\"count\":3}]", 4, null, true, "dolphin", 0, "", null, null, null, null, null, null, null, "The Dolphin of Doubt" },
                    { 17, 1.0, 700.0, 2.0, "pet", null, "", 425, 49, "[{\"type\":\"eggs\",\"key\":\"Falcon\",\"count\":3}]", 4, null, true, "falcon", 0, "", null, null, null, null, null, null, null, "The Birds of Preycrastination" },
                    { 18, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Ferret\",\"count\":3}]", 4, null, true, "ferret", 0, "", null, null, null, null, null, null, null, "The Nefarious Ferret" },
                    { 19, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Frog\",\"count\":3}]", 4, null, true, "frog", 0, "", null, null, null, null, null, null, null, "Swamp of the Clutter Frog" },
                    { 20, 1.0, 1200.0, 2.5, "pet", null, "", 800, 80, "[{\"type\":\"eggs\",\"key\":\"Deer\",\"count\":3}]", 4, null, true, "ghost_stag", 0, "", null, null, null, null, null, null, null, "The Spirit of Spring" },
                    { 21, 1.0, 700.0, 2.0, "pet", null, "", 450, 50, "[{\"type\":\"eggs\",\"key\":\"Giraffe\",\"count\":3}]", 4, null, true, "giraffe", 0, "", null, null, null, null, null, null, null, "The Gear-affe" },
                    { 22, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Gryphon\",\"count\":3}]", 4, null, true, "gryphon", 0, "", null, null, null, null, null, null, null, "The Fiery Gryphon" },
                    { 23, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"GuineaPig\",\"count\":3}]", 4, null, true, "guineapig", 0, "", null, null, null, null, null, null, null, "The Guinea Pig Gang" },
                    { 24, 1.0, 600.0, 1.5, "pet", null, "", 350, 43, "[{\"type\":\"eggs\",\"key\":\"Parrot\",\"count\":3}]", 4, null, true, "harpy", 0, "", null, null, null, null, null, null, null, "Help! Harpy!" },
                    { 25, 1.0, 400.0, 1.25, "pet", null, "", 125, 30, "[{\"type\":\"eggs\",\"key\":\"Hedgehog\",\"count\":3}]", 4, null, true, "hedgehog", 0, "", null, null, null, null, null, null, null, "The Hedgebeast" },
                    { 26, 1.0, 800.0, 2.0, "pet", null, "", 500, 55, "[{\"type\":\"eggs\",\"key\":\"Hippo\",\"count\":3}]", 4, null, true, "hippo", 0, "", null, null, null, null, null, null, null, "What a Hippo-Crite" },
                    { 27, 1.0, 500.0, 1.5, "pet", null, "", 275, 37, "[{\"type\":\"eggs\",\"key\":\"Horse\",\"count\":3}]", 4, null, true, "horse", 0, "", null, null, null, null, null, null, null, "Ride the Night-Mare" },
                    { 28, 1.0, 700.0, 2.0, "pet", null, "", 425, 49, "[{\"type\":\"eggs\",\"key\":\"Kangaroo\",\"count\":3}]", 4, null, true, "kangaroo", 0, "", null, null, null, null, null, null, null, "Kangaroo Catastrophe" },
                    { 29, 1.0, 800.0, 2.0, "pet", null, "", 500, 55, "[{\"type\":\"eggs\",\"key\":\"Cuttlefish\",\"count\":3}]", 4, null, true, "kraken", 0, "", null, null, null, null, null, null, null, "The Kraken of Inkomplete" },
                    { 30, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Monkey\",\"count\":3}]", 4, null, true, "monkey", 0, "", null, null, null, null, null, null, null, "Monstrous Mandrill and the Mischief Monkeys" },
                    { 31, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Nudibranch\",\"count\":3}]", 4, null, true, "nudibranch", 0, "", null, null, null, null, null, null, null, "Infestation of the NowDo Nudibranchs" },
                    { 32, 1.0, 1200.0, 2.5, "pet", null, "", 800, 80, "[{\"type\":\"eggs\",\"key\":\"Octopus\",\"count\":3}]", 4, null, true, "octopus", 0, "", null, null, null, null, null, null, null, "The Call of Octothulu" },
                    { 33, 1.0, 1200.0, 2.5, "pet", null, "", 900, 90, "[{\"type\":\"eggs\",\"key\":\"Otter\",\"count\":3}]", 4, null, true, "otter", 0, "", null, null, 0.29999999999999999, null, "Otter Rage", null, 50.0, "The Perfidious Plotter!" },
                    { 34, 1.0, 500.0, 1.5, "pet", null, "", 275, 37, "[{\"type\":\"eggs\",\"key\":\"Owl\",\"count\":3}]", 4, null, true, "owl", 0, "", null, null, null, null, null, null, null, "The Night-Owl" },
                    { 35, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Peacock\",\"count\":3}]", 4, null, true, "peacock", 0, "", null, null, null, null, null, null, null, "The Push-and-Pull Peacock" },
                    { 36, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Penguin\",\"count\":3}]", 4, null, true, "penguin", 0, "", null, null, null, null, null, null, null, "The Fowl Frost" },
                    { 37, 1.0, 1000.0, 2.0, "pet", null, "", 1000, 100, "[{\"type\":\"eggs\",\"key\":\"Platypus\",\"count\":3}]", 4, null, true, "platypus", 0, "", null, null, null, 0.33000000000000002, "Platypus Rage", null, 50.0, "The Perfectionist Platypus" },
                    { 38, 1.0, 1000.0, 2.0, "pet", null, "", 650, 67, "[{\"type\":\"eggs\",\"key\":\"Pterodactyl\",\"count\":3}]", 4, null, true, "pterodactyl", 0, "", null, null, null, null, null, null, null, "The Pterror-dactyl" },
                    { 39, 1.0, 800.0, 2.0, "pet", null, "", 600, 70, "[{\"type\":\"eggs\",\"key\":\"Raccoon\",\"count\":3}]", 4, null, true, "raccoon", 0, "", null, null, 0.29999999999999999, null, "Raccoon Rage", null, 50.0, "Raccoon Tycoon" },
                    { 40, 1.0, 1200.0, 2.5, "pet", null, "", 800, 80, "[{\"type\":\"eggs\",\"key\":\"Rat\",\"count\":3}]", 4, null, true, "rat", 0, "", null, null, null, null, null, null, null, "The Rat King" },
                    { 41, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Rock\",\"count\":3}]", 4, null, true, "rock", 0, "", null, null, null, null, null, null, null, "Escape the Cave Creature" },
                    { 42, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Rooster\",\"count\":3}]", 4, null, true, "rooster", 0, "", null, null, null, null, null, null, null, "Rooster Rampage" },
                    { 43, 1.0, 1000.0, 2.0, "pet", null, "", 650, 67, "[{\"type\":\"eggs\",\"key\":\"Sabretooth\",\"count\":3}]", 4, null, true, "sabretooth", 0, "", null, null, null, null, null, null, null, "The Sabre Cat" },
                    { 44, 1.0, 1200.0, 2.5, "pet", null, "", 800, 80, "[{\"type\":\"eggs\",\"key\":\"SeaSerpent\",\"count\":3}]", 4, null, true, "seaserpent", 0, "", null, null, null, null, null, null, null, "Danger in the Depths: Sea Serpent Strike!" },
                    { 45, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Sheep\",\"count\":3}]", 4, null, true, "sheep", 0, "", null, null, null, null, null, null, null, "The Thunder Ram" },
                    { 46, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Slime\",\"count\":3}]", 4, null, true, "slime", 0, "", null, null, null, null, null, null, null, "The Jelly Regent" },
                    { 47, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Sloth\",\"count\":3}]", 4, null, true, "sloth", 0, "", null, null, null, null, null, null, null, "The Somnolent Sloth" },
                    { 48, 1.0, 500.0, 1.5, "pet", null, "", 275, 37, "[{\"type\":\"eggs\",\"key\":\"Snail\",\"count\":3}]", 4, null, true, "snail", 0, "", null, null, null, null, null, null, null, "The Snail of Drudgery Sludge" },
                    { 49, 1.0, 1100.0, 2.5, "pet", null, "", 725, 73, "[{\"type\":\"eggs\",\"key\":\"Snake\",\"count\":3}]", 4, null, true, "snake", 0, "", null, null, null, null, null, null, null, "The Serpent of Distraction" },
                    { 50, 1.0, 400.0, 1.5, "pet", null, "", 200, 31, "[{\"type\":\"eggs\",\"key\":\"Spider\",\"count\":3}]", 4, null, true, "spider", 0, "", null, null, null, null, null, null, null, "The Icy Arachnid" },
                    { 51, 1.0, 700.0, 2.0, "pet", null, "", 425, 49, "[{\"type\":\"eggs\",\"key\":\"Squirrel\",\"count\":3}]", 4, null, true, "squirrel", 0, "", null, null, null, null, null, null, null, "The Sneaky Squirrel" },
                    { 52, 1.0, 600.0, 1.5, "pet", null, "", 350, 43, "[{\"type\":\"eggs\",\"key\":\"Treeling\",\"count\":3}]", 4, null, true, "treeling", 0, "", null, null, null, null, null, null, null, "The Tangle Tree" },
                    { 53, 1.0, 800.0, 2.0, "pet", null, "", 500, 55, "[{\"type\":\"eggs\",\"key\":\"TRex\",\"count\":3}]", 4, null, true, "trex", 0, "", null, null, null, null, null, null, null, "King of the Dinosaurs" },
                    { 54, 1.0, 500.0, 2.0, "pet", null, "", 500, 55, "[{\"type\":\"eggs\",\"key\":\"TRex\",\"count\":3}]", 4, null, true, "trex_undead", 0, "", null, null, 0.29999999999999999, null, "TRex Undead Rage", null, 50.0, "The Dinosaur Unearthed" },
                    { 55, 1.0, 1200.0, 2.5, "pet", null, "", 800, 80, "[{\"type\":\"eggs\",\"key\":\"Triceratops\",\"count\":3}]", 4, null, true, "triceratops", 0, "", null, null, null, null, null, null, null, "The Trampling Triceratops" },
                    { 56, 1.0, 300.0, 1.5, "pet", null, "", 125, 25, "[{\"type\":\"eggs\",\"key\":\"Turtle\",\"count\":3}]", 4, null, true, "turtle", 0, "", null, null, null, null, null, null, null, "Guide the Turtle" },
                    { 57, 1.0, 600.0, 1.5, "pet", null, "", 350, 43, "[{\"type\":\"eggs\",\"key\":\"Unicorn\",\"count\":3}]", 4, null, true, "unicorn", 0, "", null, null, null, null, null, null, null, "Convincing the Unicorn Queen" },
                    { 58, 1.0, 900.0, 2.0, "pet", null, "", 575, 63, "[{\"type\":\"eggs\",\"key\":\"Velociraptor\",\"count\":3}]", 4, null, true, "velociraptor", 0, "", null, null, null, null, null, null, null, "The Veloci-Rapper" },
                    { 59, 1.0, 500.0, 1.5, "pet", null, "", 275, 37, "[{\"type\":\"eggs\",\"key\":\"Whale\",\"count\":3}]", 4, null, true, "whale", 0, "", null, null, null, null, null, null, null, "Wail of the Whale" },
                    { 60, 1.0, 500.0, 1.5, "pet", null, "", 275, 37, "[{\"type\":\"eggs\",\"key\":\"Yarn\",\"count\":3}]", 4, null, true, "yarn", 0, "", null, null, null, null, null, null, null, "A Tangled Yarn" },
                    { 61, 1.0, 300.0, 1.25, "hatchingPotion", null, "", 100, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"Amber\",\"count\":3}]", 4, null, true, "amber", 0, "", null, null, null, null, null, null, null, "The Amber Alliance" },
                    { 62, 1.0, 725.0, 1.75, "hatchingPotion", null, "", 450, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"BlackPearl\",\"count\":3}]", 4, null, true, "blackPearl", 0, "", null, null, null, null, null, null, null, "A Startling Starry Idea" },
                    { 63, 1.0, 800.0, 2.0, "hatchingPotion", null, "", 575, 63, "[{\"type\":\"hatchingPotions\",\"key\":\"Bronze\",\"count\":3}]", 4, null, true, "bronze", 0, "", null, null, null, null, null, null, null, "Brazen Beetle Battle" },
                    { 64, 1.0, 1200.0, 2.0, "hatchingPotion", null, "", 750, 70, "[{\"type\":\"hatchingPotions\",\"key\":\"Fluorite\",\"count\":3}]", 4, null, true, "fluorite", 0, "", null, null, null, null, null, null, null, "A Bright Fluorite Fright" },
                    { 65, 1.0, 400.0, 1.25, "hatchingPotion", null, "", 400, 40, "[{\"type\":\"hatchingPotions\",\"key\":\"Jade\",\"count\":3}]", 4, null, true, "jade", 0, "", null, null, null, null, null, null, null, "A Jaded Jinx" },
                    { 66, 1.0, 1200.0, 2.0, "hatchingPotion", null, "", 800, 75, "[{\"type\":\"hatchingPotions\",\"key\":\"PinkMarble\",\"count\":3}]", 4, null, true, "pinkMarble", 0, "", null, null, null, null, "Marble Rage", 0.5, 50.0, "Calm the Corrupted Cupid" },
                    { 67, 1.0, null, 1.0, "hatchingPotion", "{\"onyxStone\":25,\"plutoRune\":10,\"leoRune\":10}", "", 100, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"Onyx\",\"count\":3}]", 4, null, false, "onyx", 0, "", null, null, null, null, null, null, null, "The Onyx Odyssey" },
                    { 68, 1.0, null, 1.0, "hatchingPotion", "{\"rubyGem\":25,\"venusRune\":10,\"aquariusRune\":10}", "", 100, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"Ruby\",\"count\":3}]", 4, null, false, "ruby", 0, "", null, null, null, null, null, null, null, "Ruby Rapport" },
                    { 69, 1.0, null, 1.0, "hatchingPotion", "{\"silverIngot\":20,\"moonRune\":15,\"cancerRune\":15}", "", 100, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"Silver\",\"count\":3}]", 4, null, false, "silver", 0, "", null, null, null, null, null, null, null, "The Silver Solution" },
                    { 70, 1.0, null, 1.0, "hatchingPotion", "{\"mossyStone\":25,\"marsRune\":10,\"capricornRune\":10}", "", 100, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"MossyStone\",\"count\":3}]", 4, null, false, "stone", 0, "", null, null, null, null, null, null, null, "A Maze of Moss" },
                    { 71, 1.0, null, 1.0, "hatchingPotion", "{\"turquoiseGem\":25,\"sagittariusRune\":10,\"neptuneRune\":10}", "", 100, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"Turquoise\",\"count\":3}]", 4, null, false, "turquoise", 0, "", null, null, null, null, null, null, null, "Turquoise Treasure Toil" },
                    { 72, 1.0, null, 1.0, "hatchingPotion", "{\"opalGem\":25,\"libraRune\":10,\"mercuryRune\":10}", "", 350, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"Opal\",\"count\":3}]", 4, null, false, "opal", 0, "", null, null, null, null, null, null, null, "The Legend of the Obscure Opals" },
                    { 73, 1.0, 300.0, 1.0, "pet", null, "", 100, 20, "[{\"type\":\"mounts\",\"key\":\"BearCub-Polar\",\"count\":1}]", 4, null, true, "evilsanta", 0, "", null, null, null, null, null, null, null, "Trapper Santa" },
                    { 74, 1.0, null, 1.0, "pet", "{\"tracks\":20,\"branches\":10}", "", 100, 20, "[{\"type\":\"pets\",\"key\":\"BearCub-Polar\",\"count\":1}]", 4, null, false, "evilsanta2", 0, "", null, null, null, null, null, null, null, "Find the Cub" },
                    { 75, 1.0, null, 1.0, "pet", "{\"plainEgg\":40}", "", 0, 0, "[{\"type\":\"eggs\",\"key\":\"Egg\",\"count\":10}]", 1, null, false, "egg", 0, "", null, null, null, null, null, null, null, "Egg Hunt" },
                    { 76, 1.0, 500.0, 2.0, "hatchingPotion", null, "", 500, 40, "[{\"type\":\"hatchingPotions\",\"key\":\"Dessert\",\"count\":3}]", 4, null, true, "waffle", 0, "", null, null, null, null, "Waffle Rage", 0.5, 50.0, "Waffling with the Fool: Disaster Breakfast!" },
                    { 77, 1.0, 500.0, 2.0, "hatchingPotion", null, "", 500, 40, "[{\"type\":\"hatchingPotions\",\"key\":\"VirtualPet\",\"count\":3}]", 4, null, true, "virtualpet", 0, "", null, null, null, null, "VirtualPet Rage", 0.5, 50.0, "Virtual Mayhem with the April Fool: The Beepening" },
                    { 78, 1.0, 500.0, 2.0, "hatchingPotion", null, "", 500, 40, "[{\"type\":\"hatchingPotions\",\"key\":\"Fungi\",\"count\":3}]", 4, null, true, "fungi", 0, "", null, null, null, 0.33000000000000002, "Fungi Rage", null, 50.0, "The Moody Mushroom" },
                    { 79, 1.0, 500.0, 2.0, "hatchingPotion", null, "", 500, 40, "[{\"type\":\"hatchingPotions\",\"key\":\"Alien\",\"count\":3}]", 4, null, true, "alien", 0, "", null, null, 0.29999999999999999, null, "Alien Rage", null, 50.0, "Invasion of the Motivation Snatchers" },
                    { 80, 1.0, null, 1.0, "unlockable", "{\"soapBars\":20}", "", 50, 7, "[{\"type\":\"quests\",\"key\":\"atom2\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupAtom", false, "atom1", 15, "", null, null, null, null, null, null, null, "Attack of the Mundane, Part 1: Dish Disaster!" },
                    { 81, 1.0, 300.0, 1.0, "unlockable", null, "", 100, 20, "[{\"type\":\"quests\",\"key\":\"atom3\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupAtom", true, "atom2", 15, "", "atom1", null, null, null, null, null, null, "Attack of the Mundane, Part 2: The SnackLess Monster" },
                    { 82, 1.0, 800.0, 1.5, "unlockable", null, "", 125, 25, "[{\"type\":\"hatchingPotions\",\"key\":\"Base\",\"count\":2}]", 4, "questGroupAtom", true, "atom3", 15, "", "atom2", null, null, null, null, null, null, "Attack of the Mundane, Part 3: The Laundromancer" },
                    { 83, 1.0, null, 1.0, "unlockable", "{\"testimony\":60}", "", 120, 15, "[{\"type\":\"quests\",\"key\":\"goldenknight2\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupGoldenknight", false, "goldenknight1", 40, "", null, null, null, null, null, null, null, "The Golden Knight, Part 1: A Stern Talking-To" },
                    { 84, 1.0, 1000.0, 3.0, "unlockable", null, "", 750, 75, "[{\"type\":\"quests\",\"key\":\"goldenknight3\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupGoldenknight", true, "goldenknight2", 40, "", "goldenknight1", null, null, null, null, null, null, "The Golden Knight, Part 2: Gold Knight" },
                    { 85, 1.0, 1700.0, 3.5, "unlockable", null, "", 1500, 900, "[{\"type\":\"food\",\"key\":\"Honey\",\"count\":3},{\"type\":\"hatchingPotions\",\"key\":\"Golden\",\"count\":2}]", 4, "questGroupGoldenknight", true, "goldenknight3", 40, "", "goldenknight2", null, null, null, null, null, null, "The Golden Knight, Part 3: The Iron Knight" },
                    { 86, 1.0, null, 1.0, "unlockable", "{\"shard\":20}", "", 50, 7, "[{\"type\":\"gear\",\"key\":\"head_special_lunarWarriorHelm\",\"count\":1}]", 4, "questGroupMoon", false, "moon1", 0, "", null, null, null, null, null, null, null, "Lunar Battle, Part 1: Find the Mysterious Shards" },
                    { 87, 1.0, 100.0, 1.5, "unlockable", null, "", 275, 37, "[{\"type\":\"gear\",\"key\":\"armor_special_lunarWarriorArmor\",\"count\":1}]", 4, "questGroupMoon", true, "moon2", 0, "", "moon1", null, null, null, null, null, null, "Lunar Battle, Part 2: Stop the Overshadowing Stress" },
                    { 88, 1.0, 1000.0, 2.0, "unlockable", null, "", 650, 67, "[{\"type\":\"gear\",\"key\":\"weapon_special_lunarScythe\",\"count\":1}]", 4, "questGroupMoon", true, "moon3", 0, "", "moon2", null, null, null, null, null, null, "Lunar Battle, Part 3: The Monstrous Moon" },
                    { 89, 1.0, null, 1.0, "unlockable", "{\"moonstone\":100}", "", 100, 50, "[{\"type\":\"quests\",\"key\":\"moonstone2\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupMoonstone", false, "moonstone1", 60, "", null, null, null, null, null, null, null, "Recidivate, Part 1: The Moonstone Chain" },
                    { 90, 1.0, 1500.0, 3.0, "unlockable", null, "", 1000, 500, "[{\"type\":\"quests\",\"key\":\"moonstone3\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupMoonstone", true, "moonstone2", 60, "", "moonstone1", null, null, null, null, null, null, "Recidivate, Part 2: Recidivate the Necromancer" },
                    { 91, 1.0, 2000.0, 3.5, "unlockable", null, "", 1500, 900, "[{\"type\":\"hatchingPotions\",\"key\":\"Zombie\",\"count\":3},{\"type\":\"food\",\"key\":\"RottenMeat\",\"count\":5}]", 4, "questGroupMoonstone", true, "moonstone3", 60, "", "moonstone2", null, null, null, null, null, null, "Recidivate, Part 3: Recidivate Transformed" },
                    { 92, 1.0, 750.0, 1.5, "unlockable", null, "", 100, 20, "[{\"type\":\"quests\",\"key\":\"vice2\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupVice", true, "vice1", 30, "", null, null, null, null, null, null, null, "Vice, Part 1: Free Yourself of the Dragon's Influence" },
                    { 93, 1.0, null, 1.0, "unlockable", "{\"lightCrystal\":30}", "", 75, 20, "[{\"type\":\"quests\",\"key\":\"vice3\",\"count\":1,\"onlyOwner\":true}]", 4, "questGroupVice", false, "vice2", 30, "", "vice1", null, null, null, null, null, null, "Vice, Part 2: Find the Lair of the Wyrm" },
                    { 94, 1.0, 1500.0, 3.0, "unlockable", null, "", 1000, 100, "[{\"type\":\"eggs\",\"key\":\"Dragon\",\"count\":2},{\"type\":\"hatchingPotions\",\"key\":\"Shade\",\"count\":2}]", 4, "questGroupVice", true, "vice3", 30, "", "vice2", null, null, null, null, null, null, "Vice, Part 3: Vice Awakens" },
                    { 95, 1.0, null, 1.0, "gold", "{\"fireCoral\":20,\"blueFins\":20}", "", 75, 0, "[{\"type\":\"gear\",\"key\":\"armor_special_finnedOceanicArmor\",\"count\":1}]", 200, "questGroupDilatoryDistress", false, "dilatoryDistress1", 0, "", null, null, null, null, null, null, null, "Dilatory Distress, Part 1: Message in a Bottle" },
                    { 96, 1.0, 500.0, 1.0, "gold", null, "", 500, 0, "[{\"type\":\"hatchingPotions\",\"key\":\"Skeleton\",\"count\":1},{\"type\":\"gear\",\"key\":\"head_special_fireCoralCirclet\",\"count\":1}]", 300, "questGroupDilatoryDistress", true, "dilatoryDistress2", 0, "", "dilatoryDistress1", null, 0.29999999999999999, null, "Distress Rage", null, 50.0, "Dilatory Distress, Part 2: Creatures of the Crevasse" },
                    { 97, 1.0, 1000.0, 2.0, "gold", null, "", 650, 0, "[{\"type\":\"food\",\"key\":\"Fish\",\"count\":3},{\"type\":\"gear\",\"key\":\"weapon_special_tridentOfCrashingTides\",\"count\":1}]", 400, "questGroupDilatoryDistress", true, "dilatoryDistress3", 0, "", "dilatoryDistress2", null, null, null, null, null, null, "Dilatory Distress, Part 3: Not a Mere Maid" },
                    { 98, 1.0, 500.0, 1.0, "gold", null, "", 500, 0, "[{\"type\":\"hatchingPotions\",\"key\":\"Skeleton\",\"count\":1},{\"type\":\"gear\",\"key\":\"armor_special_roguishRainbowMessengerRobes\",\"count\":1}]", 200, "questGroupMayhemMistiflying", true, "mayhemMistiflying1", 0, "", null, null, 0.29999999999999999, null, "Mistiflying Rage", null, 50.0, "Mayhem in Mistiflying, Part 1: In Which Mistiflying Experiences a Dreadful Bother" },
                    { 99, 1.0, null, 1.0, "gold", "{\"mistifly1\":25,\"mistifly2\":15,\"mistifly3\":10}", "", 75, 0, "[{\"type\":\"gear\",\"key\":\"head_special_roguishRainbowMessengerHood\",\"count\":1}]", 300, "questGroupMayhemMistiflying", false, "mayhemMistiflying2", 0, "", "mayhemMistiflying1", null, null, null, null, null, null, "Mayhem in Mistiflying, Part 2: In Which the Wind Worsens" },
                    { 100, 1.0, 1000.0, 2.0, "gold", null, "", 650, 0, "[{\"type\":\"food\",\"key\":\"CottonCandyPink\",\"count\":3},{\"type\":\"gear\",\"key\":\"weapon_special_roguishRainbowMessage\",\"count\":1}]", 400, "questGroupMayhemMistiflying", true, "mayhemMistiflying3", 0, "", "mayhemMistiflying2", null, null, null, null, null, null, "Mayhem in Mistiflying, Part 3: In Which a Mailman is Extremely Rude" },
                    { 101, 1.0, 500.0, 1.0, "gold", null, "", 500, 0, "[{\"type\":\"hatchingPotions\",\"key\":\"Skeleton\",\"count\":1},{\"type\":\"gear\",\"key\":\"armor_special_mammothRiderArmor\",\"count\":1}]", 200, "questGroupStoikalmCalamity", true, "stoikalmCalamity1", 0, "", null, null, 0.29999999999999999, null, "Stoikalm Rage", null, 50.0, "Stoïkalm Calamity, Part 1: Earthen Enemies" },
                    { 102, 1.0, null, 1.0, "gold", "{\"icicleCoin\":40}", "", 75, 0, "[{\"type\":\"gear\",\"key\":\"head_special_mammothRiderHelm\",\"count\":1}]", 300, "questGroupStoikalmCalamity", false, "stoikalmCalamity2", 0, "", "stoikalmCalamity1", null, null, null, null, null, null, "Stoïkalm Calamity, Part 2: Seek the Icicle Caverns" },
                    { 103, 1.0, 1000.0, 2.0, "gold", null, "", 650, 0, "[{\"type\":\"food\",\"key\":\"CottonCandyBlue\",\"count\":3},{\"type\":\"gear\",\"key\":\"weapon_special_mammothRiderSpear\",\"count\":1}]", 400, "questGroupStoikalmCalamity", true, "stoikalmCalamity3", 0, "", "stoikalmCalamity2", null, null, null, null, null, null, "Stoïkalm Calamity, Part 3: Icicle Drake Quake" },
                    { 104, 1.0, 500.0, 1.0, "gold", null, "", 500, 0, "[{\"type\":\"hatchingPotions\",\"key\":\"Skeleton\",\"count\":1},{\"type\":\"gear\",\"key\":\"head_special_pyromancersTurban\",\"count\":1}]", 200, "questGroupTaskwoodsTerror", true, "taskwoodsTerror1", 0, "", null, null, 0.29999999999999999, null, "Taskwoods Rage", null, 50.0, "Terror in the Taskwoods, Part 1: The Blaze in the Taskwoods" },
                    { 105, 1.0, null, 1.0, "gold", "{\"pixie\":25,\"brownie\":15,\"dryad\":10}", "", 75, 0, "[{\"type\":\"gear\",\"key\":\"armor_special_pyromancersRobes\",\"count\":1}]", 300, "questGroupTaskwoodsTerror", false, "taskwoodsTerror2", 0, "", "taskwoodsTerror1", null, null, null, null, null, null, "Terror in the Taskwoods, Part 2: Finding the Flourishing Fairies" },
                    { 106, 1.0, 1000.0, 2.0, "gold", null, "", 650, 0, "[{\"type\":\"food\",\"key\":\"Strawberry\",\"count\":3},{\"type\":\"gear\",\"key\":\"weapon_special_taskwoodsLantern\",\"count\":1}]", 400, "questGroupTaskwoodsTerror", true, "taskwoodsTerror3", 0, "", "taskwoodsTerror2", null, null, null, null, null, null, "Terror in the Taskwoods, Part 3: Jacko of the Lantern" },
                    { 107, 1.0, null, 1.0, "gold", "{\"ancientTome\":40,\"forbiddenTome\":40,\"hiddenTome\":40}", "", 200, 0, "[{\"type\":\"food\",\"key\":\"Meat\",\"count\":3},{\"type\":\"food\",\"key\":\"Milk\",\"count\":3}]", 400, "questGroupLostMasterclasser", false, "lostMasterclasser1", 0, "", null, null, null, null, null, null, null, "The Mystery of the Masterclassers, Part 1: Read Between the Lines" },
                    { 108, 1.0, 1500.0, 2.5, "gold", null, "", 1500, 0, "[{\"type\":\"food\",\"key\":\"Chocolate\",\"count\":3},{\"type\":\"gear\",\"key\":\"eyewear_special_aetherMask\",\"count\":1}]", 500, "questGroupLostMasterclasser", true, "lostMasterclasser2", 0, "", "lostMasterclasser1", null, null, null, null, null, null, "The Mystery of the Masterclassers, Part 2: Assembling the a'Voidant" },
                    { 109, 1.0, 2000.0, 3.0, "gold", null, "", 2000, 0, "[{\"type\":\"hatchingPotions\",\"key\":\"Golden\",\"count\":1},{\"type\":\"hatchingPotions\",\"key\":\"Shade\",\"count\":1}]", 600, "questGroupLostMasterclasser", true, "lostMasterclasser3", 0, "", "lostMasterclasser2", null, 0.29999999999999999, null, "Masterclasser Rage", null, 25.0, "The Mystery of the Masterclassers, Part 3: City in the Sands" },
                    { 110, 1.0, 3000.0, 4.0, "gold", null, "", 3500, 0, "[{\"type\":\"gear\",\"key\":\"back_special_aetherCloak\",\"count\":1},{\"type\":\"gear\",\"key\":\"weapon_special_aetherCrystals\",\"count\":1}]", 700, "questGroupLostMasterclasser", true, "lostMasterclasser4", 0, "", "lostMasterclasser3", "Full Mana Drain", null, 1.0, "Masterclasser Rage", null, 15.0, "The Mystery of the Masterclassers, Part 4: The Lost Masterclasser" },
                    { 111, 1.0, null, 1.0, "timeTravelers", "{\"bolt\":15,\"gear\":10,\"spring\":10}", "", 75, 40, "[{\"type\":\"eggs\",\"key\":\"Robot\",\"count\":3}]", 0, null, false, "robot", 0, "", null, null, null, null, null, null, null, "Mysterious Mechanical Marvels!" },
                    { 112, 1.0, 1500.0, 2.5, "timeTravelers", null, "", 900, 90, "[{\"type\":\"hatchingPotions\",\"key\":\"SolarSystem\",\"count\":3}]", 0, null, true, "solarSystem", 0, "", null, null, null, null, null, null, null, "A Voyage of Cosmic Concentration" },
                    { 113, 1.0, 1000.0, 1.0, "timeTravelers", null, "", 425, 50, "[{\"type\":\"hatchingPotions\",\"key\":\"Windup\",\"count\":3}]", 0, null, true, "windup", 0, "", null, null, null, null, null, null, null, "A Whirl with a Wind-Up Warrior" },
                    { 114, 1.0, 100.0, 0.5, "unlockable", null, "", 42, 8, null, 100, null, true, "basilist", 0, "", null, null, null, null, null, null, null, "The Basi-List" },
                    { 115, 1.0, 100.0, 0.5, "unlockable", null, "", 42, 8, null, 1, null, true, "dustbunnies", 0, "", null, null, null, null, null, null, null, "The Feral Dust Bunnies" }
                });

            migrationBuilder.InsertData(
                table: "GameItems",
                columns: new[] { "Id", "GoldValue", "Icon", "IsDroppable", "Key", "Name", "Rarity", "Target", "Type" },
                values: new object[,]
                {
                    { 182, 4, "📜", false, "quest_alligator", "The Insta-Gator", 0, null, 3 },
                    { 183, 4, "📜", false, "quest_alpaca", "The Overpacked Alpaca", 0, null, 3 },
                    { 184, 4, "📜", false, "quest_armadillo", "The Indulgent Armadillo", 0, null, 3 },
                    { 185, 4, "📜", false, "quest_axolotl", "The Magical Axolotl", 0, null, 3 },
                    { 186, 4, "📜", false, "quest_badger", "Stop Badgering Me!", 0, null, 3 },
                    { 187, 4, "📜", false, "quest_beetle", "The CRITICAL BUG", 0, null, 3 },
                    { 188, 4, "📜", false, "quest_bunny", "The Killer Bunny", 0, null, 3 },
                    { 189, 4, "📜", false, "quest_butterfly", "Bye, Bye, Butterfry", 0, null, 3 },
                    { 190, 4, "📜", false, "quest_cat", "A Purrplexing Predicament", 0, null, 3 },
                    { 191, 4, "📜", false, "quest_chameleon", "The Chaotic Chameleon", 0, null, 3 },
                    { 192, 4, "📜", false, "quest_cheetah", "Such a Cheetah", 0, null, 3 },
                    { 193, 4, "📜", false, "quest_cow", "The Mootant Cow", 0, null, 3 },
                    { 194, 4, "📜", false, "quest_crab", "The Fiddling Crab", 0, null, 3 },
                    { 195, 4, "📜", false, "quest_dilatory_derby", "The Dilatory Derby", 0, null, 3 },
                    { 196, 4, "📜", false, "quest_dog", "Triple Dog Dare!", 0, null, 3 },
                    { 197, 4, "📜", false, "quest_dolphin", "The Dolphin of Doubt", 0, null, 3 },
                    { 198, 4, "📜", false, "quest_falcon", "The Birds of Preycrastination", 0, null, 3 },
                    { 199, 4, "📜", false, "quest_ferret", "The Nefarious Ferret", 0, null, 3 },
                    { 200, 4, "📜", false, "quest_frog", "Swamp of the Clutter Frog", 0, null, 3 },
                    { 201, 4, "📜", false, "quest_ghost_stag", "The Spirit of Spring", 0, null, 3 },
                    { 202, 4, "📜", false, "quest_giraffe", "The Gear-affe", 0, null, 3 },
                    { 203, 4, "📜", false, "quest_gryphon", "The Fiery Gryphon", 0, null, 3 },
                    { 204, 4, "📜", false, "quest_guineapig", "The Guinea Pig Gang", 0, null, 3 },
                    { 205, 4, "📜", false, "quest_harpy", "Help! Harpy!", 0, null, 3 },
                    { 206, 4, "📜", false, "quest_hedgehog", "The Hedgebeast", 0, null, 3 },
                    { 207, 4, "📜", false, "quest_hippo", "What a Hippo-Crite", 0, null, 3 },
                    { 208, 4, "📜", false, "quest_horse", "Ride the Night-Mare", 0, null, 3 },
                    { 209, 4, "📜", false, "quest_kangaroo", "Kangaroo Catastrophe", 0, null, 3 },
                    { 210, 4, "📜", false, "quest_kraken", "The Kraken of Inkomplete", 0, null, 3 },
                    { 211, 4, "📜", false, "quest_monkey", "Monstrous Mandrill", 0, null, 3 },
                    { 212, 4, "📜", false, "quest_nudibranch", "Infestation of the NowDo Nudibranchs", 0, null, 3 },
                    { 213, 4, "📜", false, "quest_octopus", "The Call of Octothulu", 0, null, 3 },
                    { 214, 4, "📜", false, "quest_otter", "The Perfidious Plotter!", 0, null, 3 },
                    { 215, 4, "📜", false, "quest_owl", "The Night-Owl", 0, null, 3 },
                    { 216, 4, "📜", false, "quest_peacock", "The Push-and-Pull Peacock", 0, null, 3 },
                    { 217, 4, "📜", false, "quest_penguin", "The Fowl Frost", 0, null, 3 },
                    { 218, 4, "📜", false, "quest_platypus", "The Perfectionist Platypus", 0, null, 3 },
                    { 219, 4, "📜", false, "quest_pterodactyl", "The Pterror-dactyl", 0, null, 3 },
                    { 220, 4, "📜", false, "quest_raccoon", "Raccoon Tycoon", 0, null, 3 },
                    { 221, 4, "📜", false, "quest_rat", "The Rat King", 0, null, 3 },
                    { 222, 4, "📜", false, "quest_rock", "Escape the Cave Creature", 0, null, 3 },
                    { 223, 4, "📜", false, "quest_rooster", "Rooster Rampage", 0, null, 3 },
                    { 224, 4, "📜", false, "quest_sabretooth", "The Sabre Cat", 0, null, 3 },
                    { 225, 4, "📜", false, "quest_seaserpent", "Sea Serpent Strike!", 0, null, 3 },
                    { 226, 4, "📜", false, "quest_sheep", "The Thunder Ram", 0, null, 3 },
                    { 227, 4, "📜", false, "quest_slime", "The Jelly Regent", 0, null, 3 },
                    { 228, 4, "📜", false, "quest_sloth", "The Somnolent Sloth", 0, null, 3 },
                    { 229, 4, "📜", false, "quest_snail", "The Snail of Drudgery Sludge", 0, null, 3 },
                    { 230, 4, "📜", false, "quest_snake", "The Serpent of Distraction", 0, null, 3 },
                    { 231, 4, "📜", false, "quest_spider", "The Icy Arachnid", 0, null, 3 },
                    { 232, 4, "📜", false, "quest_squirrel", "The Sneaky Squirrel", 0, null, 3 },
                    { 233, 4, "📜", false, "quest_treeling", "The Tangle Tree", 0, null, 3 },
                    { 234, 4, "📜", false, "quest_trex", "King of the Dinosaurs", 0, null, 3 },
                    { 235, 4, "📜", false, "quest_trex_undead", "The Dinosaur Unearthed", 0, null, 3 },
                    { 236, 4, "📜", false, "quest_triceratops", "The Trampling Triceratops", 0, null, 3 },
                    { 237, 4, "📜", false, "quest_turtle", "Guide the Turtle", 0, null, 3 },
                    { 238, 4, "📜", false, "quest_unicorn", "Convincing the Unicorn Queen", 0, null, 3 },
                    { 239, 4, "📜", false, "quest_velociraptor", "The Veloci-Rapper", 0, null, 3 },
                    { 240, 4, "📜", false, "quest_whale", "Wail of the Whale", 0, null, 3 },
                    { 241, 4, "📜", false, "quest_yarn", "A Tangled Yarn", 0, null, 3 },
                    { 242, 4, "📜", false, "quest_amber", "The Amber Alliance", 0, null, 3 },
                    { 243, 4, "📜", false, "quest_blackPearl", "A Startling Starry Idea", 0, null, 3 },
                    { 244, 4, "📜", false, "quest_bronze", "Brazen Beetle Battle", 0, null, 3 },
                    { 245, 4, "📜", false, "quest_fluorite", "A Bright Fluorite Fright", 0, null, 3 },
                    { 246, 4, "📜", false, "quest_jade", "A Jaded Jinx", 0, null, 3 },
                    { 247, 4, "📜", false, "quest_pinkMarble", "Calm the Corrupted Cupid", 0, null, 3 },
                    { 248, 4, "📜", false, "quest_onyx", "The Onyx Odyssey", 0, null, 3 },
                    { 249, 4, "📜", false, "quest_ruby", "Ruby Rapport", 0, null, 3 },
                    { 250, 4, "📜", false, "quest_silver", "The Silver Solution", 0, null, 3 },
                    { 251, 4, "📜", false, "quest_stone", "A Maze of Moss", 0, null, 3 },
                    { 252, 4, "📜", false, "quest_turquoise", "Turquoise Treasure Toil", 0, null, 3 },
                    { 253, 4, "📜", false, "quest_opal", "The Legend of the Obscure Opals", 0, null, 3 },
                    { 254, 4, "📜", false, "quest_evilsanta", "Trapper Santa", 0, null, 3 },
                    { 255, 4, "📜", false, "quest_evilsanta2", "Find the Cub", 0, null, 3 },
                    { 256, 1, "📜", false, "quest_egg", "Egg Hunt", 0, null, 3 },
                    { 257, 4, "📜", false, "quest_waffle", "Waffling with the Fool", 0, null, 3 },
                    { 258, 4, "📜", false, "quest_virtualpet", "Virtual Mayhem with the April Fool", 0, null, 3 },
                    { 259, 4, "📜", false, "quest_fungi", "The Moody Mushroom", 0, null, 3 },
                    { 260, 4, "📜", false, "quest_alien", "Invasion of the Motivation Snatchers", 0, null, 3 },
                    { 261, 4, "📜", false, "quest_atom1", "Attack of the Mundane, Part 1", 0, null, 3 },
                    { 262, 4, "📜", false, "quest_atom2", "Attack of the Mundane, Part 2", 0, null, 3 },
                    { 263, 4, "📜", false, "quest_atom3", "Attack of the Mundane, Part 3", 0, null, 3 },
                    { 264, 4, "📜", false, "quest_goldenknight1", "The Golden Knight, Part 1", 0, null, 3 },
                    { 265, 4, "📜", false, "quest_goldenknight2", "The Golden Knight, Part 2", 0, null, 3 },
                    { 266, 4, "📜", false, "quest_goldenknight3", "The Golden Knight, Part 3", 0, null, 3 },
                    { 267, 4, "📜", false, "quest_moon1", "Lunar Battle, Part 1", 0, null, 3 },
                    { 268, 4, "📜", false, "quest_moon2", "Lunar Battle, Part 2", 0, null, 3 },
                    { 269, 4, "📜", false, "quest_moon3", "Lunar Battle, Part 3", 0, null, 3 },
                    { 270, 4, "📜", false, "quest_moonstone1", "Recidivate, Part 1", 0, null, 3 },
                    { 271, 4, "📜", false, "quest_moonstone2", "Recidivate, Part 2", 0, null, 3 },
                    { 272, 4, "📜", false, "quest_moonstone3", "Recidivate, Part 3", 0, null, 3 },
                    { 273, 4, "📜", false, "quest_vice1", "Vice, Part 1", 0, null, 3 },
                    { 274, 4, "📜", false, "quest_vice2", "Vice, Part 2", 0, null, 3 },
                    { 275, 4, "📜", false, "quest_vice3", "Vice, Part 3", 0, null, 3 },
                    { 276, 200, "📜", false, "quest_dilatoryDistress1", "Dilatory Distress, Part 1", 2, null, 3 },
                    { 277, 300, "📜", false, "quest_dilatoryDistress2", "Dilatory Distress, Part 2", 2, null, 3 },
                    { 278, 400, "📜", false, "quest_dilatoryDistress3", "Dilatory Distress, Part 3", 2, null, 3 },
                    { 279, 200, "📜", false, "quest_mayhemMistiflying1", "Mayhem in Mistiflying, Part 1", 2, null, 3 },
                    { 280, 300, "📜", false, "quest_mayhemMistiflying2", "Mayhem in Mistiflying, Part 2", 2, null, 3 },
                    { 281, 400, "📜", false, "quest_mayhemMistiflying3", "Mayhem in Mistiflying, Part 3", 2, null, 3 },
                    { 282, 200, "📜", false, "quest_stoikalmCalamity1", "Stoïkalm Calamity, Part 1", 2, null, 3 },
                    { 283, 300, "📜", false, "quest_stoikalmCalamity2", "Stoïkalm Calamity, Part 2", 2, null, 3 },
                    { 284, 400, "📜", false, "quest_stoikalmCalamity3", "Stoïkalm Calamity, Part 3", 2, null, 3 },
                    { 285, 200, "📜", false, "quest_taskwoodsTerror1", "Terror in the Taskwoods, Part 1", 2, null, 3 },
                    { 286, 300, "📜", false, "quest_taskwoodsTerror2", "Terror in the Taskwoods, Part 2", 2, null, 3 },
                    { 287, 400, "📜", false, "quest_taskwoodsTerror3", "Terror in the Taskwoods, Part 3", 2, null, 3 },
                    { 288, 400, "📜", false, "quest_lostMasterclasser1", "Mystery of the Masterclassers, Part 1", 2, null, 3 },
                    { 289, 500, "📜", false, "quest_lostMasterclasser2", "Mystery of the Masterclassers, Part 2", 2, null, 3 },
                    { 290, 600, "📜", false, "quest_lostMasterclasser3", "Mystery of the Masterclassers, Part 3", 2, null, 3 },
                    { 291, 700, "📜", false, "quest_lostMasterclasser4", "Mystery of the Masterclassers, Part 4", 2, null, 3 },
                    { 292, 0, "📜", false, "quest_robot", "Mysterious Mechanical Marvels!", 3, null, 3 },
                    { 293, 0, "📜", false, "quest_solarSystem", "A Voyage of Cosmic Concentration", 3, null, 3 },
                    { 294, 0, "📜", false, "quest_windup", "A Whirl with a Wind-Up Warrior", 3, null, 3 },
                    { 295, 100, "📜", false, "quest_basilist", "The Basi-List", 1, null, 3 },
                    { 296, 1, "📜", false, "quest_dustbunnies", "The Feral Dust Bunnies", 0, null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BossQuests_Key",
                table: "BossQuests",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyQuestMembers_PartyQuestId_UserId",
                table: "PartyQuestMembers",
                columns: new[] { "PartyQuestId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyQuestMembers_UserId",
                table: "PartyQuestMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyQuests_BossQuestId",
                table: "PartyQuests",
                column: "BossQuestId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyQuests_LeaderUserId",
                table: "PartyQuests",
                column: "LeaderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyQuests_PartyId",
                table: "PartyQuests",
                column: "PartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartyQuestMembers");

            migrationBuilder.DropTable(
                name: "PartyQuests");

            migrationBuilder.DropTable(
                name: "BossQuests");

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 294);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 296);
        }
    }
}
