using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class SeedBreadAndWater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO UserInventoryItems (UserId, ItemId, ContainerType, GridX, GridY, IsRotated, AcquiredAt)
                SELECT u.Id, 'bread', 'Storage', 0, 0, 0, GETDATE()
                FROM Users u
                WHERE NOT EXISTS (
                    SELECT 1 FROM UserInventoryItems i WHERE i.UserId = u.Id AND i.ItemId = 'bread'
                );

                INSERT INTO UserInventoryItems (UserId, ItemId, ContainerType, GridX, GridY, IsRotated, AcquiredAt)
                SELECT u.Id, 'water_bottle', 'Storage', 1, 0, 0, GETDATE()
                FROM Users u
                WHERE NOT EXISTS (
                    SELECT 1 FROM UserInventoryItems i WHERE i.UserId = u.Id AND i.ItemId = 'water_bottle'
                );
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM UserInventoryItems WHERE ItemId IN ('bread', 'water_bottle');
            ");
        }
    }
}
