using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddHideoutSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StatAffected = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BuffDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BuffPerLevel = table.Column<int>(type: "int", nullable: false),
                    MaxLevel = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFacilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFacilities_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFacilities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "Id", "BuffDescription", "BuffPerLevel", "Description", "Icon", "IsActive", "MaxLevel", "Name", "StatAffected" },
                values: new object[,]
                {
                    { 1, "+5 ATK per level", 5, "A dedicated space for physical conditioning. Forges raw combat power through daily drills.", "🏋️", true, 5, "Training Grounds", "ATK" },
                    { 2, "+20 HP per level", 20, "Silence the mind, fortify the spirit. Meditative practice expands your life force.", "🧘", true, 5, "Meditation Hall", "HP" },
                    { 3, "+2% XP Gain per level", 2, "Ancient texts and ongoing studies. Knowledge accelerates growth and sharpens the mind.", "📚", true, 5, "Archive", "XPGain" },
                    { 4, "+10 Stamina per level", 10, "Obstacle runs and reflex drills push your body to its limits. Speed is half the battle.", "🏃", true, 5, "Agility Course", "Stamina" },
                    { 5, "+5 Armor per level", 5, "Hardened defenses line the outer walls. A stalwart position from which to weather any storm.", "🛡️", true, 5, "Barracks", "Armor" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFacilities_FacilityId",
                table: "UserFacilities",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFacilities_UserId_FacilityId",
                table: "UserFacilities",
                columns: new[] { "UserId", "FacilityId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFacilities");

            migrationBuilder.DropTable(
                name: "Facilities");
        }
    }
}
