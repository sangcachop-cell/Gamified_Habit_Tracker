using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddHideoutUpgradeSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpgradeStartedAt",
                table: "UserFacilities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "Id", "BuffDescription", "BuffPerLevel", "Description", "Icon", "IsActive", "MaxLevel", "Name", "StatAffected" },
                values: new object[] { 6, "+30 slots per level", 30, "Expand your hideout's storage capacity. Each upgrade adds 30 more grid slots (10×3).", "📦", true, 5, "Storage Room", "Storage" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "UpgradeStartedAt",
                table: "UserFacilities");
        }
    }
}
