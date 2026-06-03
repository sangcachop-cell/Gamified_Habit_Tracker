using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase13Admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMuted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AdminBlocklistEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedByAdminId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminBlocklistEntries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminBlocklistEntries");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsMuted",
                table: "Users");
        }
    }
}
