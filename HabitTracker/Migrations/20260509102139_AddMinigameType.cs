using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddMinigameType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MinigameType",
                table: "Quests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "MinigameType",
                value: "QTE");

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Category", "CategoryId", "CreatedAt", "Description", "Difficulty", "FacilityId", "Frequency", "Icon", "IsActive", "MinigameType", "Name", "TimesCompleted", "UpdatedAt", "XPReward" },
                values: new object[] { 2, "Sức khỏe", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", 4, "Daily", "🏃", true, "Dino", "Chạy bộ", 0, null, 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "MinigameType",
                table: "Quests");
        }
    }
}
