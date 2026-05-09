using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkbench : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "Id", "BuffDescription", "BuffPerLevel", "Description", "Icon", "IsActive", "MaxLevel", "Name", "StatAffected" },
                values: new object[] { 7, "+1 craft slot per level", 1, "A crafting station for processing raw materials. Higher levels unlock more slots and recipes.", "🔨", true, 5, "Workbench", "Crafting" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
