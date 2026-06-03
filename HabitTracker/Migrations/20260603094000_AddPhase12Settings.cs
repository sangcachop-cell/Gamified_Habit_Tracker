using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase12Settings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiToken",
                table: "Users",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayStart",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PMPermission",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileVisibility",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SuppressNotifications",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DayStart",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PMPermission",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuppressNotifications",
                table: "Users");
        }
    }
}
