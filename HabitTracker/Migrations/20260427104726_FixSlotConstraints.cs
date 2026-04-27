using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class FixSlotConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE UserInventoryItems
                SET ContainerType = 'Storage', GridX = 0, GridY = 0
                WHERE ContainerType IN ('Backpack', 'EquippedRig');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder) { }
    }
}
