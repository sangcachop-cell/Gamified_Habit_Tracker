using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQuestSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 11);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Category", "CategoryId", "CreatedAt", "Description", "Difficulty", "Frequency", "Icon", "IsActive", "Name", "TimesCompleted", "UpdatedAt", "XPReward" },
                values: new object[,]
                {
                    { 1, "Sức khỏe", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", "Daily", "💧", true, "Uống 2 lít nước", 0, null, 10 },
                    { 2, "Sức khỏe", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Medium", "Daily", "🏃", true, "Tập thể dục 30 phút", 0, null, 25 },
                    { 3, "Sức khỏe", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", "Daily", "😴", true, "Ngủ đủ 8 tiếng", 0, null, 10 },
                    { 4, "Sức khỏe", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hard", "Weekly", "🏅", true, "Chạy bộ 5km", 0, null, 50 },
                    { 5, "Học tập", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", "Daily", "📚", true, "Đọc sách 20 phút", 0, null, 10 },
                    { 6, "Học tập", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Medium", "Daily", "🗣️", true, "Học ngoại ngữ 30 phút", 0, null, 25 },
                    { 7, "Học tập", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hard", "Monthly", "🎓", true, "Hoàn thành 1 khóa học online", 0, null, 50 },
                    { 8, "Tinh thần", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", "Daily", "🧘", true, "Thiền 10 phút", 0, null, 10 },
                    { 9, "Tinh thần", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", "Daily", "📝", true, "Viết nhật ký", 0, null, 10 },
                    { 10, "Tài chính", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Easy", "Daily", "💰", true, "Ghi chép chi tiêu hôm nay", 0, null, 10 },
                    { 11, "Tài chính", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Medium", "Weekly", "🏦", true, "Tiết kiệm theo kế hoạch tuần", 0, null, 25 }
                });
        }
    }
}
