using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddWackyPotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameItems",
                columns: new[] { "Id", "GoldValue", "Icon", "IsDroppable", "Key", "Name", "Rarity", "Target", "Type" },
                values: new object[,]
                {
                    { 175, 0, "🥦", false, "potion_Veggie", "Veggie Potion", 2, null, 2 },
                    { 176, 0, "🍰", false, "potion_Dessert", "Dessert Potion", 2, null, 2 },
                    { 177, 0, "🎮", false, "potion_VirtualPet", "Virtual Pet Potion", 2, null, 2 },
                    { 178, 0, "🍄", false, "potion_Fungi", "Fungi Potion", 2, null, 2 },
                    { 179, 0, "👾", false, "potion_Cryptid", "Cryptid Potion", 2, null, 2 },
                    { 180, 0, "👽", false, "potion_Alien", "Alien Potion", 2, null, 2 },
                    { 181, 0, "🤖", false, "potion_Windup", "Windup Potion", 2, null, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 181);
        }
    }
}
