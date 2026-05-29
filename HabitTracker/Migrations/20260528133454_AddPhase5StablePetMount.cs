using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase5StablePetMount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveMountKey",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivePetKey",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Target",
                table: "GameItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PetKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FeedingPoints = table.Column<int>(type: "int", nullable: false),
                    IsMount = table.Column<bool>(type: "bit", nullable: false),
                    HatchedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Target",
                value: "Base");

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "Target",
                value: "Red");

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "Target",
                value: "Desert");

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "Target",
                value: "Shade");

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "Target",
                value: "Skeleton");

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "Target",
                value: "Golden");

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 14,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 16,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 18,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 20,
                column: "Target",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "Target",
                value: null);

            migrationBuilder.InsertData(
                table: "GameItems",
                columns: new[] { "Id", "GoldValue", "Icon", "Key", "Name", "Rarity", "Target", "Type" },
                values: new object[,]
                {
                    { 22, 1, "🥛", "food_Milk", "Milk", 0, "White", 0 },
                    { 23, 1, "🍖", "food_RottenMeat", "Rotten Meat", 0, "Zombie", 0 },
                    { 24, 1, "🍬", "food_CottonCandyPink", "Cotton Candy Pink", 0, "CottonCandyPink", 0 },
                    { 25, 1, "🍬", "food_CottonCandyBlue", "Cotton Candy Blue", 0, "CottonCandyBlue", 0 },
                    { 26, 3, "🥚", "egg_TigerCub", "Tiger Cub Egg", 0, null, 1 },
                    { 27, 3, "🥚", "egg_PandaCub", "Panda Cub Egg", 0, null, 1 },
                    { 28, 3, "🥚", "egg_LionCub", "Lion Cub Egg", 0, null, 1 },
                    { 29, 3, "🦊", "egg_Fox", "Fox Egg", 0, null, 1 },
                    { 30, 3, "🐷", "egg_FlyingPig", "Flying Pig Egg", 0, null, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPets_UserId_IsMount",
                table: "UserPets",
                columns: new[] { "UserId", "IsMount" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPets_UserId_PetKey",
                table: "UserPets",
                columns: new[] { "UserId", "PetKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPets");

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DropColumn(
                name: "ActiveMountKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ActivePetKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "GameItems");
        }
    }
}
