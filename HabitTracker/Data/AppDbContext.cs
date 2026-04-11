// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using HabitTracker.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed dữ liệu Quest có sẵn
            modelBuilder.Entity<Quest>().HasData(
                // === SỨC KHỎE ===
                new Quest { Id = 1, Name = "Uống 2 lít nước", Category = "Sức khỏe", Difficulty = "Easy", Frequency = "Daily", XPReward = 10, Icon = "💧" },
                new Quest { Id = 2, Name = "Tập thể dục 30 phút", Category = "Sức khỏe", Difficulty = "Medium", Frequency = "Daily", XPReward = 25, Icon = "🏃" },
                new Quest { Id = 3, Name = "Ngủ đủ 8 tiếng", Category = "Sức khỏe", Difficulty = "Easy", Frequency = "Daily", XPReward = 10, Icon = "😴" },
                new Quest { Id = 4, Name = "Chạy bộ 5km", Category = "Sức khỏe", Difficulty = "Hard", Frequency = "Weekly", XPReward = 50, Icon = "🏅" },

                // === HỌC TẬP ===
                new Quest { Id = 5, Name = "Đọc sách 20 phút", Category = "Học tập", Difficulty = "Easy", Frequency = "Daily", XPReward = 10, Icon = "📚" },
                new Quest { Id = 6, Name = "Học ngoại ngữ 30 phút", Category = "Học tập", Difficulty = "Medium", Frequency = "Daily", XPReward = 25, Icon = "🗣️" },
                new Quest { Id = 7, Name = "Hoàn thành 1 khóa học online", Category = "Học tập", Difficulty = "Hard", Frequency = "Monthly", XPReward = 50, Icon = "🎓" },

                // === TINH THẦN ===
                new Quest { Id = 8, Name = "Thiền 10 phút", Category = "Tinh thần", Difficulty = "Easy", Frequency = "Daily", XPReward = 10, Icon = "🧘" },
                new Quest { Id = 9, Name = "Viết nhật ký", Category = "Tinh thần", Difficulty = "Easy", Frequency = "Daily", XPReward = 10, Icon = "📝" },

                // === TÀI CHÍNH ===
                new Quest { Id = 10, Name = "Ghi chép chi tiêu hôm nay", Category = "Tài chính", Difficulty = "Easy", Frequency = "Daily", XPReward = 10, Icon = "💰" },
                new Quest { Id = 11, Name = "Tiết kiệm theo kế hoạch tuần", Category = "Tài chính", Difficulty = "Medium", Frequency = "Weekly", XPReward = 25, Icon = "🏦" }
            );

            // Seed Badge
            modelBuilder.Entity<Badge>().HasData(
                new Badge { Id = 1, Name = "Người mới bắt đầu", Description = "Đạt 50 XP", Icon = "🌱", RequiredXP = 50 },
                new Badge { Id = 2, Name = "Chiến binh", Description = "Đạt 200 XP", Icon = "⚔️", RequiredXP = 200 },
                new Badge { Id = 3, Name = "Huyền thoại", Description = "Đạt 500 XP", Icon = "🏆", RequiredXP = 500 }
            );
        }
    }
}