using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddBossQuestGemCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GemCost",
                table: "BossQuests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 7, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 7, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 9, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 7, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 9, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 7, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 7, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 9, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 8, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 95,
                column: "GemCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 96,
                column: "GemCost",
                value: 5);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 97,
                column: "GemCost",
                value: 6);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 98,
                column: "GemCost",
                value: 5);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 99,
                column: "GemCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 100,
                column: "GemCost",
                value: 6);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 101,
                column: "GemCost",
                value: 5);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 102,
                column: "GemCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 103,
                column: "GemCost",
                value: 6);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 104,
                column: "GemCost",
                value: 5);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 105,
                column: "GemCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 106,
                column: "GemCost",
                value: 6);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 107,
                column: "GemCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 108,
                column: "GemCost",
                value: 8);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 109,
                column: "GemCost",
                value: 9);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 110,
                column: "GemCost",
                value: 11);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 111,
                column: "GemCost",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 112,
                column: "GemCost",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 113,
                column: "GemCost",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "GemCost", "GoldCost" },
                values: new object[] { 4, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GemCost",
                table: "BossQuests");

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 1,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 2,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 3,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 4,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 5,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 6,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 7,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 8,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 9,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 10,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 11,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 12,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 13,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 14,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 15,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 16,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 17,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 18,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 19,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 20,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 21,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 22,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 23,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 24,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 25,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 26,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 27,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 28,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 29,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 30,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 31,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 32,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 33,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 34,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 35,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 36,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 37,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 38,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 39,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 40,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 41,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 42,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 43,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 44,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 45,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 46,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 47,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 48,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 49,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 50,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 51,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 52,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 53,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 54,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 55,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 56,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 57,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 58,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 59,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 60,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 61,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 62,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 63,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 64,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 65,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 66,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 67,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 68,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 69,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 70,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 71,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 72,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 73,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 74,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 75,
                column: "GoldCost",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 76,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 77,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 78,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 79,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 80,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 81,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 82,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 83,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 84,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 85,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 86,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 87,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 88,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 89,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 90,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 91,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 92,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 93,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 94,
                column: "GoldCost",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 114,
                column: "GoldCost",
                value: 100);

            migrationBuilder.UpdateData(
                table: "BossQuests",
                keyColumn: "Id",
                keyValue: 115,
                column: "GoldCost",
                value: 1);
        }
    }
}
