using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase4InventoryShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gems",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsArmoire",
                table: "GearItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GoldValue",
                table: "GameItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "GoldValue",
                value: 1);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "GoldValue",
                value: 1);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "GoldValue",
                value: 1);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "GoldValue",
                value: 1);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "GoldValue",
                value: 1);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "GoldValue",
                value: 1);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "GoldValue",
                value: 2);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "GoldValue",
                value: 2);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 14,
                column: "GoldValue",
                value: 2);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 16,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "GoldValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 18,
                column: "GoldValue",
                value: 5);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "GoldValue",
                value: 5);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 20,
                column: "GoldValue",
                value: 5);

            migrationBuilder.UpdateData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "GoldValue",
                value: 10);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsArmoire",
                value: true);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 16,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 18,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 20,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 22,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsArmoire",
                value: true);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 24,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 25,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 26,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 27,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 28,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 29,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 30,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 31,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 32,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 33,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 34,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 35,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 36,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 37,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 38,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 39,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "IsArmoire",
                value: true);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 41,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 42,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 43,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 44,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 45,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 46,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 47,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 48,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 49,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 50,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 51,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 52,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 53,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 54,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 55,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 56,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 57,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 58,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 59,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 60,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 61,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 62,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 63,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 64,
                column: "IsArmoire",
                value: true);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 65,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 66,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 67,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 68,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 69,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 70,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 71,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 72,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 73,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 74,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 75,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 76,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 77,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 78,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 79,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 80,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 81,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 82,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 83,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 84,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 85,
                column: "IsArmoire",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gems",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsArmoire",
                table: "GearItems");

            migrationBuilder.DropColumn(
                name: "GoldValue",
                table: "GameItems");
        }
    }
}
