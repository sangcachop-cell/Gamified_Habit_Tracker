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
                    Icon = "🏃", FacilityId = 4, MinigameType = "Dino" }
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
                    Id = 1, Name = "Training Grounds", Icon = "🏋️",
                    Description = "A dedicated space for physical conditioning. Forges raw combat power through daily drills.",
                    StatAffected = "ATK", BuffDescription = "+5 ATK per level", BuffPerLevel = 5, MaxLevel = 5
                },
                new Facility
                {
                    Id = 2, Name = "Meditation Hall", Icon = "🧘",
                    Description = "Silence the mind, fortify the spirit. Meditative practice expands your life force.",
                    StatAffected = "HP", BuffDescription = "+20 HP per level", BuffPerLevel = 20, MaxLevel = 5
                },
                new Facility
                {
                    Id = 3, Name = "Archive", Icon = "📚",
                    Description = "Ancient texts and ongoing studies. Knowledge accelerates growth and sharpens the mind.",
                    StatAffected = "XPGain", BuffDescription = "+2% XP Gain per level", BuffPerLevel = 2, MaxLevel = 5
                },
                new Facility
                {
                    Id = 4, Name = "Agility Course", Icon = "🏃",
                    Description = "Obstacle runs and reflex drills push your body to its limits. Speed is half the battle.",
                    StatAffected = "Stamina", BuffDescription = "+10 Stamina per level", BuffPerLevel = 10, MaxLevel = 5
                },
                new Facility
                {
                    Id = 5, Name = "Barracks", Icon = "🛡️",
                    Description = "Hardened defenses line the outer walls. A stalwart position from which to weather any storm.",
                    StatAffected = "Armor", BuffDescription = "+5 Armor per level", BuffPerLevel = 5, MaxLevel = 5
                },
                new Facility
                {
                    Id = 6, Name = "Storage Room", Icon = "📦",
                    Description = "Expand your hideout's storage capacity. Each upgrade adds 30 more grid slots (10×3).",
                    StatAffected = "Storage", BuffDescription = "+30 slots per level", BuffPerLevel = 30, MaxLevel = 5
                },
                new Facility
                {
                    Id = 7, Name = "Workbench", Icon = "🔨",
                    Description = "A crafting station for processing raw materials. Higher levels unlock more slots and recipes.",
                    StatAffected = "Crafting", BuffDescription = "+1 craft slot per level", BuffPerLevel = 1, MaxLevel = 5
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