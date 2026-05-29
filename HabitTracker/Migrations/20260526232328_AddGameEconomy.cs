using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddGameEconomy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CON",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DailyDropCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Gold",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HP",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "INT",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSleeping",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDropResetDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Mana",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PER",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "STR",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GameItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Rarity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ObtainedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInventoryItems_GameItems_GameItemId",
                        column: x => x.GameItemId,
                        principalTable: "GameItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInventoryItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GameItems",
                columns: new[] { "Id", "Icon", "Key", "Name", "Rarity", "Type" },
                values: new object[,]
                {
                    { 1, "🥩", "food_Meat", "Meat", 0, 0 },
                    { 2, "🍓", "food_Strawberry", "Strawberry", 0, 0 },
                    { 3, "🥔", "food_Potato", "Potato", 0, 0 },
                    { 4, "🍫", "food_Chocolate", "Chocolate", 0, 0 },
                    { 5, "🐟", "food_Fish", "Fish", 0, 0 },
                    { 6, "🍯", "food_Honey", "Honey", 0, 0 },
                    { 7, "🥚", "egg_Wolf", "Wolf Egg", 0, 1 },
                    { 8, "🥚", "egg_Bear", "Bear Egg", 0, 1 },
                    { 9, "🌵", "egg_Cactus", "Cactus Egg", 0, 1 },
                    { 10, "🐉", "egg_Dragon", "Dragon Egg", 0, 1 },
                    { 11, "🦎", "egg_Axolotl", "Axolotl Egg", 0, 1 },
                    { 12, "🧪", "potion_Base", "Base Potion", 0, 2 },
                    { 13, "🤍", "potion_White", "White Potion", 0, 2 },
                    { 14, "🏜️", "potion_Desert", "Desert Potion", 0, 2 },
                    { 15, "❤️", "potion_Red", "Red Potion", 1, 2 },
                    { 16, "🖤", "potion_Shade", "Shade Potion", 1, 2 },
                    { 17, "💀", "potion_Skeleton", "Skeleton Potion", 1, 2 },
                    { 18, "🧟", "potion_Zombie", "Zombie Potion", 2, 2 },
                    { 19, "🩷", "potion_CottonCandyPink", "Cotton Candy Pink Potion", 2, 2 },
                    { 20, "💙", "potion_CottonCandyBlue", "Cotton Candy Blue Potion", 2, 2 },
                    { 21, "✨", "potion_Golden", "Golden Potion", 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInventoryItems_GameItemId",
                table: "UserInventoryItems",
                column: "GameItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInventoryItems_UserId_GameItemId",
                table: "UserInventoryItems",
                columns: new[] { "UserId", "GameItemId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInventoryItems");

            migrationBuilder.DropTable(
                name: "GameItems");

            migrationBuilder.DropColumn(
                name: "CON",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DailyDropCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HP",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "INT",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsSleeping",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastDropResetDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Mana",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PER",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "STR",
                table: "Users");
        }
    }
}
