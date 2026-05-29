using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class FixGearItemSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop unique index temporarily — many rows swap Keys during reorder
            migrationBuilder.DropIndex(
                name: "IX_GearItems_Key",
                table: "GearItems");

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "GoldCost",
                value: 1);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 0, 20 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 0, 30 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 0, 45 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 0, 65 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 0, 90 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Slot", "Tier" },
                values: new object[] { 0, 120, "weapon_warrior_6", "Celestial Sword", 18, "weapon", 6 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 3, 30, "armor_warrior_1", "Chain Mail", 0, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 5, 45, "armor_warrior_2", "Plate Armor", 0, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 7, 65, "armor_warrior_3", "Dragon Scale", 0, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 9, 90, "armor_warrior_4", "Titan Plate", 0, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Slot", "Tier" },
                values: new object[] { 11, 120, "armor_warrior_5", "Legendary Armor", 0, "armor", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 0, 15, "head_warrior_1", "Iron Helm", 2, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 0, 25, "head_warrior_2", "Steel Helm", 4, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 0, 40, "head_warrior_3", "Dragon Helm", 6, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 0, 60, "head_warrior_4", "Titan Helm", 9, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Slot", "Tier" },
                values: new object[] { 0, 80, "head_warrior_5", "Legendary Helm", 12, "head", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 2, 20, "shield_warrior_1", "Wooden Shield", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 3, 35, "shield_warrior_2", "Iron Shield", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 5, 50, "shield_warrior_3", "Steel Shield", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 7, 70, "shield_warrior_4", "Dragon Shield", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CON", "GearClass", "GoldCost", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 9, "warrior", 90, "shield_warrior_5", "Legendary Shield", "shield", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 0, 0, "weapon_wizard_0", "Broken Branch", 0, 0, true });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 30, 3, "weapon_wizard_1", "Apprentice Wand", 1, 1, true });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 50, 6, "weapon_wizard_2", "Journeyman Staff", 2, 2, true });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 80, 9, "weapon_wizard_3", "Mage Staff", 3, 3, true });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 120, 12, "weapon_wizard_4", "Arcane Staff", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier", "TwoHanded" },
                values: new object[] { 160, 15, "weapon_wizard_5", "Legendary Arcane Staff", 7, "weapon", 5, true });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier", "TwoHanded" },
                values: new object[] { 200, 18, "weapon_wizard_6", "Celestial Staff", 10, "weapon", 6, true });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 30, 2, "armor_wizard_1", "Initiate Robes", 0, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 45, 4, "armor_wizard_2", "Apprentice Robes", 0, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 65, 6, "armor_wizard_3", "Journeyman Robes", 0, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 90, 9, "armor_wizard_4", "Mage Robes", 0, "armor", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 120, 12, "armor_wizard_5", "Legendary Robes", 0, "armor", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 15, 0, "head_wizard_1", "Cloth Cap", 2, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 25, 0, "head_wizard_2", "Apprentice Hat", 3, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 40, 0, "head_wizard_3", "Journeyman Hat", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 60, "head_wizard_4", "Mage Hat", 7, "head", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 80, "head_wizard_5", "Legendary Hat", 10, "head", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "GearClass", "GoldCost", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { "rogue", 0, "weapon_rogue_0", "Twig", 0, "weapon", 0 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "GearClass", "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { "rogue", 20, "weapon_rogue_1", "Iron Dagger", 0, 2, "weapon", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "GearClass", "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { "rogue", 35, "weapon_rogue_2", "Steel Dagger", 0, 3, "weapon", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 50, "weapon_rogue_3", "Thief Blade", 4, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 70, "weapon_rogue_4", "Shadow Blade", 0, 6, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 90, "weapon_rogue_5", "Legendary Dagger", 0, 8, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 120, "weapon_rogue_6", "Celestial Dagger", 0, 10, 6 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 30, "armor_rogue_1", "Leather Armor", 6, 0, "armor", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 45, "armor_rogue_2", "Studded Leather", 9, 0, "armor", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 65, "armor_rogue_3", "Shadow Leather", 12, 0, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 90, "armor_rogue_4", "Thief Armor", 15, 0, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 120, "armor_rogue_5", "Legendary Leather", 18, 0, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 15, "head_rogue_1", "Leather Cap", 2, 0, "head", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 25, "head_rogue_2", "Scout Hood", 4, 0, "head", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 40, "head_rogue_3", "Shadow Hood", 6, 0, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 60, "head_rogue_4", "Thief Hood", 9, 0, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 80, "head_rogue_5", "Legendary Hood", 12, 0, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 0, "shield_rogue_0", "Empty Hand", 0, 0, "shield", 0 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 20, "shield_rogue_1", "Iron Off-hand", 0, 2, "shield", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 35, "shield_rogue_2", "Steel Off-hand", 3, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 50, "shield_rogue_3", "Thief Off-hand", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 70, "shield_rogue_4", "Shadow Off-hand", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 90, "shield_rogue_5", "Legendary Off-hand", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 120, "shield_rogue_6", "Celestial Off-hand", 6 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 0, 20, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 0, 30, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 0, 45, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 0, 65, 7 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 0, 90, 9 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 0, 120, 11, "weapon_healer_6", "Celestial Holy Staff", "weapon", 6 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 6, 30, 0, "armor_healer_1", "Acolyte Robes", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 9, 45, 0, "armor_healer_2", "Healer Robes", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 12, 65, 0, "armor_healer_3", "Holy Vestments", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 15, 90, 0, "armor_healer_4", "Divine Robes", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 18, 120, 0, "armor_healer_5", "Legendary Vestments", "armor", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 0, 15, "head_healer_1", "Cloth Wrap", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 0, 25, "head_healer_2", "Healer Wrap", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 0, 40, 5, "head_healer_3", "Holy Crown", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 0, 60, 7, "head_healer_4", "Divine Crown", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 80, 9, "head_healer_5", "Legendary Crown", "head", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 2, 20, 0, "shield_healer_1", "Worn Buckler", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 4, 35, 0, "shield_healer_2", "Healer Shield", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 6, 50, 0, "shield_healer_3", "Holy Shield", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 9, 70, 0, "shield_healer_4", "Divine Shield", 4 });

            migrationBuilder.InsertData(
                table: "GearItems",
                columns: new[] { "Id", "CON", "GearClass", "GoldCost", "INT", "IsSpecial", "Key", "Name", "PER", "STR", "Slot", "Tier", "TwoHanded" },
                values: new object[] { 85, 12, "healer", 90, 0, false, "shield_healer_5", "Legendary Shield", 0, 0, "shield", 5, false });

            // Recreate unique index now that all Keys are settled
            migrationBuilder.CreateIndex(
                name: "IX_GearItems_Key",
                table: "GearItems",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "GoldCost",
                value: 0);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 1, 25 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 2, 50 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 3, 75 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 4, 100 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CON", "GoldCost" },
                values: new object[] { 5, 125 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Slot", "Tier" },
                values: new object[] { 2, 20, "armor_warrior_1", "Chain Mail", 2, "armor", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 4, 40, "armor_warrior_2", "Plate Armor", 4, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 6, 60, "armor_warrior_3", "Dragon Scale", 6, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 8, 80, "armor_warrior_4", "Titan Plate", 8, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 10, 100, "armor_warrior_5", "Legendary Armor", 10, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Slot", "Tier" },
                values: new object[] { 1, 15, "head_warrior_1", "Iron Helm", 2, "head", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 2, 30, "head_warrior_2", "Steel Helm", 4, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 3, 45, "head_warrior_3", "Dragon Helm", 6, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 4, 60, "head_warrior_4", "Titan Helm", 8, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 5, 75, "head_warrior_5", "Legendary Helm", 10, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "STR", "Slot", "Tier" },
                values: new object[] { 2, 15, "shield_warrior_1", "Wooden Shield", 0, "shield", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 4, 30, "shield_warrior_2", "Iron Shield", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 6, 45, "shield_warrior_3", "Steel Shield", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 8, 60, "shield_warrior_4", "Dragon Shield", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 10, 75, "shield_warrior_5", "Legendary Shield", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CON", "GearClass", "GoldCost", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 0, "mage", 0, "weapon_wizard_0", "Broken Branch", "weapon", 0 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 25, 3, "weapon_wizard_1", "Apprentice Wand", 1, 1, false });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 50, 6, "weapon_wizard_2", "Journeyman Staff", 2, 2, false });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 75, 9, "weapon_wizard_3", "Mage Staff", 3, 3, false });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier", "TwoHanded" },
                values: new object[] { 100, 12, "weapon_wizard_4", "Arcane Staff", 4, 4, false });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 125, 15, "weapon_wizard_5", "Legendary Arcane Staff", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier", "TwoHanded" },
                values: new object[] { 20, 2, "armor_wizard_1", "Initiate Robes", 2, "armor", 1, false });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier", "TwoHanded" },
                values: new object[] { 40, 4, "armor_wizard_2", "Apprentice Robes", 4, "armor", 2, false });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 60, 6, "armor_wizard_3", "Journeyman Robes", 6, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 80, 8, "armor_wizard_4", "Mage Robes", 8, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 100, 10, "armor_wizard_5", "Legendary Robes", 10, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 15, 2, "head_wizard_1", "Cloth Cap", 1, "head", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 30, 4, "head_wizard_2", "Apprentice Hat", 2, "head", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 45, 6, "head_wizard_3", "Journeyman Hat", 3, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "PER", "Tier" },
                values: new object[] { 60, 8, "head_wizard_4", "Mage Hat", 4, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 75, 10, "head_wizard_5", "Legendary Hat", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 15, "shield_wizard_1", "Orb of Protection", 2, "shield", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { 30, "shield_wizard_2", "Arcane Orb", 4, "shield", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "GearClass", "GoldCost", "Key", "Name", "PER", "Slot", "Tier" },
                values: new object[] { "mage", 45, "shield_wizard_3", "Shimmering Orb", 6, "shield", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "GearClass", "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { "mage", 60, "shield_wizard_4", "Radiant Orb", 8, 0, "shield", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "GearClass", "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { "mage", 75, "shield_wizard_5", "Legendary Orb", 10, 0, "shield", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 0, "weapon_rogue_0", "Twig", 0, 0 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 25, "weapon_rogue_1", "Iron Dagger", 3, 1, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 50, "weapon_rogue_2", "Steel Dagger", 6, 2, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 75, "weapon_rogue_3", "Thief Blade", 9, 3, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 100, "weapon_rogue_4", "Shadow Blade", 12, 4, "weapon", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 125, "weapon_rogue_5", "Legendary Dagger", 15, 5, "weapon", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 20, "armor_rogue_1", "Leather Armor", 2, 2, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 40, "armor_rogue_2", "Studded Leather", 4, 4, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 60, "armor_rogue_3", "Shadow Leather", 6, 6, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 80, "armor_rogue_4", "Thief Armor", 8, 8, "armor", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 100, "armor_rogue_5", "Legendary Leather", 10, 10, "armor", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 15, "head_rogue_1", "Leather Cap", 2, 1, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 30, "head_rogue_2", "Scout Hood", 4, 2, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Tier" },
                values: new object[] { 45, "head_rogue_3", "Shadow Hood", 6, 3, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 60, "head_rogue_4", "Thief Hood", 8, 4, "head", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "GoldCost", "Key", "Name", "PER", "STR", "Slot", "Tier" },
                values: new object[] { 75, "head_rogue_5", "Legendary Hood", 10, 5, "head", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "GoldCost", "Key", "Name", "STR", "Tier" },
                values: new object[] { 15, "shield_rogue_1", "Buckler", 2, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 30, "shield_rogue_2", "Iron Buckler", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 45, "shield_rogue_3", "Shadow Buckler", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 60, "shield_rogue_4", "Thief Buckler", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 75, "shield_rogue_5", "Legendary Buckler", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 3, 25, 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 6, 50, 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 9, 75, 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 12, 100, 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "CON", "GoldCost", "INT" },
                values: new object[] { 15, 125, 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 2, 20, 2, "armor_healer_1", "Acolyte Robes", "armor", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 4, 40, 4, "armor_healer_2", "Healer Robes", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 6, 60, 6, "armor_healer_3", "Holy Vestments", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 8, 80, 8, "armor_healer_4", "Divine Robes", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 10, 100, 10, "armor_healer_5", "Legendary Vestments", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 2, 15, 1, "head_healer_1", "Cloth Wrap", "head", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 4, 30, "head_healer_2", "Healer Wrap", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "CON", "GoldCost", "Key", "Name", "Tier" },
                values: new object[] { 6, 45, "head_healer_3", "Holy Crown", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 8, 60, 4, "head_healer_4", "Divine Crown", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 10, 75, 5, "head_healer_5", "Legendary Crown", 5 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "GoldCost", "INT", "Key", "Name", "Slot", "Tier" },
                values: new object[] { 15, 2, "shield_healer_1", "Wooden Buckler", "shield", 1 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 0, 30, 4, "shield_healer_2", "Healer Shield", 2 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 0, 45, 6, "shield_healer_3", "Holy Shield", 3 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 0, 60, 8, "shield_healer_4", "Divine Shield", 4 });

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "CON", "GoldCost", "INT", "Key", "Name", "Tier" },
                values: new object[] { 0, 75, 10, "shield_healer_5", "Legendary Shield", 5 });
        }
    }
}
