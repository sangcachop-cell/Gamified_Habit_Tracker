using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddTapTheDucQuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Category", "CategoryId", "CreatedAt", "Description", "Difficulty", "FacilityId", "Frequency", "Icon", "IsActive", "Name", "TimesCompleted", "UpdatedAt", "XPReward" },
                values: new object[] { 1, "Sức khỏe", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", 1, "Daily", "🏋️", true, "Tập thể dục", 0, null, 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
