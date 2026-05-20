// Data/AppDbContext.cs
using HabitTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<UserQuest> UserQuests { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<UserFacility> UserFacilities { get; set; }
        public DbSet<UserInventoryItem> UserInventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Quests
            modelBuilder.Entity<Quest>().HasData(
                new Quest { Id = 1, Name = "Tập thể dục", Category = "Sức khỏe",
                    Difficulty = "Easy", Frequency = "Daily", XPReward = 10,
                    Icon = "🏋️", FacilityId = 1, MinigameType = "QTE" },
                new Quest { Id = 2, Name = "Chạy bộ", Category = "Sức khỏe",
                    Difficulty = "Easy", Frequency = "Daily", XPReward = 10,
                    Icon = "🏃", FacilityId = 4, MinigameType = "Dino" },
                new Quest { Id = 3, Name = "Đọc sách", Category = "Học tập",
                    Difficulty = "Medium", Frequency = "Daily", XPReward = 25,
                    Icon = "📖", FacilityId = 3, MinigameType = "Tetris" }
            );

            // Seed Badge
            modelBuilder.Entity<Badge>().HasData(
                new Badge { Id = 1, Name = "Người mới bắt đầu", Description = "Đạt 50 XP", Icon = "🌱", RequiredXP = 50 },
                new Badge { Id = 2, Name = "Chiến binh", Description = "Đạt 200 XP", Icon = "⚔️", RequiredXP = 200 },
                new Badge { Id = 3, Name = "Huyền thoại", Description = "Đạt 500 XP", Icon = "🏆", RequiredXP = 500 }
            );

            // Quest → Facility (nullable, SetNull on facility delete)
            modelBuilder.Entity<Quest>()
                .HasOne(q => q.AssignedFacility)
                .WithMany()
                .HasForeignKey(q => q.FacilityId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== FACILITY SEED DATA =====
            modelBuilder.Entity<Facility>().HasData(
                new Facility
                {
                    Id = 1, Name = "Sân Tập Luyện", Icon = "🏋️",
                    Description = "Không gian tập luyện thể chất. Rèn giũa sức mạnh chiến đấu qua các buổi luyện tập hàng ngày.",
                    StatAffected = "ATK", BuffDescription = "+5 Tấn Công mỗi cấp", BuffPerLevel = 5, MaxLevel = 5
                },
                new Facility
                {
                    Id = 2, Name = "Thiền Đường", Icon = "🧘",
                    Description = "Thanh lọc tâm trí, tăng cường tinh thần. Thiền định giúp mở rộng sinh lực.",
                    StatAffected = "HP", BuffDescription = "+20 Sinh Lực mỗi cấp", BuffPerLevel = 20, MaxLevel = 5
                },
                new Facility
                {
                    Id = 3, Name = "Thư Viện", Icon = "📚",
                    Description = "Kho sách cổ và tri thức. Kiến thức thúc đẩy sự phát triển và mài sắc trí tuệ.",
                    StatAffected = "XPGain", BuffDescription = "+2% Nhận XP mỗi cấp", BuffPerLevel = 2, MaxLevel = 5
                },
                new Facility
                {
                    Id = 4, Name = "Đường Chướng Ngại", Icon = "🏃",
                    Description = "Bài tập vượt chướng ngại và phản xạ đẩy cơ thể đến giới hạn. Tốc độ là nửa chiến thắng.",
                    StatAffected = "Stamina", BuffDescription = "+10 Sức Bền mỗi cấp", BuffPerLevel = 10, MaxLevel = 5
                },
                new Facility
                {
                    Id = 5, Name = "Doanh Trại", Icon = "🛡️",
                    Description = "Phòng thủ vững chắc bao bọc căn cứ. Vị trí kiên cố để chịu đựng mọi cuộc tấn công.",
                    StatAffected = "Armor", BuffDescription = "+5 Giáp mỗi cấp", BuffPerLevel = 5, MaxLevel = 5
                },
                new Facility
                {
                    Id = 6, Name = "Phòng Kho", Icon = "📦",
                    Description = "Mở rộng kho chứa của căn cứ. Mỗi lần nâng cấp thêm 30 ô lưới (10×3).",
                    StatAffected = "Storage", BuffDescription = "+30 ô mỗi cấp", BuffPerLevel = 30, MaxLevel = 5
                },
                new Facility
                {
                    Id = 7, Name = "Bàn Thợ", Icon = "🔨",
                    Description = "Trạm chế tác để xử lý nguyên liệu thô. Cấp cao hơn mở khóa thêm ô và công thức.",
                    StatAffected = "Crafting", BuffDescription = "+1 ô chế tác mỗi cấp", BuffPerLevel = 1, MaxLevel = 5
                }
            );

            // UserFacility relationships
            modelBuilder.Entity<UserFacility>()
                .HasOne(uf => uf.Facility_User)
                .WithMany()
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFacility>()
                .HasOne(uf => uf.Facility)
                .WithMany(f => f.UserFacilities)
                .HasForeignKey(uf => uf.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFacility>()
                .HasIndex(uf => new { uf.UserId, uf.FacilityId })
                .IsUnique();

            // UserInventoryItem → User
            modelBuilder.Entity<UserInventoryItem>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Friendship relationships
            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Requester)
                .WithMany()
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Receiver)
                .WithMany()
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}