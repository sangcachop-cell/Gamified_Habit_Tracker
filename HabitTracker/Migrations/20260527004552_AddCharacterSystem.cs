using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BodyType",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BuffCON",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BuffExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuffINT",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuffPER",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuffSTR",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeArmor",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeBack",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeBody",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeEyewear",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeHead",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeHeadAccessory",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CostumeModeEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CostumeShield",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeWeapon",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedArmor",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedBack",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedBody",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedEyewear",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedHead",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedHeadAccessory",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedShield",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquippedWeapon",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HairBangs",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HairBeard",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HairColor",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HairMustache",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HairStyle",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShirtStyle",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkinColor",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatPoints",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GearItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slot = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GearClass = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    GoldCost = table.Column<int>(type: "int", nullable: false),
                    STR = table.Column<int>(type: "int", nullable: false),
                    CON = table.Column<int>(type: "int", nullable: false),
                    INT = table.Column<int>(type: "int", nullable: false),
                    PER = table.Column<int>(type: "int", nullable: false),
                    TwoHanded = table.Column<bool>(type: "bit", nullable: false),
                    IsSpecial = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GearItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGearItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GearItemId = table.Column<int>(type: "int", nullable: false),
                    AcquiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGearItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGearItems_GearItems_GearItemId",
                        column: x => x.GearItemId,
                        principalTable: "GearItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGearItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GearItems",
                columns: new[] { "Id", "CON", "GearClass", "GoldCost", "INT", "IsSpecial", "Key", "Name", "PER", "STR", "Slot", "Tier", "TwoHanded" },
                values: new object[,]
                {
                    { 1, 0, "warrior", 0, 0, false, "weapon_warrior_0", "Worn Hatchet", 0, 0, "weapon", 0, false },
                    { 2, 1, "warrior", 25, 0, false, "weapon_warrior_1", "Iron Sword", 0, 3, "weapon", 1, false },
                    { 3, 2, "warrior", 50, 0, false, "weapon_warrior_2", "Steel Sword", 0, 6, "weapon", 2, false },
                    { 4, 3, "warrior", 75, 0, false, "weapon_warrior_3", "Battle Axe", 0, 9, "weapon", 3, false },
                    { 5, 4, "warrior", 100, 0, false, "weapon_warrior_4", "Dragon Sword", 0, 12, "weapon", 4, false },
                    { 6, 5, "warrior", 125, 0, false, "weapon_warrior_5", "Legendary Blade", 0, 15, "weapon", 5, false },
                    { 7, 2, "warrior", 20, 0, false, "armor_warrior_1", "Chain Mail", 0, 2, "armor", 1, false },
                    { 8, 4, "warrior", 40, 0, false, "armor_warrior_2", "Plate Armor", 0, 4, "armor", 2, false },
                    { 9, 6, "warrior", 60, 0, false, "armor_warrior_3", "Dragon Scale", 0, 6, "armor", 3, false },
                    { 10, 8, "warrior", 80, 0, false, "armor_warrior_4", "Titan Plate", 0, 8, "armor", 4, false },
                    { 11, 10, "warrior", 100, 0, false, "armor_warrior_5", "Legendary Armor", 0, 10, "armor", 5, false },
                    { 12, 1, "warrior", 15, 0, false, "head_warrior_1", "Iron Helm", 0, 2, "head", 1, false },
                    { 13, 2, "warrior", 30, 0, false, "head_warrior_2", "Steel Helm", 0, 4, "head", 2, false },
                    { 14, 3, "warrior", 45, 0, false, "head_warrior_3", "Dragon Helm", 0, 6, "head", 3, false },
                    { 15, 4, "warrior", 60, 0, false, "head_warrior_4", "Titan Helm", 0, 8, "head", 4, false },
                    { 16, 5, "warrior", 75, 0, false, "head_warrior_5", "Legendary Helm", 0, 10, "head", 5, false },
                    { 17, 2, "warrior", 15, 0, false, "shield_warrior_1", "Wooden Shield", 0, 0, "shield", 1, false },
                    { 18, 4, "warrior", 30, 0, false, "shield_warrior_2", "Iron Shield", 0, 0, "shield", 2, false },
                    { 19, 6, "warrior", 45, 0, false, "shield_warrior_3", "Steel Shield", 0, 0, "shield", 3, false },
                    { 20, 8, "warrior", 60, 0, false, "shield_warrior_4", "Dragon Shield", 0, 0, "shield", 4, false },
                    { 21, 10, "warrior", 75, 0, false, "shield_warrior_5", "Legendary Shield", 0, 0, "shield", 5, false },
                    { 22, 0, "mage", 0, 0, false, "weapon_wizard_0", "Broken Branch", 0, 0, "weapon", 0, false },
                    { 23, 0, "mage", 25, 3, false, "weapon_wizard_1", "Apprentice Wand", 1, 0, "weapon", 1, false },
                    { 24, 0, "mage", 50, 6, false, "weapon_wizard_2", "Journeyman Staff", 2, 0, "weapon", 2, false },
                    { 25, 0, "mage", 75, 9, false, "weapon_wizard_3", "Mage Staff", 3, 0, "weapon", 3, false },
                    { 26, 0, "mage", 100, 12, false, "weapon_wizard_4", "Arcane Staff", 4, 0, "weapon", 4, false },
                    { 27, 0, "mage", 125, 15, false, "weapon_wizard_5", "Legendary Arcane Staff", 5, 0, "weapon", 5, true },
                    { 28, 0, "mage", 20, 2, false, "armor_wizard_1", "Initiate Robes", 2, 0, "armor", 1, false },
                    { 29, 0, "mage", 40, 4, false, "armor_wizard_2", "Apprentice Robes", 4, 0, "armor", 2, false },
                    { 30, 0, "mage", 60, 6, false, "armor_wizard_3", "Journeyman Robes", 6, 0, "armor", 3, false },
                    { 31, 0, "mage", 80, 8, false, "armor_wizard_4", "Mage Robes", 8, 0, "armor", 4, false },
                    { 32, 0, "mage", 100, 10, false, "armor_wizard_5", "Legendary Robes", 10, 0, "armor", 5, false },
                    { 33, 0, "mage", 15, 2, false, "head_wizard_1", "Cloth Cap", 1, 0, "head", 1, false },
                    { 34, 0, "mage", 30, 4, false, "head_wizard_2", "Apprentice Hat", 2, 0, "head", 2, false },
                    { 35, 0, "mage", 45, 6, false, "head_wizard_3", "Journeyman Hat", 3, 0, "head", 3, false },
                    { 36, 0, "mage", 60, 8, false, "head_wizard_4", "Mage Hat", 4, 0, "head", 4, false },
                    { 37, 0, "mage", 75, 10, false, "head_wizard_5", "Legendary Hat", 5, 0, "head", 5, false },
                    { 38, 0, "mage", 15, 0, false, "shield_wizard_1", "Orb of Protection", 2, 0, "shield", 1, false },
                    { 39, 0, "mage", 30, 0, false, "shield_wizard_2", "Arcane Orb", 4, 0, "shield", 2, false },
                    { 40, 0, "mage", 45, 0, false, "shield_wizard_3", "Shimmering Orb", 6, 0, "shield", 3, false },
                    { 41, 0, "mage", 60, 0, false, "shield_wizard_4", "Radiant Orb", 8, 0, "shield", 4, false },
                    { 42, 0, "mage", 75, 0, false, "shield_wizard_5", "Legendary Orb", 10, 0, "shield", 5, false },
                    { 43, 0, "rogue", 0, 0, false, "weapon_rogue_0", "Twig", 0, 0, "weapon", 0, false },
                    { 44, 0, "rogue", 25, 0, false, "weapon_rogue_1", "Iron Dagger", 3, 1, "weapon", 1, false },
                    { 45, 0, "rogue", 50, 0, false, "weapon_rogue_2", "Steel Dagger", 6, 2, "weapon", 2, false },
                    { 46, 0, "rogue", 75, 0, false, "weapon_rogue_3", "Thief Blade", 9, 3, "weapon", 3, false },
                    { 47, 0, "rogue", 100, 0, false, "weapon_rogue_4", "Shadow Blade", 12, 4, "weapon", 4, false },
                    { 48, 0, "rogue", 125, 0, false, "weapon_rogue_5", "Legendary Dagger", 15, 5, "weapon", 5, false },
                    { 49, 0, "rogue", 20, 0, false, "armor_rogue_1", "Leather Armor", 2, 2, "armor", 1, false },
                    { 50, 0, "rogue", 40, 0, false, "armor_rogue_2", "Studded Leather", 4, 4, "armor", 2, false },
                    { 51, 0, "rogue", 60, 0, false, "armor_rogue_3", "Shadow Leather", 6, 6, "armor", 3, false },
                    { 52, 0, "rogue", 80, 0, false, "armor_rogue_4", "Thief Armor", 8, 8, "armor", 4, false },
                    { 53, 0, "rogue", 100, 0, false, "armor_rogue_5", "Legendary Leather", 10, 10, "armor", 5, false },
                    { 54, 0, "rogue", 15, 0, false, "head_rogue_1", "Leather Cap", 2, 1, "head", 1, false },
                    { 55, 0, "rogue", 30, 0, false, "head_rogue_2", "Scout Hood", 4, 2, "head", 2, false },
                    { 56, 0, "rogue", 45, 0, false, "head_rogue_3", "Shadow Hood", 6, 3, "head", 3, false },
                    { 57, 0, "rogue", 60, 0, false, "head_rogue_4", "Thief Hood", 8, 4, "head", 4, false },
                    { 58, 0, "rogue", 75, 0, false, "head_rogue_5", "Legendary Hood", 10, 5, "head", 5, false },
                    { 59, 0, "rogue", 15, 0, false, "shield_rogue_1", "Buckler", 0, 2, "shield", 1, false },
                    { 60, 0, "rogue", 30, 0, false, "shield_rogue_2", "Iron Buckler", 0, 4, "shield", 2, false },
                    { 61, 0, "rogue", 45, 0, false, "shield_rogue_3", "Shadow Buckler", 0, 6, "shield", 3, false },
                    { 62, 0, "rogue", 60, 0, false, "shield_rogue_4", "Thief Buckler", 0, 8, "shield", 4, false },
                    { 63, 0, "rogue", 75, 0, false, "shield_rogue_5", "Legendary Buckler", 0, 10, "shield", 5, false },
                    { 64, 0, "healer", 0, 0, false, "weapon_healer_0", "Worn Staff", 0, 0, "weapon", 0, false },
                    { 65, 3, "healer", 25, 1, false, "weapon_healer_1", "Acolyte Staff", 0, 0, "weapon", 1, false },
                    { 66, 6, "healer", 50, 2, false, "weapon_healer_2", "Healer Staff", 0, 0, "weapon", 2, false },
                    { 67, 9, "healer", 75, 3, false, "weapon_healer_3", "Holy Staff", 0, 0, "weapon", 3, false },
                    { 68, 12, "healer", 100, 4, false, "weapon_healer_4", "Divine Staff", 0, 0, "weapon", 4, false },
                    { 69, 15, "healer", 125, 5, false, "weapon_healer_5", "Legendary Holy Staff", 0, 0, "weapon", 5, false },
                    { 70, 2, "healer", 20, 2, false, "armor_healer_1", "Acolyte Robes", 0, 0, "armor", 1, false },
                    { 71, 4, "healer", 40, 4, false, "armor_healer_2", "Healer Robes", 0, 0, "armor", 2, false },
                    { 72, 6, "healer", 60, 6, false, "armor_healer_3", "Holy Vestments", 0, 0, "armor", 3, false },
                    { 73, 8, "healer", 80, 8, false, "armor_healer_4", "Divine Robes", 0, 0, "armor", 4, false },
                    { 74, 10, "healer", 100, 10, false, "armor_healer_5", "Legendary Vestments", 0, 0, "armor", 5, false },
                    { 75, 2, "healer", 15, 1, false, "head_healer_1", "Cloth Wrap", 0, 0, "head", 1, false },
                    { 76, 4, "healer", 30, 2, false, "head_healer_2", "Healer Wrap", 0, 0, "head", 2, false },
                    { 77, 6, "healer", 45, 3, false, "head_healer_3", "Holy Crown", 0, 0, "head", 3, false },
                    { 78, 8, "healer", 60, 4, false, "head_healer_4", "Divine Crown", 0, 0, "head", 4, false },
                    { 79, 10, "healer", 75, 5, false, "head_healer_5", "Legendary Crown", 0, 0, "head", 5, false },
                    { 80, 0, "healer", 15, 2, false, "shield_healer_1", "Wooden Buckler", 0, 0, "shield", 1, false },
                    { 81, 0, "healer", 30, 4, false, "shield_healer_2", "Healer Shield", 0, 0, "shield", 2, false },
                    { 82, 0, "healer", 45, 6, false, "shield_healer_3", "Holy Shield", 0, 0, "shield", 3, false },
                    { 83, 0, "healer", 60, 8, false, "shield_healer_4", "Divine Shield", 0, 0, "shield", 4, false },
                    { 84, 0, "healer", 75, 10, false, "shield_healer_5", "Legendary Shield", 0, 0, "shield", 5, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GearItems_Key",
                table: "GearItems",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGearItems_GearItemId",
                table: "UserGearItems",
                column: "GearItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGearItems_UserId_GearItemId",
                table: "UserGearItems",
                columns: new[] { "UserId", "GearItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGearItems");

            migrationBuilder.DropTable(
                name: "GearItems");

            migrationBuilder.DropColumn(
                name: "BodyType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BuffCON",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BuffExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BuffINT",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BuffPER",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BuffSTR",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeArmor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeBack",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeBody",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeEyewear",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeHead",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeHeadAccessory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeModeEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeShield",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostumeWeapon",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedArmor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedBack",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedBody",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedEyewear",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedHead",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedHeadAccessory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedShield",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EquippedWeapon",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HairBangs",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HairBeard",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HairColor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HairMustache",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HairStyle",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShirtStyle",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SkinColor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StatPoints",
                table: "Users");
        }
    }
}
