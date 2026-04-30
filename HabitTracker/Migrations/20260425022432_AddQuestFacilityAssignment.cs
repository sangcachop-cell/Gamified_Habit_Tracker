using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestFacilityAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "Quests",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "FacilityId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "FacilityId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "FacilityId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "FacilityId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                column: "FacilityId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 6,
                column: "FacilityId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 7,
                column: "FacilityId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 8,
                column: "FacilityId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 9,
                column: "FacilityId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 10,
                column: "FacilityId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 11,
                column: "FacilityId",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_FacilityId",
                table: "Quests",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Facilities_FacilityId",
                table: "Quests",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Facilities_FacilityId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_FacilityId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "Quests");
        }
    }
}
