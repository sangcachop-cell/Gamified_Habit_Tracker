using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase10Achievements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "RequiredXP",
                table: "Badges",
                newName: "TriggerValue");

            migrationBuilder.AddColumn<int>(
                name: "PerfectDayCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTasksCompleted",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Badges",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Badges",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Badges",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TriggerType",
                table: "Badges",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "CreatedAt", "Description", "Icon", "IsActive", "Key", "Name", "Rarity", "TriggerType", "TriggerValue" },
                values: new object[,]
                {
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maintain a 7-day task streak", "🌡️", true, "streak_7", "Streak Starter", "Common", "Streak", 7 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maintain a 21-day task streak", "🔥", true, "streak_21", "Habit Builder", "Rare", "Streak", 21 },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maintain a 90-day task streak", "⚡", true, "streak_90", "Dedicated", "Epic", "Streak", 90 },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maintain a 180-day task streak", "💎", true, "streak_180", "Half Year Hero", "Legendary", "Streak", 180 },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maintain a 365-day task streak", "👑", true, "streak_365", "Year Champion", "Legendary", "Streak", 365 },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete your first task", "✅", true, "tasks_1", "First Step", "Common", "TaskMilestone", 1 },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 10 tasks", "📋", true, "tasks_10", "Getting Started", "Common", "TaskMilestone", 10 },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 50 tasks", "🎯", true, "tasks_50", "On a Roll", "Rare", "TaskMilestone", 50 },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 100 tasks", "💯", true, "tasks_100", "Century Mark", "Epic", "TaskMilestone", 100 },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 500 tasks", "🏆", true, "tasks_500", "Task Master", "Legendary", "TaskMilestone", 500 },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete all dailies in a day", "⭐", true, "perfect_1", "Perfect Day", "Common", "PerfectDay", 1 },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Achieve 7 perfect days", "🌟", true, "perfect_7", "Week of Perfection", "Rare", "PerfectDay", 7 },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Achieve 30 perfect days", "✨", true, "perfect_30", "Flawless Month", "Epic", "PerfectDay", 30 },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Collect all Warrior class gear", "⚔️", true, "ultimate_warrior", "Warrior's Arsenal", "Epic", "UltimateGear", 0 },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Collect all Mage class gear", "🔮", true, "ultimate_mage", "Arcane Scholar", "Epic", "UltimateGear", 0 },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Collect all Rogue class gear", "🗡️", true, "ultimate_rogue", "Shadow Striker", "Epic", "UltimateGear", 0 },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Collect all Healer class gear", "💚", true, "ultimate_healer", "Divine Healer", "Epic", "UltimateGear", 0 },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete your first party quest", "⚔️", true, "quests_1", "First Quest", "Common", "Quest", 1 },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 10 party quests", "🗡️", true, "quests_10", "Quest Veteran", "Rare", "Quest", 10 },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 50 party quests", "🏅", true, "quests_50", "Quest Legend", "Epic", "Quest", 50 },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Own 10 pets or mounts", "🐾", true, "stable_10", "Animal Keeper", "Common", "Stable", 10 },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Own 100 unique pets", "🦁", true, "beast_master", "Beast Master", "Legendary", "Stable", 100 },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Own 50 unique mounts", "🐴", true, "mount_master", "Mount Master", "Legendary", "Stable", 50 },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Own a pet and mount from the same species", "🎰", true, "triad_bingo", "Triad Bingo", "Epic", "Stable", 0 },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Join your first guild", "🏰", true, "joined_guild", "Guild Member", "Common", "Social", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DropColumn(
                name: "PerfectDayCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalTasksCompleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "TriggerType",
                table: "Badges");

            migrationBuilder.RenameColumn(
                name: "TriggerValue",
                table: "Badges",
                newName: "RequiredXP");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Badges",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Badges",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "CreatedAt", "Description", "Icon", "IsActive", "Name", "Rarity", "RequiredXP" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đạt 50 XP", "🌱", true, "Người mới bắt đầu", "Common", 50 },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đạt 200 XP", "⚔️", true, "Chiến binh", "Common", 200 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đạt 500 XP", "🏆", true, "Huyền thoại", "Common", 500 }
                });
        }
    }
}
