using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class VietnameseFacilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+5 Tấn Công mỗi cấp", "Không gian tập luyện thể chất. Rèn giũa sức mạnh chiến đấu qua các buổi luyện tập hàng ngày.", "Sân Tập Luyện" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+20 Sinh Lực mỗi cấp", "Thanh lọc tâm trí, tăng cường tinh thần. Thiền định giúp mở rộng sinh lực.", "Thiền Đường" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+2% Nhận XP mỗi cấp", "Kho sách cổ và tri thức. Kiến thức thúc đẩy sự phát triển và mài sắc trí tuệ.", "Thư Viện" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+10 Sức Bền mỗi cấp", "Bài tập vượt chướng ngại và phản xạ đẩy cơ thể đến giới hạn. Tốc độ là nửa chiến thắng.", "Đường Chướng Ngại" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+5 Giáp mỗi cấp", "Phòng thủ vững chắc bao bọc căn cứ. Vị trí kiên cố để chịu đựng mọi cuộc tấn công.", "Doanh Trại" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+30 ô mỗi cấp", "Mở rộng kho chứa của căn cứ. Mỗi lần nâng cấp thêm 30 ô lưới (10×3).", "Phòng Kho" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+1 ô chế tác mỗi cấp", "Trạm chế tác để xử lý nguyên liệu thô. Cấp cao hơn mở khóa thêm ô và công thức.", "Bàn Thợ" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+5 ATK per level", "A dedicated space for physical conditioning. Forges raw combat power through daily drills.", "Training Grounds" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+20 HP per level", "Silence the mind, fortify the spirit. Meditative practice expands your life force.", "Meditation Hall" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+2% XP Gain per level", "Ancient texts and ongoing studies. Knowledge accelerates growth and sharpens the mind.", "Archive" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+10 Stamina per level", "Obstacle runs and reflex drills push your body to its limits. Speed is half the battle.", "Agility Course" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+5 Armor per level", "Hardened defenses line the outer walls. A stalwart position from which to weather any storm.", "Barracks" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+30 slots per level", "Expand your hideout's storage capacity. Each upgrade adds 30 more grid slots (10×3).", "Storage Room" });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BuffDescription", "Description", "Name" },
                values: new object[] { "+1 craft slot per level", "A crafting station for processing raw materials. Higher levels unlock more slots and recipes.", "Workbench" });
        }
    }
}
