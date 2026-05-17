using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddDocSachQuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Category", "CategoryId", "CreatedAt", "Description", "Difficulty", "FacilityId", "Frequency", "Icon", "IsActive", "MinigameType", "Name", "TimesCompleted", "UpdatedAt", "XPReward" },
                values: new object[] { 3, "Học tập", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Medium", 3, "Daily", "📖", true, "Tetris", "Đọc sách", 0, null, 25 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
