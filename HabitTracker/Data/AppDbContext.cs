// Data/AppDbContext.cs
using HabitTracker.Constants;
using HabitTracker.Data.Seeds;
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

        // Task system (Phase 1)
        public DbSet<HabitTask> HabitTasks { get; set; }
        public DbSet<TaskChecklistItem> TaskChecklistItems { get; set; }
        public DbSet<UserTaskTag> UserTaskTags { get; set; }
        public DbSet<TaskTagAssignment> TaskTagAssignments { get; set; }

        // Economy (Phase 2)
        public DbSet<GameItem>          GameItems          { get; set; }
        public DbSet<UserInventoryItem> UserInventoryItems { get; set; }

        // Character System (Phase 3)
        public DbSet<GearItem>     GearItems     { get; set; }
        public DbSet<UserGearItem> UserGearItems { get; set; }

        // Stable System (Phase 5)
        public DbSet<UserPet> UserPets { get; set; }

        // Social System (Phase 6)
        public DbSet<Message>     Messages     { get; set; }
        public DbSet<MessageLike> MessageLikes { get; set; }
        public DbSet<UserBlock>   UserBlocks   { get; set; }
        public DbSet<Report>      Reports      { get; set; }

        // Guilds & Parties (Phase 7)
        public DbSet<Guild>            Guilds            { get; set; }
        public DbSet<GuildMember>      GuildMembers      { get; set; }
        public DbSet<GuildMessage>     GuildMessages     { get; set; }
        public DbSet<GuildMessageLike> GuildMessageLikes { get; set; }
        public DbSet<GuildInvite>      GuildInvites      { get; set; }
        public DbSet<Party>            Parties           { get; set; }
        public DbSet<PartyMember>      PartyMembers      { get; set; }
        public DbSet<PartyMessage>     PartyMessages     { get; set; }
        public DbSet<PartyMessageLike> PartyMessageLikes { get; set; }
        public DbSet<PartyInvite>      PartyInvites      { get; set; }

        // Boss Quests (Phase 8)
        public DbSet<BossQuest>        BossQuests        { get; set; }
        public DbSet<PartyQuest>       PartyQuests       { get; set; }
        public DbSet<PartyQuestMember> PartyQuestMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Badge
            modelBuilder.Entity<Badge>().HasData(
                new Badge { Id = 1, Name = "Người mới bắt đầu", Description = "Đạt 50 XP", Icon = "🌱", RequiredXP = 50 },
                new Badge { Id = 2, Name = "Chiến binh", Description = "Đạt 200 XP", Icon = "⚔️", RequiredXP = 200 },
                new Badge { Id = 3, Name = "Huyền thoại", Description = "Đạt 500 XP", Icon = "🏆", RequiredXP = 500 }
            );

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

            // HabitTask → User
            modelBuilder.Entity<HabitTask>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskChecklistItem → HabitTask
            modelBuilder.Entity<TaskChecklistItem>()
                .HasOne(c => c.HabitTask)
                .WithMany(t => t.ChecklistItems)
                .HasForeignKey(c => c.HabitTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskTagAssignment → HabitTask
            modelBuilder.Entity<TaskTagAssignment>()
                .HasOne(a => a.HabitTask)
                .WithMany(t => t.TagAssignments)
                .HasForeignKey(a => a.HabitTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskTagAssignment → UserTaskTag (Restrict to avoid multiple cascade paths from User)
            modelBuilder.Entity<TaskTagAssignment>()
                .HasOne(a => a.UserTaskTag)
                .WithMany(tag => tag.TaskAssignments)
                .HasForeignKey(a => a.UserTaskTagId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserTaskTag → User
            modelBuilder.Entity<UserTaskTag>()
                .HasOne(tag => tag.User)
                .WithMany()
                .HasForeignKey(tag => tag.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique: one tag assignment per (task, tag) pair
            modelBuilder.Entity<TaskTagAssignment>()
                .HasIndex(a => new { a.HabitTaskId, a.UserTaskTagId })
                .IsUnique();

            // Index for board query: tasks by user + type
            modelBuilder.Entity<HabitTask>()
                .HasIndex(t => new { t.UserId, t.Type });

            // UserInventoryItem → User (cascade delete)
            modelBuilder.Entity<UserInventoryItem>()
                .HasOne(i => i.User)
                .WithMany(u => u.InventoryItems)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserInventoryItem → GameItem (restrict: catalog rows must not cascade-delete)
            modelBuilder.Entity<UserInventoryItem>()
                .HasOne(i => i.GameItem)
                .WithMany()
                .HasForeignKey(i => i.GameItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite index for inventory lookup
            modelBuilder.Entity<UserInventoryItem>()
                .HasIndex(i => new { i.UserId, i.GameItemId });

            // ===== GEAR SYSTEM (Phase 3) =====

            // UserGearItem → User (cascade: delete user removes their gear ownership)
            modelBuilder.Entity<UserGearItem>()
                .HasOne(g => g.User)
                .WithMany(u => u.OwnedGear)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserGearItem → GearItem (restrict: catalog rows must not cascade-delete)
            modelBuilder.Entity<UserGearItem>()
                .HasOne(g => g.GearItem)
                .WithMany(gi => gi.OwnedByUsers)
                .HasForeignKey(g => g.GearItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique: one ownership row per (user, gearItem)
            modelBuilder.Entity<UserGearItem>()
                .HasIndex(g => new { g.UserId, g.GearItemId })
                .IsUnique();

            // GearItem unique key
            modelBuilder.Entity<GearItem>()
                .HasIndex(g => g.Key)
                .IsUnique();

            // ===== STABLE SYSTEM (Phase 5) =====

            // UserPet → User (cascade: delete user removes their pets)
            modelBuilder.Entity<UserPet>()
                .HasOne(p => p.User)
                .WithMany(u => u.OwnedPets)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique: one pet row per (user, petKey)
            modelBuilder.Entity<UserPet>()
                .HasIndex(p => new { p.UserId, p.PetKey })
                .IsUnique();

            // Index for pets vs mounts tab query
            modelBuilder.Entity<UserPet>()
                .HasIndex(p => new { p.UserId, p.IsMount });

            // ===== SOCIAL SYSTEM (Phase 6) =====

            // Message → Sender (Restrict: deleting a user must not cascade-delete messages)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → Receiver (Restrict)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for inbox queries
            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.SenderId, m.ReceiverId });

            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.ReceiverId, m.IsRead });

            // MessageLike — composite PK
            modelBuilder.Entity<MessageLike>()
                .HasKey(l => new { l.MessageId, l.LikerUserId });

            modelBuilder.Entity<MessageLike>()
                .HasOne(l => l.Message)
                .WithMany(m => m.Likes)
                .HasForeignKey(l => l.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageLike>()
                .HasOne(l => l.LikerUser)
                .WithMany()
                .HasForeignKey(l => l.LikerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserBlock — unique (BlockerId, BlockedId)
            modelBuilder.Entity<UserBlock>()
                .HasIndex(b => new { b.BlockerId, b.BlockedId })
                .IsUnique();

            modelBuilder.Entity<UserBlock>()
                .HasOne(b => b.Blocker)
                .WithMany()
                .HasForeignKey(b => b.BlockerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserBlock>()
                .HasOne(b => b.Blocked)
                .WithMany()
                .HasForeignKey(b => b.BlockedId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report → Reporter
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany()
                .HasForeignKey(r => r.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report → ReportedUser
            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedUser)
                .WithMany()
                .HasForeignKey(r => r.ReportedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report → ReportedMessage (nullable, SetNull if message deleted)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedMessage)
                .WithMany()
                .HasForeignKey(r => r.ReportedMessageId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== GEAR ITEM SEED DATA — Habitica-accurate (85 class + 108 special items) =====
            // Source: habitica-develop/website/common/script/content/gear/sets/
            // Names from: locales/en/gear.json. Stats from JS source files.
            // Wizard (mage) has NO shield — two-handed weapons fill both hands.
            // Rogue shield = off-hand weapon (STR, tiers 0-6 same text as main weapon).
            modelBuilder.Entity<GearItem>().HasData(

                // ── WARRIOR ──────────────────────────────────────────────────────────────
                // Weapons tier 0-6  (STR)
                new GearItem { Id =  1, Key = "weapon_warrior_0", Name = "Training Sword",   Slot = "weapon", GearClass = "warrior", Tier = 0, GoldCost =   1 },
                new GearItem { Id =  2, Key = "weapon_warrior_1", Name = "Sword",             Slot = "weapon", GearClass = "warrior", Tier = 1, GoldCost =  20, STR =  3 },
                new GearItem { Id =  3, Key = "weapon_warrior_2", Name = "Axe",               Slot = "weapon", GearClass = "warrior", Tier = 2, GoldCost =  30, STR =  6 },
                new GearItem { Id =  4, Key = "weapon_warrior_3", Name = "Morning Star",      Slot = "weapon", GearClass = "warrior", Tier = 3, GoldCost =  45, STR =  9 },
                new GearItem { Id =  5, Key = "weapon_warrior_4", Name = "Sapphire Blade",    Slot = "weapon", GearClass = "warrior", Tier = 4, GoldCost =  65, STR = 12 },
                new GearItem { Id =  6, Key = "weapon_warrior_5", Name = "Ruby Sword",        Slot = "weapon", GearClass = "warrior", Tier = 5, GoldCost =  90, STR = 15 },
                new GearItem { Id =  7, Key = "weapon_warrior_6", Name = "Golden Sword",      Slot = "weapon", GearClass = "warrior", Tier = 6, GoldCost = 120, STR = 18 },
                // Armor tier 1-5  (CON only)
                new GearItem { Id =  8, Key = "armor_warrior_1",  Name = "Leather Armor",     Slot = "armor",  GearClass = "warrior", Tier = 1, GoldCost =  30, CON =  3 },
                new GearItem { Id =  9, Key = "armor_warrior_2",  Name = "Chain Mail",        Slot = "armor",  GearClass = "warrior", Tier = 2, GoldCost =  45, CON =  5 },
                new GearItem { Id = 10, Key = "armor_warrior_3",  Name = "Plate Armor",       Slot = "armor",  GearClass = "warrior", Tier = 3, GoldCost =  65, CON =  7 },
                new GearItem { Id = 11, Key = "armor_warrior_4",  Name = "Red Armor",         Slot = "armor",  GearClass = "warrior", Tier = 4, GoldCost =  90, CON =  9 },
                new GearItem { Id = 12, Key = "armor_warrior_5",  Name = "Golden Armor",      Slot = "armor",  GearClass = "warrior", Tier = 5, GoldCost = 120, CON = 11 },
                // Head tier 1-5  (STR only)
                new GearItem { Id = 13, Key = "head_warrior_1",   Name = "Leather Helm",      Slot = "head",   GearClass = "warrior", Tier = 1, GoldCost =  15, STR =  2 },
                new GearItem { Id = 14, Key = "head_warrior_2",   Name = "Chain Coif",        Slot = "head",   GearClass = "warrior", Tier = 2, GoldCost =  25, STR =  4 },
                new GearItem { Id = 15, Key = "head_warrior_3",   Name = "Plate Helm",        Slot = "head",   GearClass = "warrior", Tier = 3, GoldCost =  40, STR =  6 },
                new GearItem { Id = 16, Key = "head_warrior_4",   Name = "Red Helm",          Slot = "head",   GearClass = "warrior", Tier = 4, GoldCost =  60, STR =  9 },
                new GearItem { Id = 17, Key = "head_warrior_5",   Name = "Golden Helm",       Slot = "head",   GearClass = "warrior", Tier = 5, GoldCost =  80, STR = 12 },
                // Shield tier 1-5  (CON only)
                new GearItem { Id = 18, Key = "shield_warrior_1", Name = "Wooden Shield",     Slot = "shield", GearClass = "warrior", Tier = 1, GoldCost =  20, CON =  2 },
                new GearItem { Id = 19, Key = "shield_warrior_2", Name = "Buckler",           Slot = "shield", GearClass = "warrior", Tier = 2, GoldCost =  35, CON =  3 },
                new GearItem { Id = 20, Key = "shield_warrior_3", Name = "Reinforced Shield", Slot = "shield", GearClass = "warrior", Tier = 3, GoldCost =  50, CON =  5 },
                new GearItem { Id = 21, Key = "shield_warrior_4", Name = "Red Shield",        Slot = "shield", GearClass = "warrior", Tier = 4, GoldCost =  70, CON =  7 },
                new GearItem { Id = 22, Key = "shield_warrior_5", Name = "Golden Shield",     Slot = "shield", GearClass = "warrior", Tier = 5, GoldCost =  90, CON =  9 },

                // ── MAGE (image key = "wizard"; NO shield — all weapons two-handed) ──────
                // Weapons tier 0-6  (INT + PER, two-handed)
                new GearItem { Id = 23, Key = "weapon_wizard_0",  Name = "Apprentice Staff",  Slot = "weapon", GearClass = "mage", Tier = 0, GoldCost =   0, TwoHanded = true },
                new GearItem { Id = 24, Key = "weapon_wizard_1",  Name = "Wooden Staff",      Slot = "weapon", GearClass = "mage", Tier = 1, GoldCost =  30, INT =  3, PER =  1, TwoHanded = true },
                new GearItem { Id = 25, Key = "weapon_wizard_2",  Name = "Jeweled Staff",     Slot = "weapon", GearClass = "mage", Tier = 2, GoldCost =  50, INT =  6, PER =  2, TwoHanded = true },
                new GearItem { Id = 26, Key = "weapon_wizard_3",  Name = "Iron Staff",        Slot = "weapon", GearClass = "mage", Tier = 3, GoldCost =  80, INT =  9, PER =  3, TwoHanded = true },
                new GearItem { Id = 27, Key = "weapon_wizard_4",  Name = "Brass Staff",       Slot = "weapon", GearClass = "mage", Tier = 4, GoldCost = 120, INT = 12, PER =  5, TwoHanded = true },
                new GearItem { Id = 28, Key = "weapon_wizard_5",  Name = "Archmage Staff",    Slot = "weapon", GearClass = "mage", Tier = 5, GoldCost = 160, INT = 15, PER =  7, TwoHanded = true },
                new GearItem { Id = 29, Key = "weapon_wizard_6",  Name = "Golden Staff",      Slot = "weapon", GearClass = "mage", Tier = 6, GoldCost = 200, INT = 18, PER = 10, TwoHanded = true },
                // Armor tier 1-5  (INT only)
                new GearItem { Id = 30, Key = "armor_wizard_1",   Name = "Magician Robe",     Slot = "armor",  GearClass = "mage", Tier = 1, GoldCost =  30, INT =  2 },
                new GearItem { Id = 31, Key = "armor_wizard_2",   Name = "Wizard Robe",       Slot = "armor",  GearClass = "mage", Tier = 2, GoldCost =  45, INT =  4 },
                new GearItem { Id = 32, Key = "armor_wizard_3",   Name = "Robe of Mysteries", Slot = "armor",  GearClass = "mage", Tier = 3, GoldCost =  65, INT =  6 },
                new GearItem { Id = 33, Key = "armor_wizard_4",   Name = "Archmage Robe",     Slot = "armor",  GearClass = "mage", Tier = 4, GoldCost =  90, INT =  9 },
                new GearItem { Id = 34, Key = "armor_wizard_5",   Name = "Royal Magus Robe",  Slot = "armor",  GearClass = "mage", Tier = 5, GoldCost = 120, INT = 12 },
                // Head tier 1-5  (PER only)
                new GearItem { Id = 35, Key = "head_wizard_1",    Name = "Magician Hat",      Slot = "head",   GearClass = "mage", Tier = 1, GoldCost =  15, PER =  2 },
                new GearItem { Id = 36, Key = "head_wizard_2",    Name = "Cornuthaum",        Slot = "head",   GearClass = "mage", Tier = 2, GoldCost =  25, PER =  3 },
                new GearItem { Id = 37, Key = "head_wizard_3",    Name = "Astrologer Hat",    Slot = "head",   GearClass = "mage", Tier = 3, GoldCost =  40, PER =  5 },
                new GearItem { Id = 38, Key = "head_wizard_4",    Name = "Archmage Hat",      Slot = "head",   GearClass = "mage", Tier = 4, GoldCost =  60, PER =  7 },
                new GearItem { Id = 39, Key = "head_wizard_5",    Name = "Royal Magus Hat",   Slot = "head",   GearClass = "mage", Tier = 5, GoldCost =  80, PER = 10 },
                // NO shield items for mage — two-handed weapons

                // ── ROGUE ────────────────────────────────────────────────────────────────
                // Weapons tier 0-6  (STR)
                new GearItem { Id = 40, Key = "weapon_rogue_0",   Name = "Dagger",            Slot = "weapon", GearClass = "rogue", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 41, Key = "weapon_rogue_1",   Name = "Short Sword",       Slot = "weapon", GearClass = "rogue", Tier = 1, GoldCost =  20, STR =  2 },
                new GearItem { Id = 42, Key = "weapon_rogue_2",   Name = "Scimitar",          Slot = "weapon", GearClass = "rogue", Tier = 2, GoldCost =  35, STR =  3 },
                new GearItem { Id = 43, Key = "weapon_rogue_3",   Name = "Kukri",             Slot = "weapon", GearClass = "rogue", Tier = 3, GoldCost =  50, STR =  4 },
                new GearItem { Id = 44, Key = "weapon_rogue_4",   Name = "Nunchaku",          Slot = "weapon", GearClass = "rogue", Tier = 4, GoldCost =  70, STR =  6 },
                new GearItem { Id = 45, Key = "weapon_rogue_5",   Name = "Ninja-to",          Slot = "weapon", GearClass = "rogue", Tier = 5, GoldCost =  90, STR =  8 },
                new GearItem { Id = 46, Key = "weapon_rogue_6",   Name = "Hook Sword",        Slot = "weapon", GearClass = "rogue", Tier = 6, GoldCost = 120, STR = 10 },
                // Armor tier 1-5  (PER only)
                new GearItem { Id = 47, Key = "armor_rogue_1",    Name = "Oiled Leather",     Slot = "armor",  GearClass = "rogue", Tier = 1, GoldCost =  30, PER =  6 },
                new GearItem { Id = 48, Key = "armor_rogue_2",    Name = "Black Leather",     Slot = "armor",  GearClass = "rogue", Tier = 2, GoldCost =  45, PER =  9 },
                new GearItem { Id = 49, Key = "armor_rogue_3",    Name = "Camouflage Vest",   Slot = "armor",  GearClass = "rogue", Tier = 3, GoldCost =  65, PER = 12 },
                new GearItem { Id = 50, Key = "armor_rogue_4",    Name = "Penumbral Armor",   Slot = "armor",  GearClass = "rogue", Tier = 4, GoldCost =  90, PER = 15 },
                new GearItem { Id = 51, Key = "armor_rogue_5",    Name = "Umbral Armor",      Slot = "armor",  GearClass = "rogue", Tier = 5, GoldCost = 120, PER = 18 },
                // Head tier 1-5  (PER only)
                new GearItem { Id = 52, Key = "head_rogue_1",     Name = "Leather Hood",      Slot = "head",   GearClass = "rogue", Tier = 1, GoldCost =  15, PER =  2 },
                new GearItem { Id = 53, Key = "head_rogue_2",     Name = "Black Leather Hood",Slot = "head",   GearClass = "rogue", Tier = 2, GoldCost =  25, PER =  4 },
                new GearItem { Id = 54, Key = "head_rogue_3",     Name = "Camouflage Hood",   Slot = "head",   GearClass = "rogue", Tier = 3, GoldCost =  40, PER =  6 },
                new GearItem { Id = 55, Key = "head_rogue_4",     Name = "Penumbral Hood",    Slot = "head",   GearClass = "rogue", Tier = 4, GoldCost =  60, PER =  9 },
                new GearItem { Id = 56, Key = "head_rogue_5",     Name = "Umbral Hood",       Slot = "head",   GearClass = "rogue", Tier = 5, GoldCost =  80, PER = 12 },
                // Shield tier 0-6  (off-hand weapon, STR — same names as main weapon)
                new GearItem { Id = 57, Key = "shield_rogue_0",   Name = "Dagger",            Slot = "shield", GearClass = "rogue", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 58, Key = "shield_rogue_1",   Name = "Short Sword",       Slot = "shield", GearClass = "rogue", Tier = 1, GoldCost =  20, STR =  2 },
                new GearItem { Id = 59, Key = "shield_rogue_2",   Name = "Scimitar",          Slot = "shield", GearClass = "rogue", Tier = 2, GoldCost =  35, STR =  3 },
                new GearItem { Id = 60, Key = "shield_rogue_3",   Name = "Kukri",             Slot = "shield", GearClass = "rogue", Tier = 3, GoldCost =  50, STR =  4 },
                new GearItem { Id = 61, Key = "shield_rogue_4",   Name = "Nunchaku",          Slot = "shield", GearClass = "rogue", Tier = 4, GoldCost =  70, STR =  6 },
                new GearItem { Id = 62, Key = "shield_rogue_5",   Name = "Ninja-to",          Slot = "shield", GearClass = "rogue", Tier = 5, GoldCost =  90, STR =  8 },
                new GearItem { Id = 63, Key = "shield_rogue_6",   Name = "Hook Sword",        Slot = "shield", GearClass = "rogue", Tier = 6, GoldCost = 120, STR = 10 },

                // ── HEALER ───────────────────────────────────────────────────────────────
                // Weapons tier 0-6  (INT)
                new GearItem { Id = 64, Key = "weapon_healer_0",  Name = "Novice Rod",        Slot = "weapon", GearClass = "healer", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 65, Key = "weapon_healer_1",  Name = "Acolyte Rod",       Slot = "weapon", GearClass = "healer", Tier = 1, GoldCost =  20, INT =  2 },
                new GearItem { Id = 66, Key = "weapon_healer_2",  Name = "Quartz Rod",        Slot = "weapon", GearClass = "healer", Tier = 2, GoldCost =  30, INT =  3 },
                new GearItem { Id = 67, Key = "weapon_healer_3",  Name = "Amethyst Rod",      Slot = "weapon", GearClass = "healer", Tier = 3, GoldCost =  45, INT =  5 },
                new GearItem { Id = 68, Key = "weapon_healer_4",  Name = "Physician Rod",     Slot = "weapon", GearClass = "healer", Tier = 4, GoldCost =  65, INT =  7 },
                new GearItem { Id = 69, Key = "weapon_healer_5",  Name = "Royal Scepter",     Slot = "weapon", GearClass = "healer", Tier = 5, GoldCost =  90, INT =  9 },
                new GearItem { Id = 70, Key = "weapon_healer_6",  Name = "Golden Scepter",    Slot = "weapon", GearClass = "healer", Tier = 6, GoldCost = 120, INT = 11 },
                // Armor tier 1-5  (CON only)
                new GearItem { Id = 71, Key = "armor_healer_1",   Name = "Acolyte Robe",      Slot = "armor",  GearClass = "healer", Tier = 1, GoldCost =  30, CON =  6 },
                new GearItem { Id = 72, Key = "armor_healer_2",   Name = "Medic Robe",        Slot = "armor",  GearClass = "healer", Tier = 2, GoldCost =  45, CON =  9 },
                new GearItem { Id = 73, Key = "armor_healer_3",   Name = "Defender Mantle",   Slot = "armor",  GearClass = "healer", Tier = 3, GoldCost =  65, CON = 12 },
                new GearItem { Id = 74, Key = "armor_healer_4",   Name = "Physician Mantle",  Slot = "armor",  GearClass = "healer", Tier = 4, GoldCost =  90, CON = 15 },
                new GearItem { Id = 75, Key = "armor_healer_5",   Name = "Royal Mantle",      Slot = "armor",  GearClass = "healer", Tier = 5, GoldCost = 120, CON = 18 },
                // Head tier 1-5  (INT only)
                new GearItem { Id = 76, Key = "head_healer_1",    Name = "Quartz Circlet",    Slot = "head",   GearClass = "healer", Tier = 1, GoldCost =  15, INT =  2 },
                new GearItem { Id = 77, Key = "head_healer_2",    Name = "Amethyst Circlet",  Slot = "head",   GearClass = "healer", Tier = 2, GoldCost =  25, INT =  3 },
                new GearItem { Id = 78, Key = "head_healer_3",    Name = "Sapphire Circlet",  Slot = "head",   GearClass = "healer", Tier = 3, GoldCost =  40, INT =  5 },
                new GearItem { Id = 79, Key = "head_healer_4",    Name = "Emerald Diadem",    Slot = "head",   GearClass = "healer", Tier = 4, GoldCost =  60, INT =  7 },
                new GearItem { Id = 80, Key = "head_healer_5",    Name = "Royal Diadem",      Slot = "head",   GearClass = "healer", Tier = 5, GoldCost =  80, INT =  9 },
                // Shield tier 1-5  (CON only)
                new GearItem { Id = 81, Key = "shield_healer_1",  Name = "Medic Buckler",     Slot = "shield", GearClass = "healer", Tier = 1, GoldCost =  20, CON =  2 },
                new GearItem { Id = 82, Key = "shield_healer_2",  Name = "Kite Shield",       Slot = "shield", GearClass = "healer", Tier = 2, GoldCost =  35, CON =  4 },
                new GearItem { Id = 83, Key = "shield_healer_3",  Name = "Protector Shield",  Slot = "shield", GearClass = "healer", Tier = 3, GoldCost =  50, CON =  6 },
                new GearItem { Id = 84, Key = "shield_healer_4",  Name = "Savior Shield",     Slot = "shield", GearClass = "healer", Tier = 4, GoldCost =  70, CON =  9 },
                new GearItem { Id = 85, Key = "shield_healer_5",  Name = "Royal Shield",      Slot = "shield", GearClass = "healer", Tier = 5, GoldCost =  90, CON = 12 },

                // ── SPECIAL — WEAPON (17 items, IDs 552–568) ─────────────────────────────
                new GearItem { Id = 552, Key = "weapon_special_0",                   Name = "Dark Souls Blade",                       Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 150, STR = 20 },
                new GearItem { Id = 553, Key = "weapon_special_1",                   Name = "Crystal Blade",                          Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 170, STR = 6, CON = 6, INT = 6, PER = 6 },
                new GearItem { Id = 554, Key = "weapon_special_2",                   Name = "Stephen Weber's Shaft of the Dragon",     Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 200, STR = 25, PER = 25 },
                new GearItem { Id = 555, Key = "weapon_special_3",                   Name = "Mustaine's Milestone Mashing Morning Star",Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 200, STR = 17, INT = 17, CON = 17 },
                new GearItem { Id = 556, Key = "weapon_special_aetherCrystals",      Name = "Aether Crystals",                        Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 175, STR = 10, CON = 10, INT = 10, PER = 10, TwoHanded = true },
                new GearItem { Id = 557, Key = "weapon_special_bardInstrument",      Name = "Bardic Lute",                            Slot = "weapon", GearClass = "special", Tier = 0, GoldCost =   0, INT = 4, PER = 4, TwoHanded = true },
                new GearItem { Id = 558, Key = "weapon_special_critical",            Name = "Critical Hammer of Bug-Crushing",        Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 200, STR = 40, PER = 40 },
                new GearItem { Id = 559, Key = "weapon_special_fencingFoil",         Name = "Fencing Foil",                           Slot = "weapon", GearClass = "special", Tier = 0, GoldCost =   0, STR = 16 },
                new GearItem { Id = 560, Key = "weapon_special_lunarScythe",         Name = "Lunar Scythe",                           Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 130, STR = 7, PER = 7, TwoHanded = true },
                new GearItem { Id = 561, Key = "weapon_special_mammothRiderSpear",   Name = "Mammoth Rider Spear",                    Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 130, INT = 15 },
                new GearItem { Id = 562, Key = "weapon_special_nomadsScimitar",      Name = "Nomad's Scimitar",                       Slot = "weapon", GearClass = "special", Tier = 0, GoldCost =   0, INT = 16 },
                new GearItem { Id = 563, Key = "weapon_special_pageBanner",          Name = "Page Banner",                            Slot = "weapon", GearClass = "special", Tier = 0, GoldCost =   0, STR = 16 },
                new GearItem { Id = 564, Key = "weapon_special_roguishRainbowMessage",Name = "Roguish Rainbow Message",               Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 130, PER = 15 },
                new GearItem { Id = 565, Key = "weapon_special_skeletonKey",         Name = "Skeleton Key",                           Slot = "weapon", GearClass = "special", Tier = 0, GoldCost =   0, CON = 16 },
                new GearItem { Id = 566, Key = "weapon_special_tachi",               Name = "Tachi",                                  Slot = "weapon", GearClass = "special", Tier = 0, GoldCost =   0, STR = 17 },
                new GearItem { Id = 567, Key = "weapon_special_taskwoodsLantern",    Name = "Taskwoods Lantern",                      Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 130, PER = 15, INT = 15, TwoHanded = true },
                new GearItem { Id = 568, Key = "weapon_special_tridentOfCrashingTides",Name = "Trident of Crashing Tides",           Slot = "weapon", GearClass = "special", Tier = 0, GoldCost = 130, INT = 15 },

                // ── SPECIAL — ARMOR (18 items, IDs 569–586) ──────────────────────────────
                new GearItem { Id = 569, Key = "armor_special_0",                           Name = "Shade Armor",                       Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 150, CON = 20 },
                new GearItem { Id = 570, Key = "armor_special_1",                           Name = "Crystal Armor",                     Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 170, CON = 6, STR = 6, PER = 6, INT = 6 },
                new GearItem { Id = 571, Key = "armor_special_2",                           Name = "Jean Chalard's Noble Tunic",        Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 200, INT = 25, CON = 25 },
                new GearItem { Id = 572, Key = "armor_special_bardRobes",                   Name = "Bardic Robes",                      Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0, PER = 3 },
                new GearItem { Id = 573, Key = "armor_special_dandySuit",                   Name = "Dandy Suit",                        Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0, PER = 17 },
                new GearItem { Id = 574, Key = "armor_special_finnedOceanicArmor",          Name = "Finned Oceanic Armor",              Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 130, STR = 15 },
                new GearItem { Id = 575, Key = "armor_special_heroicTunic",                 Name = "Heroic Tunic",                      Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 175, CON = 7, STR = 7, PER = 7, INT = 7 },
                new GearItem { Id = 576, Key = "armor_special_lunarWarriorArmor",           Name = "Lunar Warrior Armor",               Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 130, STR = 7, CON = 7 },
                new GearItem { Id = 577, Key = "armor_special_mammothRiderArmor",           Name = "Mammoth Rider Armor",               Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 130, CON = 15 },
                new GearItem { Id = 578, Key = "armor_special_nomadsCuirass",               Name = "Nomad's Cuirass",                   Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0, CON = 17 },
                new GearItem { Id = 579, Key = "armor_special_pageArmor",                   Name = "Page Armor",                        Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0, CON = 16 },
                new GearItem { Id = 580, Key = "armor_special_pyromancersRobes",            Name = "Pyromancer's Robes",                Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 130, CON = 15 },
                new GearItem { Id = 581, Key = "armor_special_roguishRainbowMessengerRobes",Name = "Roguish Rainbow Messenger Robes",   Slot = "armor", GearClass = "special", Tier = 0, GoldCost = 130, STR = 15 },
                new GearItem { Id = 582, Key = "armor_special_samuraiArmor",                Name = "Samurai Armor",                     Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0, PER = 17 },
                new GearItem { Id = 583, Key = "armor_special_sneakthiefRobes",             Name = "Sneakthief Robes",                  Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0, INT = 16 },
                new GearItem { Id = 584, Key = "armor_special_snowSovereignRobes",          Name = "Snow Sovereign Robes",              Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0, PER = 17 },
                new GearItem { Id = 585, Key = "armor_special_turkeyArmorBase",             Name = "Turkey Armor",                      Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 586, Key = "armor_special_turkeyArmorGilded",           Name = "Gilded Turkey Armor",               Slot = "armor", GearClass = "special", Tier = 0, GoldCost =   0 },

                // ── SPECIAL — HEAD (18 items, IDs 587–604) ───────────────────────────────
                new GearItem { Id = 587, Key = "head_special_0",                           Name = "Shade Helm",                        Slot = "head", GearClass = "special", Tier = 0, GoldCost = 150, INT = 20 },
                new GearItem { Id = 588, Key = "head_special_1",                           Name = "Crystal Helm",                      Slot = "head", GearClass = "special", Tier = 0, GoldCost = 170, CON = 6, STR = 6, PER = 6, INT = 6 },
                new GearItem { Id = 589, Key = "head_special_2",                           Name = "Nameless Helm",                     Slot = "head", GearClass = "special", Tier = 0, GoldCost = 200, INT = 25, STR = 25 },
                new GearItem { Id = 590, Key = "head_special_bardHat",                     Name = "Bardic Cap",                        Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0, INT = 3 },
                new GearItem { Id = 591, Key = "head_special_clandestineCowl",             Name = "Clandestine Cowl",                  Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0, PER = 16 },
                new GearItem { Id = 592, Key = "head_special_dandyHat",                    Name = "Dandy Hat",                         Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0, CON = 17 },
                new GearItem { Id = 593, Key = "head_special_fireCoralCirclet",            Name = "Fire Coral Circlet",                Slot = "head", GearClass = "special", Tier = 0, GoldCost = 130, PER = 15 },
                new GearItem { Id = 594, Key = "head_special_kabuto",                      Name = "Kabuto",                            Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0, INT = 17 },
                new GearItem { Id = 595, Key = "head_special_lunarWarriorHelm",            Name = "Lunar Warrior Helm",                Slot = "head", GearClass = "special", Tier = 0, GoldCost = 130, INT = 7, STR = 7 },
                new GearItem { Id = 596, Key = "head_special_mammothRiderHelm",            Name = "Mammoth Rider Helm",                Slot = "head", GearClass = "special", Tier = 0, GoldCost = 130, PER = 15 },
                new GearItem { Id = 597, Key = "head_special_namingDay2017",               Name = "Royal Purple Gryphon Helm",         Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 598, Key = "head_special_pageHelm",                    Name = "Page Helm",                         Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0, PER = 16 },
                new GearItem { Id = 599, Key = "head_special_pyromancersTurban",           Name = "Pyromancer's Turban",               Slot = "head", GearClass = "special", Tier = 0, GoldCost = 130, STR = 15 },
                new GearItem { Id = 600, Key = "head_special_roguishRainbowMessengerHood", Name = "Roguish Rainbow Messenger Hood",    Slot = "head", GearClass = "special", Tier = 0, GoldCost = 130, CON = 15 },
                new GearItem { Id = 601, Key = "head_special_snowSovereignCrown",          Name = "Snow Sovereign Crown",              Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0, CON = 16 },
                new GearItem { Id = 602, Key = "head_special_spikedHelm",                  Name = "Spiked Helm",                       Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0, STR = 16 },
                new GearItem { Id = 603, Key = "head_special_turkeyHelmBase",              Name = "Turkey Helm",                       Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 604, Key = "head_special_turkeyHelmGilded",            Name = "Gilded Turkey Helm",                Slot = "head", GearClass = "special", Tier = 0, GoldCost =   0 },

                // ── SPECIAL — SHIELD (10 items, IDs 605–614) ─────────────────────────────
                new GearItem { Id = 605, Key = "shield_special_0",                  Name = "Tormented Skull",                          Slot = "shield", GearClass = "special", Tier = 0, GoldCost = 150, PER = 20 },
                new GearItem { Id = 606, Key = "shield_special_1",                  Name = "Crystal Shield",                           Slot = "shield", GearClass = "special", Tier = 0, GoldCost = 170, CON = 6, STR = 6, PER = 6, INT = 6 },
                new GearItem { Id = 607, Key = "shield_special_diamondStave",       Name = "Diamond Stave",                            Slot = "shield", GearClass = "special", Tier = 0, GoldCost =   0, INT = 16 },
                new GearItem { Id = 608, Key = "shield_special_goldenknight",       Name = "Mustaine's Milestone Mashing Morning Star", Slot = "shield", GearClass = "special", Tier = 0, GoldCost = 200, CON = 25, PER = 25 },
                new GearItem { Id = 609, Key = "shield_special_lootBag",            Name = "Loot Bag",                                 Slot = "shield", GearClass = "special", Tier = 0, GoldCost =   0, STR = 16 },
                new GearItem { Id = 610, Key = "shield_special_mammothRiderHorn",   Name = "Mammoth Rider's Horn",                     Slot = "shield", GearClass = "special", Tier = 0, GoldCost = 130, STR = 15 },
                new GearItem { Id = 611, Key = "shield_special_moonpearlShield",    Name = "Moonpearl Shield",                         Slot = "shield", GearClass = "special", Tier = 0, GoldCost = 130, CON = 15 },
                new GearItem { Id = 612, Key = "shield_special_roguishRainbowMessage",Name = "Roguish Rainbow Message",                Slot = "shield", GearClass = "special", Tier = 0, GoldCost = 130, INT = 15 },
                new GearItem { Id = 613, Key = "shield_special_wakizashi",          Name = "Wakizashi",                                Slot = "shield", GearClass = "special", Tier = 0, GoldCost =   0, CON = 17 },
                new GearItem { Id = 614, Key = "shield_special_wintryMirror",       Name = "Wintry Mirror",                            Slot = "shield", GearClass = "special", Tier = 0, GoldCost =   0, INT = 16 },

                // ── SPECIAL — BACK (14 items, IDs 615–628) ───────────────────────────────
                new GearItem { Id = 615, Key = "back_special_aetherCloak",    Name = "Aether Cloak",              Slot = "back", GearClass = "special", Tier = 0, GoldCost = 175, PER = 10 },
                new GearItem { Id = 616, Key = "back_special_bearTail",       Name = "Bear Tail",                 Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 617, Key = "back_special_cactusTail",     Name = "Cactus Tail",               Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 618, Key = "back_special_foxTail",        Name = "Fox Tail",                  Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 619, Key = "back_special_heroicAureole",  Name = "Heroic Aureole",            Slot = "back", GearClass = "special", Tier = 0, GoldCost = 175, CON = 7, STR = 7, PER = 7, INT = 7 },
                new GearItem { Id = 620, Key = "back_special_lionTail",       Name = "Lion Tail",                 Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 621, Key = "back_special_namingDay2020",  Name = "Royal Purple Gryphon Tail", Slot = "back", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 622, Key = "back_special_pandaTail",      Name = "Panda Tail",                Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 623, Key = "back_special_pigTail",        Name = "Pig Tail",                  Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 624, Key = "back_special_snowdriftVeil",  Name = "Snowdrift Veil",            Slot = "back", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 625, Key = "back_special_tigerTail",      Name = "Tiger Tail",                Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 626, Key = "back_special_turkeyTailBase", Name = "Turkey Tail",               Slot = "back", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 627, Key = "back_special_turkeyTailGilded",Name = "Gilded Turkey Tail",       Slot = "back", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 628, Key = "back_special_wolfTail",       Name = "Wolf Tail",                 Slot = "back", GearClass = "special", Tier = 0, GoldCost =  20 },

                // ── SPECIAL — EYEWEAR (15 items, IDs 629–643) ────────────────────────────
                new GearItem { Id = 629, Key = "eyewear_special_aetherMask",     Name = "Aether Mask",                Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost = 175, INT = 10 },
                new GearItem { Id = 630, Key = "eyewear_special_blackHalfMoon",  Name = "Black Half-Moon Eyeglasses", Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 631, Key = "eyewear_special_blackTopFrame",  Name = "Black Standard Eyeglasses",  Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 632, Key = "eyewear_special_blueHalfMoon",   Name = "Blue Half-Moon Eyeglasses",  Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 633, Key = "eyewear_special_blueTopFrame",   Name = "Blue Standard Eyeglasses",   Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 634, Key = "eyewear_special_greenHalfMoon",  Name = "Green Half-Moon Eyeglasses", Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 635, Key = "eyewear_special_greenTopFrame",  Name = "Green Standard Eyeglasses",  Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 636, Key = "eyewear_special_pinkHalfMoon",   Name = "Pink Half-Moon Eyeglasses",  Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 637, Key = "eyewear_special_pinkTopFrame",   Name = "Pink Standard Eyeglasses",   Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 638, Key = "eyewear_special_redHalfMoon",    Name = "Red Half-Moon Eyeglasses",   Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 639, Key = "eyewear_special_redTopFrame",    Name = "Red Standard Eyeglasses",    Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 640, Key = "eyewear_special_whiteHalfMoon",  Name = "White Half-Moon Eyeglasses", Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 641, Key = "eyewear_special_whiteTopFrame",  Name = "White Standard Eyeglasses",  Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 642, Key = "eyewear_special_yellowHalfMoon", Name = "Yellow Half-Moon Eyeglasses",Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 643, Key = "eyewear_special_yellowTopFrame", Name = "Yellow Standard Eyeglasses", Slot = "eyewear", GearClass = "special", Tier = 0, GoldCost =   0 },

                // ── SPECIAL — HEAD ACCESSORY (16 items, IDs 644–659) ─────────────────────
                new GearItem { Id = 644, Key = "headAccessory_special_bearEars",      Name = "Bear Ears",       Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 645, Key = "headAccessory_special_blackHeadband", Name = "Black Headband",  Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 646, Key = "headAccessory_special_blueHeadband",  Name = "Blue Headband",   Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 647, Key = "headAccessory_special_cactusEars",    Name = "Cactus Ears",     Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 648, Key = "headAccessory_special_foxEars",       Name = "Fox Ears",        Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 649, Key = "headAccessory_special_greenHeadband", Name = "Green Headband",  Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 650, Key = "headAccessory_special_heroicCirclet", Name = "Heroic Circlet",  Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost = 175, CON = 7, STR = 7, PER = 7, INT = 7 },
                new GearItem { Id = 651, Key = "headAccessory_special_lionEars",      Name = "Lion Ears",       Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 652, Key = "headAccessory_special_pandaEars",     Name = "Panda Ears",      Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 653, Key = "headAccessory_special_pigEars",       Name = "Pig Ears",        Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 654, Key = "headAccessory_special_pinkHeadband",  Name = "Pink Headband",   Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 655, Key = "headAccessory_special_redHeadband",   Name = "Red Headband",    Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 656, Key = "headAccessory_special_tigerEars",     Name = "Tiger Ears",      Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 657, Key = "headAccessory_special_whiteHeadband", Name = "White Headband",  Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =   0 },
                new GearItem { Id = 658, Key = "headAccessory_special_wolfEars",      Name = "Wolf Ears",       Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =  20 },
                new GearItem { Id = 659, Key = "headAccessory_special_yellowHeadband",Name = "Yellow Headband", Slot = "headAccessory", GearClass = "special", Tier = 0, GoldCost =   0 },

                // ===== ARMOIRE GEAR (466 items) =====
                new GearItem { Id = 86, Key = "armor_armoire_lunarArmor", Name = "Lunar Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 87, Key = "armor_armoire_gladiatorArmor", Name = "Gladiator Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 88, Key = "armor_armoire_rancherRobes", Name = "Rancher Robes", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 89, Key = "armor_armoire_goldenToga", Name = "Golden Toga", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 90, Key = "armor_armoire_hornedIronArmor", Name = "Horned Iron Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 91, Key = "armor_armoire_plagueDoctorOvercoat", Name = "Plague Doctor Overcoat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 6, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 92, Key = "armor_armoire_shepherdRobes", Name = "Shepherd Robes", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 9, CON = 0, INT = 0, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 93, Key = "armor_armoire_royalRobes", Name = "Royal Robes", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 94, Key = "armor_armoire_crystalCrescentRobes", Name = "Crystal Crescent Robes", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 95, Key = "armor_armoire_dragonTamerArmor", Name = "Dragon Tamer Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 96, Key = "armor_armoire_barristerRobes", Name = "Barrister Robes", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 97, Key = "armor_armoire_jesterCostume", Name = "Jester Costume", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 15, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 98, Key = "armor_armoire_minerOveralls", Name = "Miner Overalls", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 99, Key = "armor_armoire_basicArcherArmor", Name = "Basic Archer Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 100, Key = "armor_armoire_graduateRobe", Name = "Graduate Robe", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 101, Key = "armor_armoire_stripedSwimsuit", Name = "Striped Swimsuit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 13, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 102, Key = "armor_armoire_cannoneerRags", Name = "Cannoneer Rags", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 103, Key = "armor_armoire_falconerArmor", Name = "Falconer Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 104, Key = "armor_armoire_vermilionArcherArmor", Name = "Vermilion Archer Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 105, Key = "armor_armoire_ogreArmor", Name = "Ogre Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 106, Key = "armor_armoire_ironBlueArcherArmor", Name = "Iron Blue Archer Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 12, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 107, Key = "armor_armoire_redPartyDress", Name = "Red Party Dress", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 108, Key = "armor_armoire_woodElfArmor", Name = "Wood Elf Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 109, Key = "armor_armoire_ramFleeceRobes", Name = "Ram Fleece Robes", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 110, Key = "armor_armoire_gownOfHearts", Name = "Gown Of Hearts", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 13, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 111, Key = "armor_armoire_mushroomDruidArmor", Name = "Mushroom Druid Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 112, Key = "armor_armoire_greenFestivalYukata", Name = "Green Festival Yukata", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 113, Key = "armor_armoire_merchantTunic", Name = "Merchant Tunic", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 114, Key = "armor_armoire_vikingTunic", Name = "Viking Tunic", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 115, Key = "armor_armoire_swanDancerTutu", Name = "Swan Dancer Tutu", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 116, Key = "armor_armoire_yellowPartyDress", Name = "Yellow Party Dress", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 117, Key = "armor_armoire_antiProcrastinationArmor", Name = "Anti Procrastination Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 118, Key = "armor_armoire_farrierOutfit", Name = "Farrier Outfit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 6, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 119, Key = "armor_armoire_candlestickMakerOutfit", Name = "Candlestick Maker Outfit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 120, Key = "armor_armoire_wovenRobes", Name = "Woven Robes", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 9, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 121, Key = "armor_armoire_lamplightersGreatcoat", Name = "Lamplighters Greatcoat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 14, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 122, Key = "armor_armoire_coachDriverLivery", Name = "Coach Driver Livery", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 12, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 123, Key = "armor_armoire_robeOfDiamonds", Name = "Robe Of Diamonds", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 13, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 124, Key = "armor_armoire_flutteryFrock", Name = "Fluttery Frock", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 125, Key = "armor_armoire_cobblersCoveralls", Name = "Cobblers Coveralls", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 126, Key = "armor_armoire_glassblowersCoveralls", Name = "Glassblowers Coveralls", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 127, Key = "armor_armoire_bluePartyDress", Name = "Blue Party Dress", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 128, Key = "armor_armoire_piraticalPrincessGown", Name = "Piratical Princess Gown", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 129, Key = "armor_armoire_jeweledArcherArmor", Name = "Jeweled Archer Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 130, Key = "armor_armoire_coverallsOfBookbinding", Name = "Coveralls Of Bookbinding", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 131, Key = "armor_armoire_robeOfSpades", Name = "Robe Of Spades", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 13, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 132, Key = "armor_armoire_softBlueSuit", Name = "Soft Blue Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 133, Key = "armor_armoire_softGreenSuit", Name = "Soft Green Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 134, Key = "armor_armoire_softRedSuit", Name = "Soft Red Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 135, Key = "armor_armoire_scribesRobe", Name = "Scribes Robe", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 136, Key = "armor_armoire_chefsJacket", Name = "Chefs Jacket", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 137, Key = "armor_armoire_vernalVestment", Name = "Vernal Vestment", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 138, Key = "armor_armoire_nephriteArmor", Name = "Nephrite Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 139, Key = "armor_armoire_boatingJacket", Name = "Boating Jacket", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 6, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 140, Key = "armor_armoire_astronomersRobe", Name = "Astronomers Robe", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 141, Key = "armor_armoire_invernessCape", Name = "Inverness Cape", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 142, Key = "armor_armoire_shadowMastersRobe", Name = "Shadow Masters Robe", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 143, Key = "armor_armoire_alchemistsRobe", Name = "Alchemists Robe", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 144, Key = "armor_armoire_duffleCoat", Name = "Duffle Coat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 145, Key = "armor_armoire_layerCakeArmor", Name = "Layer Cake Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 13, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 146, Key = "armor_armoire_matchMakersApron", Name = "Match Makers Apron", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 147, Key = "armor_armoire_baseballUniform", Name = "Baseball Uniform", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 148, Key = "armor_armoire_boxArmor", Name = "Box Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 149, Key = "armor_armoire_fiddlersCoat", Name = "Fiddlers Coat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 150, Key = "armor_armoire_pirateOutfit", Name = "Pirate Outfit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 4, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 151, Key = "armor_armoire_heroicHerbalistRobe", Name = "Heroic Herbalist Robe", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 152, Key = "armor_armoire_guardiansGown", Name = "Guardians Gown", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 153, Key = "armor_armoire_autumnEnchantersCloak", Name = "Autumn Enchanters Cloak", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 154, Key = "armor_armoire_doubletOfClubs", Name = "Doublet Of Clubs", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 155, Key = "armor_armoire_dressingGown", Name = "Dressing Gown", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 156, Key = "armor_armoire_blueMoonShozoku", Name = "Blue Moon Shozoku", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 157, Key = "armor_armoire_softPinkSuit", Name = "Soft Pink Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 158, Key = "armor_armoire_jadeArmor", Name = "Jade Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 159, Key = "armor_armoire_clownsMotley", Name = "Clowns Motley", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 160, Key = "armor_armoire_medievalLaundryOutfit", Name = "Medieval Laundry Outfit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 161, Key = "armor_armoire_medievalLaundryDress", Name = "Medieval Laundry Dress", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 162, Key = "armor_armoire_bathtub", Name = "Bathtub", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 163, Key = "armor_armoire_bagpipersKilt", Name = "Bagpipers Kilt", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 164, Key = "armor_armoire_heraldsTunic", Name = "Heralds Tunic", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 165, Key = "armor_armoire_softBlackSuit", Name = "Soft Black Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 166, Key = "armor_armoire_shootingStarCostume", Name = "Shooting Star Costume", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 167, Key = "armor_armoire_softVioletSuit", Name = "Soft Violet Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 168, Key = "armor_armoire_gardenersOveralls", Name = "Gardeners Overalls", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 169, Key = "armor_armoire_strawRaincoat", Name = "Straw Raincoat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 170, Key = "armor_armoire_fancyPirateSuit", Name = "Fancy Pirate Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 4, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 171, Key = "armor_armoire_sheetGhostCostume", Name = "Sheet Ghost Costume", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 172, Key = "armor_armoire_jewelersApron", Name = "Jewelers Apron", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 173, Key = "armor_armoire_shawlCollarCoat", Name = "Shawl Collar Coat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 174, Key = "armor_armoire_teaGown", Name = "Tea Gown", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 175, Key = "armor_armoire_basketballUniform", Name = "Basketball Uniform", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 176, Key = "armor_armoire_paintersApron", Name = "Painters Apron", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 177, Key = "armor_armoire_stripedRainbowShirt", Name = "Striped Rainbow Shirt", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 178, Key = "armor_armoire_diagonalRainbowShirt", Name = "Diagonal Rainbow Shirt", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 179, Key = "armor_armoire_admiralsUniform", Name = "Admirals Uniform", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 180, Key = "armor_armoire_karateGi", Name = "Karate Gi", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 181, Key = "armor_armoire_greenFluffTrimmedCoat", Name = "Green Fluff Trimmed Coat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 182, Key = "armor_armoire_schoolUniformSkirt", Name = "School Uniform Skirt", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 183, Key = "armor_armoire_schoolUniformPants", Name = "School Uniform Pants", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 184, Key = "armor_armoire_softWhiteSuit", Name = "Soft White Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 185, Key = "armor_armoire_hattersSuit", Name = "Hatters Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 186, Key = "armor_armoire_smileyShirt", Name = "Smiley Shirt", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 4, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 187, Key = "armor_armoire_pottersApron", Name = "Potters Apron", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 188, Key = "armor_armoire_yellowStripedSwimsuit", Name = "Yellow Striped Swimsuit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 13, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 189, Key = "armor_armoire_blueStripedSwimsuit", Name = "Blue Striped Swimsuit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 13, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 190, Key = "armor_armoire_corsairsCoatAndCape", Name = "Corsairs Coat And Cape", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 14, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 191, Key = "armor_armoire_dragonKnightsArmor", Name = "Dragon Knights Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 192, Key = "armor_armoire_funnyFoolCostume", Name = "Funny Fool Costume", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 193, Key = "armor_armoire_stormKnightArmor", Name = "Storm Knight Armor", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 11, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 194, Key = "armor_armoire_festiveHelperOveralls", Name = "Festive Helper Overalls", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 195, Key = "armor_armoire_snowyFluffTrimmedCoat", Name = "Snowy Fluff Trimmed Coat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 196, Key = "armor_armoire_springPetalYukata", Name = "Spring Petal Yukata", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 197, Key = "armor_armoire_sillyOrangeTuxedo", Name = "Silly Orange Tuxedo", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 198, Key = "armor_armoire_sillierBlueTuxedo", Name = "Sillier Blue Tuxedo", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 12, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 199, Key = "armor_armoire_gildedKnightsPlate", Name = "Gilded Knights Plate", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 11, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 200, Key = "armor_armoire_beekeepersSuit", Name = "Beekeepers Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 201, Key = "armor_armoire_flyFishingWaders", Name = "Fly Fishing Waders", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 202, Key = "armor_armoire_redWaistcoat", Name = "Red Waistcoat", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 203, Key = "armor_armoire_softOrangeSuit", Name = "Soft Orange Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 204, Key = "armor_armoire_blackPartyDress", Name = "Black Party Dress", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 205, Key = "armor_armoire_blacksmithsApron", Name = "Blacksmiths Apron", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 11, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 206, Key = "armor_armoire_loneCowpokeOutfit", Name = "Lone Cowpoke Outfit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 207, Key = "armor_armoire_softYellowSuit", Name = "Soft Yellow Suit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 9, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 208, Key = "armor_armoire_handstandOutfit", Name = "Handstand Outfit", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 209, Key = "armor_armoire_kendoBogu", Name = "Kendo Bogu", Slot = "armor", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 210, Key = "back_armoire_harpsichord", Name = "Harpsichord", Slot = "back", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 211, Key = "body_armoire_cozyScarf", Name = "Cozy Scarf", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 212, Key = "body_armoire_lifeguardWhistle", Name = "Lifeguard Whistle", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 213, Key = "body_armoire_clownsBowtie", Name = "Clowns Bowtie", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 2, CON = 2, INT = 2, PER = 2, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 214, Key = "body_armoire_karateYellowBelt", Name = "Karate Yellow Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 215, Key = "body_armoire_karateWhiteBelt", Name = "Karate White Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 3, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 216, Key = "body_armoire_karateRedBelt", Name = "Karate Red Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 217, Key = "body_armoire_karatePurpleBelt", Name = "Karate Purple Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 3, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 218, Key = "body_armoire_karateOrangeBelt", Name = "Karate Orange Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 3, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 219, Key = "body_armoire_karateGreenBelt", Name = "Karate Green Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 220, Key = "body_armoire_karateBrownBelt", Name = "Karate Brown Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 221, Key = "body_armoire_karateBlueBelt", Name = "Karate Blue Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 3, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 222, Key = "body_armoire_karateBlackBelt", Name = "Karate Black Belt", Slot = "body", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 3, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 223, Key = "eyewear_armoire_plagueDoctorMask", Name = "Plague Doctor Mask", Slot = "eyewear", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 224, Key = "eyewear_armoire_goofyGlasses", Name = "Goofy Glasses", Slot = "eyewear", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 225, Key = "eyewear_armoire_clownsNose", Name = "Clowns Nose", Slot = "eyewear", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 226, Key = "eyewear_armoire_tragedyMask", Name = "Tragedy Mask", Slot = "eyewear", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 227, Key = "eyewear_armoire_comedyMask", Name = "Comedy Mask", Slot = "eyewear", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 228, Key = "eyewear_armoire_jewelersEyeLoupe", Name = "Jewelers Eye Loupe", Slot = "eyewear", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 229, Key = "eyewear_armoire_roseColoredGlasses", Name = "Rose Colored Glasses", Slot = "eyewear", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 230, Key = "head_armoire_lunarCrown", Name = "Lunar Crown", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 231, Key = "head_armoire_redHairbow", Name = "Red Hairbow", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 232, Key = "head_armoire_violetFloppyHat", Name = "Violet Floppy Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 233, Key = "head_armoire_gladiatorHelm", Name = "Gladiator Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 234, Key = "head_armoire_rancherHat", Name = "Rancher Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 235, Key = "head_armoire_royalCrown", Name = "Royal Crown", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 236, Key = "head_armoire_blueHairbow", Name = "Blue Hairbow", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 237, Key = "head_armoire_goldenLaurels", Name = "Golden Laurels", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 238, Key = "head_armoire_hornedIronHelm", Name = "Horned Iron Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 239, Key = "head_armoire_yellowHairbow", Name = "Yellow Hairbow", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 240, Key = "head_armoire_redFloppyHat", Name = "Red Floppy Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 6, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 241, Key = "head_armoire_plagueDoctorHat", Name = "Plague Doctor Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 5, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 242, Key = "head_armoire_blackCat", Name = "Black Cat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 243, Key = "head_armoire_orangeCat", Name = "Orange Cat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 9, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 244, Key = "head_armoire_blueFloppyHat", Name = "Blue Floppy Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 245, Key = "head_armoire_shepherdHeaddress", Name = "Shepherd Headdress", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 246, Key = "head_armoire_crystalCrescentHat", Name = "Crystal Crescent Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 247, Key = "head_armoire_dragonTamerHelm", Name = "Dragon Tamer Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 15, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 248, Key = "head_armoire_barristerWig", Name = "Barrister Wig", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 249, Key = "head_armoire_jesterCap", Name = "Jester Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 250, Key = "head_armoire_minerHelmet", Name = "Miner Helmet", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 251, Key = "head_armoire_basicArcherCap", Name = "Basic Archer Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 252, Key = "head_armoire_graduateCap", Name = "Graduate Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 253, Key = "head_armoire_greenFloppyHat", Name = "Green Floppy Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 8, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 254, Key = "head_armoire_cannoneerBandanna", Name = "Cannoneer Bandanna", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 15, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 255, Key = "head_armoire_falconerCap", Name = "Falconer Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 256, Key = "head_armoire_vermilionArcherHelm", Name = "Vermilion Archer Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 257, Key = "head_armoire_ogreMask", Name = "Ogre Mask", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 258, Key = "head_armoire_ironBlueArcherHelm", Name = "Iron Blue Archer Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 259, Key = "head_armoire_woodElfHelm", Name = "Wood Elf Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 260, Key = "head_armoire_ramHeaddress", Name = "Ram Headdress", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 261, Key = "head_armoire_crownOfHearts", Name = "Crown Of Hearts", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 13, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 262, Key = "head_armoire_mushroomDruidCap", Name = "Mushroom Druid Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 263, Key = "head_armoire_merchantChaperon", Name = "Merchant Chaperon", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 264, Key = "head_armoire_vikingHelm", Name = "Viking Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 265, Key = "head_armoire_swanFeatherCrown", Name = "Swan Feather Crown", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 266, Key = "head_armoire_antiProcrastinationHelm", Name = "Anti Procrastination Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 267, Key = "head_armoire_candlestickMakerHat", Name = "Candlestick Maker Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 268, Key = "head_armoire_lamplightersTopHat", Name = "Lamplighters Top Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 14, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 269, Key = "head_armoire_coachDriversHat", Name = "Coach Drivers Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 270, Key = "head_armoire_crownOfDiamonds", Name = "Crown Of Diamonds", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 13, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 271, Key = "head_armoire_flutteryWig", Name = "Fluttery Wig", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 272, Key = "head_armoire_bigWig", Name = "Big Wig", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 273, Key = "head_armoire_paperBag", Name = "Paper Bag", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 274, Key = "head_armoire_birdsNest", Name = "Birds Nest", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 275, Key = "head_armoire_glassblowersHat", Name = "Glassblowers Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 276, Key = "head_armoire_piraticalPrincessHeaddress", Name = "Piratical Princess Headdress", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 277, Key = "head_armoire_jeweledArcherHelm", Name = "Jeweled Archer Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 15, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 278, Key = "head_armoire_veilOfSpades", Name = "Veil Of Spades", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 13, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 279, Key = "head_armoire_toqueBlanche", Name = "Toque Blanche", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 280, Key = "head_armoire_vernalHennin", Name = "Vernal Hennin", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 281, Key = "head_armoire_tricornHat", Name = "Tricorn Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 282, Key = "head_armoire_nephriteHelm", Name = "Nephrite Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 283, Key = "head_armoire_boaterHat", Name = "Boater Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 284, Key = "head_armoire_astronomersHat", Name = "Astronomers Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 285, Key = "head_armoire_deerstalkerCap", Name = "Deerstalker Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 14, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 286, Key = "head_armoire_shadowMastersHood", Name = "Shadow Masters Hood", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 287, Key = "head_armoire_alchemistsHat", Name = "Alchemists Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 288, Key = "head_armoire_earflapHat", Name = "Earflap Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 289, Key = "head_armoire_frostedHelm", Name = "Frosted Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 13, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 290, Key = "head_armoire_matchMakersBeret", Name = "Match Makers Beret", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 291, Key = "head_armoire_baseballCap", Name = "Baseball Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 292, Key = "head_armoire_fiddlersCap", Name = "Fiddlers Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 293, Key = "head_armoire_heroicHerbalistCrispinette", Name = "Heroic Herbalist Crispinette", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 294, Key = "head_armoire_guardiansBonnet", Name = "Guardians Bonnet", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 295, Key = "head_armoire_hornsOfAutumn", Name = "Horns Of Autumn", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 12, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 296, Key = "head_armoire_capOfClubs", Name = "Cap Of Clubs", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 297, Key = "head_armoire_nightcap", Name = "Nightcap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 298, Key = "head_armoire_blueMoonHelm", Name = "Blue Moon Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 299, Key = "head_armoire_pinkFloppyHat", Name = "Pink Floppy Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 300, Key = "head_armoire_jadeHelm", Name = "Jade Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 301, Key = "head_armoire_clownsWig", Name = "Clowns Wig", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 302, Key = "head_armoire_medievalLaundryCap", Name = "Medieval Laundry Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 303, Key = "head_armoire_medievalLaundryHat", Name = "Medieval Laundry Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 304, Key = "head_armoire_rubberDucky", Name = "Rubber Ducky", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 305, Key = "head_armoire_glengarry", Name = "Glengarry", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 306, Key = "head_armoire_heraldsCap", Name = "Heralds Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 307, Key = "head_armoire_blackFloppyHat", Name = "Black Floppy Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 308, Key = "head_armoire_regalCrown", Name = "Regal Crown", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 309, Key = "head_armoire_shootingStarCrown", Name = "Shooting Star Crown", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 310, Key = "head_armoire_gardenersSunHat", Name = "Gardeners Sun Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 311, Key = "head_armoire_strawRainHat", Name = "Straw Rain Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 312, Key = "head_armoire_fancyPirateHat", Name = "Fancy Pirate Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 313, Key = "head_armoire_teaHat", Name = "Tea Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 314, Key = "head_armoire_beaniePropellerHat", Name = "Beanie Propeller Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 315, Key = "head_armoire_paintersBeret", Name = "Painters Beret", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 316, Key = "head_armoire_admiralsBicorne", Name = "Admirals Bicorne", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 317, Key = "head_armoire_blackSpookySorceryHat", Name = "Black Spooky Sorcery Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 3, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 318, Key = "head_armoire_purpleSpookySorceryHat", Name = "Purple Spooky Sorcery Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 3, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 319, Key = "head_armoire_greenTrapperHat", Name = "Green Trapper Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 320, Key = "head_armoire_whiteFloppyHat", Name = "White Floppy Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 321, Key = "head_armoire_hattersTopHat", Name = "Hatters Top Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 322, Key = "head_armoire_pottersBandana", Name = "Potters Bandana", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 323, Key = "head_armoire_corsairsBandana", Name = "Corsairs Bandana", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 324, Key = "head_armoire_dragonKnightsHelm", Name = "Dragon Knights Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 325, Key = "head_armoire_funnyFoolCap", Name = "Funny Fool Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 326, Key = "head_armoire_stormKnightHelm", Name = "Storm Knight Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 11, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 327, Key = "head_armoire_festiveHelperHat", Name = "Festive Helper Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 328, Key = "head_armoire_snowyTrapperHat", Name = "Snowy Trapper Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 329, Key = "head_armoire_fancyFloralHat", Name = "Fancy Floral Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 14, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 330, Key = "head_armoire_sillyOrangeTophat", Name = "Silly Orange Tophat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 331, Key = "head_armoire_sillierBlueTophat", Name = "Sillier Blue Tophat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 332, Key = "head_armoire_gildedKnightsHelm", Name = "Gilded Knights Helm", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 11, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 333, Key = "head_armoire_beekeepersHat", Name = "Beekeepers Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 334, Key = "head_armoire_flyFishingHat", Name = "Fly Fishing Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 335, Key = "head_armoire_redNewsieHat", Name = "Red Newsie Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 336, Key = "head_armoire_floppyOrangeHat", Name = "Floppy Orange Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 4, INT = 4, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 337, Key = "head_armoire_blackHairbow", Name = "Black Hairbow", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 338, Key = "head_armoire_blacksmithsGoggles", Name = "Blacksmiths Goggles", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 11, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 339, Key = "head_armoire_loneCowpokeHat", Name = "Lone Cowpoke Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 340, Key = "head_armoire_floppyYellowHat", Name = "Floppy Yellow Hat", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 341, Key = "head_armoire_verdantArmingCap", Name = "Verdant Arming Cap", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 342, Key = "head_armoire_kendoMen", Name = "Kendo Men", Slot = "head", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 343, Key = "shield_armoire_gladiatorShield", Name = "Gladiator Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 344, Key = "shield_armoire_midnightShield", Name = "Midnight Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 2, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 345, Key = "shield_armoire_royalCane", Name = "Royal Cane", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 346, Key = "shield_armoire_dragonTamerShield", Name = "Dragon Tamer Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 347, Key = "shield_armoire_mysticLamp", Name = "Mystic Lamp", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 348, Key = "shield_armoire_floralBouquet", Name = "Floral Bouquet", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 3, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 349, Key = "shield_armoire_sandyBucket", Name = "Sandy Bucket", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 350, Key = "shield_armoire_perchingFalcon", Name = "Perching Falcon", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 16, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 351, Key = "shield_armoire_ramHornShield", Name = "Ram Horn Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 352, Key = "shield_armoire_redRose", Name = "Red Rose", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 353, Key = "shield_armoire_mushroomDruidShield", Name = "Mushroom Druid Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 354, Key = "shield_armoire_festivalParasol", Name = "Festival Parasol", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 355, Key = "shield_armoire_vikingShield", Name = "Viking Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 356, Key = "shield_armoire_swanFeatherFan", Name = "Swan Feather Fan", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 357, Key = "shield_armoire_goldenBaton", Name = "Golden Baton", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 0, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 358, Key = "shield_armoire_antiProcrastinationShield", Name = "Anti Procrastination Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 359, Key = "shield_armoire_horseshoe", Name = "Horseshoe", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 360, Key = "shield_armoire_handmadeCandlestick", Name = "Handmade Candlestick", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 12, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 361, Key = "shield_armoire_weaversShuttle", Name = "Weavers Shuttle", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 362, Key = "shield_armoire_shieldOfDiamonds", Name = "Shield Of Diamonds", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 363, Key = "shield_armoire_flutteryFan", Name = "Fluttery Fan", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 364, Key = "shield_armoire_fancyShoe", Name = "Fancy Shoe", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 365, Key = "shield_armoire_fancyBlownGlassVase", Name = "Fancy Blown Glass Vase", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 366, Key = "shield_armoire_piraticalSkullShield", Name = "Piratical Skull Shield", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 4, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 367, Key = "shield_armoire_unfinishedTome", Name = "Unfinished Tome", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 368, Key = "shield_armoire_softBluePillow", Name = "Soft Blue Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 369, Key = "shield_armoire_softGreenPillow", Name = "Soft Green Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 370, Key = "shield_armoire_softRedPillow", Name = "Soft Red Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 371, Key = "shield_armoire_mightyQuill", Name = "Mighty Quill", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 372, Key = "shield_armoire_mightyPizza", Name = "Mighty Pizza", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 373, Key = "shield_armoire_trustyUmbrella", Name = "Trusty Umbrella", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 374, Key = "shield_armoire_polishedPocketwatch", Name = "Polished Pocketwatch", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 375, Key = "shield_armoire_masteredShadow", Name = "Mastered Shadow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 376, Key = "shield_armoire_alchemistsScale", Name = "Alchemists Scale", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 377, Key = "shield_armoire_birthdayBanner", Name = "Birthday Banner", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 378, Key = "shield_armoire_perfectMatch", Name = "Perfect Match", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 379, Key = "shield_armoire_baseballGlove", Name = "Baseball Glove", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 9, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 380, Key = "shield_armoire_hobbyHorse", Name = "Hobby Horse", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 4, INT = 0, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 381, Key = "shield_armoire_fiddle", Name = "Fiddle", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 382, Key = "shield_armoire_lifeBuoy", Name = "Life Buoy", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 383, Key = "shield_armoire_piratesCompanion", Name = "Pirates Companion", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 384, Key = "shield_armoire_mortarAndPestle", Name = "Mortar And Pestle", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 385, Key = "shield_armoire_darkAutumnFlame", Name = "Dark Autumn Flame", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 12, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 386, Key = "shield_armoire_blueMoonSai", Name = "Blue Moon Sai", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 387, Key = "shield_armoire_softPinkPillow", Name = "Soft Pink Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 388, Key = "shield_armoire_clownsBalloons", Name = "Clowns Balloons", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 389, Key = "shield_armoire_strawberryFood", Name = "Strawberry Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 390, Key = "shield_armoire_rottenMeatFood", Name = "Rotten Meat Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 391, Key = "shield_armoire_potatoFood", Name = "Potato Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 392, Key = "shield_armoire_pinkCottonCandyFood", Name = "Pink Cotton Candy Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 393, Key = "shield_armoire_meatFood", Name = "Meat Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 394, Key = "shield_armoire_honeyFood", Name = "Honey Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 395, Key = "shield_armoire_fishFood", Name = "Fish Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 396, Key = "shield_armoire_chocolateFood", Name = "Chocolate Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 397, Key = "shield_armoire_blueCottonCandyFood", Name = "Blue Cotton Candy Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 398, Key = "shield_armoire_milkFood", Name = "Milk Food", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 399, Key = "shield_armoire_medievalLaundry", Name = "Medieval Laundry", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 400, Key = "shield_armoire_bouncyBubbles", Name = "Bouncy Bubbles", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 401, Key = "shield_armoire_bagpipes", Name = "Bagpipes", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 402, Key = "shield_armoire_heraldsMessageScroll", Name = "Heralds Message Scroll", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 403, Key = "shield_armoire_softBlackPillow", Name = "Soft Black Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 5, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 404, Key = "shield_armoire_softVioletPillow", Name = "Soft Violet Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 405, Key = "shield_armoire_gardenersSpade", Name = "Gardeners Spade", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 406, Key = "shield_armoire_spanishGuitar", Name = "Spanish Guitar", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 407, Key = "shield_armoire_snareDrum", Name = "Snare Drum", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 408, Key = "shield_armoire_treasureMap", Name = "Treasure Map", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 0, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 409, Key = "shield_armoire_dustpan", Name = "Dustpan", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 4, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 410, Key = "shield_armoire_bubblingCauldron", Name = "Bubbling Cauldron", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 411, Key = "shield_armoire_jewelersPliers", Name = "Jewelers Pliers", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 412, Key = "shield_armoire_teaKettle", Name = "Tea Kettle", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 413, Key = "shield_armoire_basketball", Name = "Basketball", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 414, Key = "shield_armoire_paintersPalette", Name = "Painters Palette", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 415, Key = "shield_armoire_bucket", Name = "Bucket", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 0, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 416, Key = "shield_armoire_saucepan", Name = "Saucepan", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 417, Key = "shield_armoire_trustyPencil", Name = "Trusty Pencil", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 418, Key = "shield_armoire_softWhitePillow", Name = "Soft White Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 6, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 419, Key = "shield_armoire_hattersPocketWatch", Name = "Hatters Pocket Watch", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 420, Key = "shield_armoire_happyThoughts", Name = "Happy Thoughts", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 4, INT = 4, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 421, Key = "shield_armoire_thrownVessel", Name = "Thrown Vessel", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 422, Key = "shield_armoire_buoyantBeachBall", Name = "Buoyant Beach Ball", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 12, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 423, Key = "shield_armoire_safetyFlashlight", Name = "Safety Flashlight", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 424, Key = "shield_armoire_fancyFloralFan", Name = "Fancy Floral Fan", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 14, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 425, Key = "shield_armoire_springPetalUchiwa", Name = "Spring Petal Uchiwa", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 426, Key = "shield_armoire_beekeepersHive", Name = "Beekeepers Hive", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 12, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 427, Key = "shield_armoire_flyFishingRod", Name = "Fly Fishing Rod", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 428, Key = "shield_armoire_softOrangePillow", Name = "Soft Orange Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 429, Key = "shield_armoire_doubleBass", Name = "Double Bass", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 430, Key = "shield_armoire_prettyPinkGiftBox", Name = "Pretty Pink Gift Box", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 2, CON = 2, INT = 2, PER = 2, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 431, Key = "shield_armoire_softYellowPillow", Name = "Soft Yellow Pillow", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 432, Key = "shield_armoire_verdantBanner", Name = "Verdant Banner", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 433, Key = "shield_armoire_gardenHose", Name = "Garden Hose", Slot = "shield", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 434, Key = "headAccessory_armoire_comicalArrow", Name = "Comical Arrow", Slot = "headAccessory", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 435, Key = "headAccessory_armoire_gogglesOfBookbinding", Name = "Goggles Of Bookbinding", Slot = "headAccessory", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 436, Key = "weapon_armoire_basicCrossbow", Name = "Basic Crossbow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 437, Key = "weapon_armoire_lunarSceptre", Name = "Lunar Sceptre", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 7, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 438, Key = "weapon_armoire_rancherLasso", Name = "Rancher Lasso", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 5, PER = 5, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 439, Key = "weapon_armoire_mythmakerSword", Name = "Mythmaker Sword", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 440, Key = "weapon_armoire_ironCrook", Name = "Iron Crook", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 441, Key = "weapon_armoire_goldWingStaff", Name = "Gold Wing Staff", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 4, INT = 4, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 442, Key = "weapon_armoire_batWand", Name = "Bat Wand", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 2, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 443, Key = "weapon_armoire_shepherdsCrook", Name = "Shepherds Crook", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 444, Key = "weapon_armoire_crystalCrescentStaff", Name = "Crystal Crescent Staff", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 445, Key = "weapon_armoire_blueLongbow", Name = "Blue Longbow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 8, INT = 0, PER = 9, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 446, Key = "weapon_armoire_glowingSpear", Name = "Glowing Spear", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 447, Key = "weapon_armoire_barristerGavel", Name = "Barrister Gavel", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 448, Key = "weapon_armoire_jesterBaton", Name = "Jester Baton", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 449, Key = "weapon_armoire_miningPickax", Name = "Mining Pickax", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 15, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 450, Key = "weapon_armoire_basicLongbow", Name = "Basic Longbow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 0, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 451, Key = "weapon_armoire_habiticanDiploma", Name = "Habitican Diploma", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 11, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 452, Key = "weapon_armoire_sandySpade", Name = "Sandy Spade", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 453, Key = "weapon_armoire_cannon", Name = "Cannon", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 454, Key = "weapon_armoire_vermilionArcherBow", Name = "Vermilion Archer Bow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 0, INT = 0, PER = 0, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 455, Key = "weapon_armoire_ogreClub", Name = "Ogre Club", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 456, Key = "weapon_armoire_woodElfStaff", Name = "Wood Elf Staff", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 457, Key = "weapon_armoire_wandOfHearts", Name = "Wand Of Hearts", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 13, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 458, Key = "weapon_armoire_forestFungusStaff", Name = "Forest Fungus Staff", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 459, Key = "weapon_armoire_festivalFirecracker", Name = "Festival Firecracker", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 460, Key = "weapon_armoire_merchantsDisplayTray", Name = "Merchants Display Tray", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 461, Key = "weapon_armoire_battleAxe", Name = "Battle Axe", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 462, Key = "weapon_armoire_hoofClippers", Name = "Hoof Clippers", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 463, Key = "weapon_armoire_weaversComb", Name = "Weavers Comb", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 9, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 464, Key = "weapon_armoire_lamplighter", Name = "Lamplighter", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 465, Key = "weapon_armoire_coachDriversWhip", Name = "Coach Drivers Whip", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 466, Key = "weapon_armoire_scepterOfDiamonds", Name = "Scepter Of Diamonds", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 13, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 467, Key = "weapon_armoire_flutteryArmy", Name = "Fluttery Army", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 5, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 468, Key = "weapon_armoire_cobblersHammer", Name = "Cobblers Hammer", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 7, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 469, Key = "weapon_armoire_glassblowersBlowpipe", Name = "Glassblowers Blowpipe", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 470, Key = "weapon_armoire_poisonedGoblet", Name = "Poisoned Goblet", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 7, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 471, Key = "weapon_armoire_jeweledArcherBow", Name = "Jeweled Archer Bow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 15, PER = 0, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 472, Key = "weapon_armoire_needleOfBookbinding", Name = "Needle Of Bookbinding", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 473, Key = "weapon_armoire_spearOfSpades", Name = "Spear Of Spades", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 13, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 474, Key = "weapon_armoire_arcaneScroll", Name = "Arcane Scroll", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 475, Key = "weapon_armoire_chefsSpoon", Name = "Chefs Spoon", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 476, Key = "weapon_armoire_vernalTaper", Name = "Vernal Taper", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 477, Key = "weapon_armoire_jugglingBalls", Name = "Juggling Balls", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 478, Key = "weapon_armoire_slingshot", Name = "Slingshot", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 479, Key = "weapon_armoire_nephriteBow", Name = "Nephrite Bow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 7, PER = 0, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 480, Key = "weapon_armoire_bambooCane", Name = "Bamboo Cane", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 6, PER = 6, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 481, Key = "weapon_armoire_astronomersTelescope", Name = "Astronomers Telescope", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 10, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 482, Key = "weapon_armoire_magnifyingGlass", Name = "Magnifying Glass", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 483, Key = "weapon_armoire_floridFan", Name = "Florid Fan", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 484, Key = "weapon_armoire_resplendentRapier", Name = "Resplendent Rapier", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 9, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 485, Key = "weapon_armoire_shadowMastersMace", Name = "Shadow Masters Mace", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 486, Key = "weapon_armoire_alchemistsDistiller", Name = "Alchemists Distiller", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 5, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 487, Key = "weapon_armoire_happyBanner", Name = "Happy Banner", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 488, Key = "weapon_armoire_livelyMatch", Name = "Lively Match", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 489, Key = "weapon_armoire_baseballBat", Name = "Baseball Bat", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 9, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 490, Key = "weapon_armoire_paperCutter", Name = "Paper Cutter", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 9, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 491, Key = "weapon_armoire_fiddlersBow", Name = "Fiddlers Bow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 492, Key = "weapon_armoire_beachFlag", Name = "Beach Flag", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 493, Key = "weapon_armoire_handyHook", Name = "Handy Hook", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 494, Key = "weapon_armoire_guardiansCrook", Name = "Guardians Crook", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 495, Key = "weapon_armoire_enchantersStaff", Name = "Enchanters Staff", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 496, Key = "weapon_armoire_clubOfClubs", Name = "Club Of Clubs", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 497, Key = "weapon_armoire_eveningTea", Name = "Evening Tea", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 498, Key = "weapon_armoire_blueMoonSai", Name = "Blue Moon Sai", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 8, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 499, Key = "weapon_armoire_jadeGlaive", Name = "Jade Glaive", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 500, Key = "weapon_armoire_medievalWashboard", Name = "Medieval Washboard", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 501, Key = "weapon_armoire_buoyantBubbles", Name = "Buoyant Bubbles", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 5, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 502, Key = "weapon_armoire_heraldsBuisine", Name = "Heralds Buisine", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 503, Key = "weapon_armoire_skullLantern", Name = "Skull Lantern", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 504, Key = "weapon_armoire_potionBase", Name = "Potion Base", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 505, Key = "weapon_armoire_potionBlue", Name = "Potion Blue", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 506, Key = "weapon_armoire_potionDesert", Name = "Potion Desert", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 507, Key = "weapon_armoire_potionGolden", Name = "Potion Golden", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 508, Key = "weapon_armoire_potionPink", Name = "Potion Pink", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 509, Key = "weapon_armoire_potionRed", Name = "Potion Red", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 6, CON = 6, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 510, Key = "weapon_armoire_potionShade", Name = "Potion Shade", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 9, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 511, Key = "weapon_armoire_potionSkeleton", Name = "Potion Skeleton", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 9, CON = 0, INT = 3, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 512, Key = "weapon_armoire_potionWhite", Name = "Potion White", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 5, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 513, Key = "weapon_armoire_potionZombie", Name = "Potion Zombie", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 4, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 514, Key = "weapon_armoire_regalSceptre", Name = "Regal Sceptre", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 7, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 515, Key = "weapon_armoire_shootingStarSpell", Name = "Shooting Star Spell", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 5, PER = 0, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 516, Key = "weapon_armoire_pinkLongbow", Name = "Pink Longbow", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 0, PER = 6, TwoHanded = true, IsArmoire = true },
                new GearItem { Id = 517, Key = "weapon_armoire_gardenersWateringCan", Name = "Gardeners Watering Can", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 518, Key = "weapon_armoire_huntingHorn", Name = "Hunting Horn", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 519, Key = "weapon_armoire_blueKite", Name = "Blue Kite", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 520, Key = "weapon_armoire_greenKite", Name = "Green Kite", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 521, Key = "weapon_armoire_orangeKite", Name = "Orange Kite", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 522, Key = "weapon_armoire_pinkKite", Name = "Pink Kite", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 523, Key = "weapon_armoire_yellowKite", Name = "Yellow Kite", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 524, Key = "weapon_armoire_pushBroom", Name = "Push Broom", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 0, INT = 4, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 525, Key = "weapon_armoire_featherDuster", Name = "Feather Duster", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 4, INT = 0, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 526, Key = "weapon_armoire_magicSpatula", Name = "Magic Spatula", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 527, Key = "weapon_armoire_finelyCutGem", Name = "Finely Cut Gem", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 10, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 528, Key = "weapon_armoire_paintbrush", Name = "Paintbrush", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 8, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 529, Key = "weapon_armoire_mop", Name = "Mop", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 4, INT = 0, PER = 4, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 530, Key = "weapon_armoire_cleaningCloth", Name = "Cleaning Cloth", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 4, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 531, Key = "weapon_armoire_ridingBroom", Name = "Riding Broom", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 5, CON = 0, INT = 3, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 532, Key = "weapon_armoire_rollingPin", Name = "Rolling Pin", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 533, Key = "weapon_armoire_scholarlyTextbooks", Name = "Scholarly Textbooks", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 534, Key = "weapon_armoire_hattersShears", Name = "Hatters Shears", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 10, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 535, Key = "weapon_armoire_optimistsClover", Name = "Optimists Clover", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 4, CON = 4, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 536, Key = "weapon_armoire_pottersWheel", Name = "Potters Wheel", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 8, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 537, Key = "weapon_armoire_shadyBeachUmbrella", Name = "Shady Beach Umbrella", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 0, PER = 12, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 538, Key = "weapon_armoire_corsairsBlade", Name = "Corsairs Blade", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 539, Key = "weapon_armoire_dragonKnightsLance", Name = "Dragon Knights Lance", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 540, Key = "weapon_armoire_funnyFoolBaton", Name = "Funny Fool Baton", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 15, CON = 15, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 541, Key = "weapon_armoire_spookyCandyBucket", Name = "Spooky Candy Bucket", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 10, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 542, Key = "weapon_armoire_stormKnightAxe", Name = "Storm Knight Axe", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 11, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 543, Key = "weapon_armoire_gildedKnightsSpear", Name = "Gilded Knights Spear", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 11, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 544, Key = "weapon_armoire_beekeepersSmoker", Name = "Beekeepers Smoker", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 0, INT = 12, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 545, Key = "weapon_armoire_blacksmithsHammer", Name = "Blacksmiths Hammer", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 11, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 546, Key = "weapon_armoire_bambooFlute", Name = "Bamboo Flute", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 6, INT = 6, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 547, Key = "weapon_armoire_prettyPinkParasol", Name = "Pretty Pink Parasol", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 2, CON = 2, INT = 2, PER = 2, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 548, Key = "weapon_armoire_brightRainbowKite", Name = "Bright Rainbow Kite", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 549, Key = "weapon_armoire_pastelRainbowKite", Name = "Pastel Rainbow Kite", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 3, CON = 3, INT = 3, PER = 3, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 550, Key = "weapon_armoire_kendoShinai", Name = "Kendo Shinai", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 7, CON = 0, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true },
                new GearItem { Id = 551, Key = "weapon_armoire_gardenRake", Name = "Garden Rake", Slot = "weapon", GearClass = "armoire", Tier = 0, GoldCost = 0, STR = 0, CON = 8, INT = 0, PER = 0, TwoHanded = false, IsArmoire = true }
            );

            // ===== GAME ITEM SEED DATA =====
            // Food Target = preferred potion color for feeding (Habitica food.js target field)
            // IsDroppable = false for quest/premium items that cannot drop from task completions
            modelBuilder.Entity<GameItem>().HasData(

                // ── DROP FOOD (10 items, Ids 1-6 + 22-25) ── IsDroppable = true
                new GameItem { Id = 1,  Key = "food_Meat",              Name = "Meat",               Icon = "🥩", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Base",            IsDroppable = true  },
                new GameItem { Id = 2,  Key = "food_Strawberry",        Name = "Strawberry",         Icon = "🍓", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Red",             IsDroppable = true  },
                new GameItem { Id = 3,  Key = "food_Potato",            Name = "Potato",             Icon = "🥔", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Desert",          IsDroppable = true  },
                new GameItem { Id = 4,  Key = "food_Chocolate",         Name = "Chocolate",          Icon = "🍫", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Shade",           IsDroppable = true  },
                new GameItem { Id = 5,  Key = "food_Fish",              Name = "Fish",               Icon = "🐟", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Skeleton",        IsDroppable = true  },
                new GameItem { Id = 6,  Key = "food_Honey",             Name = "Honey",              Icon = "🍯", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Golden",          IsDroppable = true  },
                new GameItem { Id = 22, Key = "food_Milk",              Name = "Milk",               Icon = "🥛", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "White",           IsDroppable = true  },
                new GameItem { Id = 23, Key = "food_RottenMeat",        Name = "Rotten Meat",        Icon = "🍖", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Zombie",          IsDroppable = true  },
                new GameItem { Id = 24, Key = "food_CottonCandyPink",   Name = "Cotton Candy Pink",  Icon = "🍬", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyPink", IsDroppable = true  },
                new GameItem { Id = 25, Key = "food_CottonCandyBlue",   Name = "Cotton Candy Blue",  Icon = "🍬", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyBlue", IsDroppable = true  },

                // ── SPECIAL FOOD (Id 92) ── IsDroppable = false
                new GameItem { Id = 92, Key = "food_Saddle",            Name = "Saddle",             Icon = "🪑", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 0,                             IsDroppable = false },

                // ── CAKE FOOD (Ids 93-102) ── IsDroppable = false
                new GameItem { Id = 93,  Key = "food_Cake_Base",           Name = "Cake (Base)",              Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Base",            IsDroppable = false },
                new GameItem { Id = 94,  Key = "food_Cake_CottonCandyBlue",Name = "Cake (Cotton Candy Blue)", Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyBlue", IsDroppable = false },
                new GameItem { Id = 95,  Key = "food_Cake_CottonCandyPink",Name = "Cake (Cotton Candy Pink)", Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyPink", IsDroppable = false },
                new GameItem { Id = 96,  Key = "food_Cake_Desert",         Name = "Cake (Desert)",            Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Desert",          IsDroppable = false },
                new GameItem { Id = 97,  Key = "food_Cake_Golden",         Name = "Cake (Golden)",            Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Golden",          IsDroppable = false },
                new GameItem { Id = 98,  Key = "food_Cake_Red",            Name = "Cake (Red)",               Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Red",             IsDroppable = false },
                new GameItem { Id = 99,  Key = "food_Cake_Shade",          Name = "Cake (Shade)",             Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Shade",           IsDroppable = false },
                new GameItem { Id = 100, Key = "food_Cake_Skeleton",       Name = "Cake (Skeleton)",          Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Skeleton",        IsDroppable = false },
                new GameItem { Id = 101, Key = "food_Cake_White",          Name = "Cake (White)",             Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "White",           IsDroppable = false },
                new GameItem { Id = 102, Key = "food_Cake_Zombie",         Name = "Cake (Zombie)",            Icon = "🎂", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Zombie",          IsDroppable = false },

                // ── CANDY FOOD (Ids 103-112) ── IsDroppable = false
                new GameItem { Id = 103, Key = "food_Candy_Base",           Name = "Candy (Base)",              Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Base",            IsDroppable = false },
                new GameItem { Id = 104, Key = "food_Candy_CottonCandyBlue",Name = "Candy (Cotton Candy Blue)", Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyBlue", IsDroppable = false },
                new GameItem { Id = 105, Key = "food_Candy_CottonCandyPink",Name = "Candy (Cotton Candy Pink)", Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyPink", IsDroppable = false },
                new GameItem { Id = 106, Key = "food_Candy_Desert",         Name = "Candy (Desert)",            Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Desert",          IsDroppable = false },
                new GameItem { Id = 107, Key = "food_Candy_Golden",         Name = "Candy (Golden)",            Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Golden",          IsDroppable = false },
                new GameItem { Id = 108, Key = "food_Candy_Red",            Name = "Candy (Red)",               Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Red",             IsDroppable = false },
                new GameItem { Id = 109, Key = "food_Candy_Shade",          Name = "Candy (Shade)",             Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Shade",           IsDroppable = false },
                new GameItem { Id = 110, Key = "food_Candy_Skeleton",       Name = "Candy (Skeleton)",          Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Skeleton",        IsDroppable = false },
                new GameItem { Id = 111, Key = "food_Candy_White",          Name = "Candy (White)",             Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "White",           IsDroppable = false },
                new GameItem { Id = 112, Key = "food_Candy_Zombie",         Name = "Candy (Zombie)",            Icon = "🍭", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Zombie",          IsDroppable = false },

                // ── PIE FOOD (Ids 113-122) ── IsDroppable = false
                new GameItem { Id = 113, Key = "food_Pie_Base",           Name = "Pie (Base)",              Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Base",            IsDroppable = false },
                new GameItem { Id = 114, Key = "food_Pie_CottonCandyBlue",Name = "Pie (Cotton Candy Blue)", Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyBlue", IsDroppable = false },
                new GameItem { Id = 115, Key = "food_Pie_CottonCandyPink",Name = "Pie (Cotton Candy Pink)", Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "CottonCandyPink", IsDroppable = false },
                new GameItem { Id = 116, Key = "food_Pie_Desert",         Name = "Pie (Desert)",            Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Desert",          IsDroppable = false },
                new GameItem { Id = 117, Key = "food_Pie_Golden",         Name = "Pie (Golden)",            Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Golden",          IsDroppable = false },
                new GameItem { Id = 118, Key = "food_Pie_Red",            Name = "Pie (Red)",               Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Red",             IsDroppable = false },
                new GameItem { Id = 119, Key = "food_Pie_Shade",          Name = "Pie (Shade)",             Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Shade",           IsDroppable = false },
                new GameItem { Id = 120, Key = "food_Pie_Skeleton",       Name = "Pie (Skeleton)",          Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Skeleton",        IsDroppable = false },
                new GameItem { Id = 121, Key = "food_Pie_White",          Name = "Pie (White)",             Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "White",           IsDroppable = false },
                new GameItem { Id = 122, Key = "food_Pie_Zombie",         Name = "Pie (Zombie)",            Icon = "🥧", Type = ItemType.Food, Rarity = ItemRarity.Common, GoldValue = 1, Target = "Zombie",          IsDroppable = false },

                // ── DROP EGGS (9 items, Ids 7-10 + 26-30) ── IsDroppable = true
                new GameItem { Id = 7,  Key = "egg_Wolf",      Name = "Wolf Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 8,  Key = "egg_Bear",      Name = "Bear Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 9,  Key = "egg_Cactus",    Name = "Cactus Egg",     Icon = "🌵", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 10, Key = "egg_Dragon",    Name = "Dragon Egg",     Icon = "🐉", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 26, Key = "egg_TigerCub",  Name = "Tiger Cub Egg",  Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 27, Key = "egg_PandaCub",  Name = "Panda Cub Egg",  Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 28, Key = "egg_LionCub",   Name = "Lion Cub Egg",   Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 29, Key = "egg_Fox",       Name = "Fox Egg",        Icon = "🦊", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },
                new GameItem { Id = 30, Key = "egg_FlyingPig", Name = "Flying Pig Egg", Icon = "🐷", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = true  },

                // ── QUEST EGGS (Ids 11 + 31-91) ── IsDroppable = false
                new GameItem { Id = 11,  Key = "egg_Axolotl",     Name = "Axolotl Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 31,  Key = "egg_Alligator",   Name = "Alligator Egg",   Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 32,  Key = "egg_Alpaca",      Name = "Alpaca Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 33,  Key = "egg_Armadillo",   Name = "Armadillo Egg",   Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 34,  Key = "egg_Badger",      Name = "Badger Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 35,  Key = "egg_Beetle",      Name = "Beetle Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 36,  Key = "egg_Bunny",       Name = "Bunny Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 37,  Key = "egg_Butterfly",   Name = "Butterfly Egg",   Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 38,  Key = "egg_Cat",         Name = "Cat Egg",         Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 39,  Key = "egg_Chameleon",   Name = "Chameleon Egg",   Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 40,  Key = "egg_Cheetah",     Name = "Cheetah Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 41,  Key = "egg_Cow",         Name = "Cow Egg",         Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 42,  Key = "egg_Crab",        Name = "Crab Egg",        Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 43,  Key = "egg_Cuttlefish",  Name = "Cuttlefish Egg",  Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 44,  Key = "egg_Deer",        Name = "Deer Egg",        Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 45,  Key = "egg_Dog",         Name = "Dog Egg",         Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 46,  Key = "egg_Dolphin",     Name = "Dolphin Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 47,  Key = "egg_Egg",         Name = "Egg Egg",         Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 48,  Key = "egg_Falcon",      Name = "Falcon Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 49,  Key = "egg_Ferret",      Name = "Ferret Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 50,  Key = "egg_Frog",        Name = "Frog Egg",        Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 51,  Key = "egg_Giraffe",     Name = "Giraffe Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 52,  Key = "egg_Gryphon",     Name = "Gryphon Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 53,  Key = "egg_GuineaPig",   Name = "Guinea Pig Egg",  Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 54,  Key = "egg_Hedgehog",    Name = "Hedgehog Egg",    Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 55,  Key = "egg_Hippo",       Name = "Hippo Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 56,  Key = "egg_Horse",       Name = "Horse Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 57,  Key = "egg_Kangaroo",    Name = "Kangaroo Egg",    Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 58,  Key = "egg_Monkey",      Name = "Monkey Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 59,  Key = "egg_Nudibranch",  Name = "Nudibranch Egg",  Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 60,  Key = "egg_Octopus",     Name = "Octopus Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 61,  Key = "egg_Otter",       Name = "Otter Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 62,  Key = "egg_Owl",         Name = "Owl Egg",         Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 63,  Key = "egg_Parrot",      Name = "Parrot Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 64,  Key = "egg_Peacock",     Name = "Peacock Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 65,  Key = "egg_Penguin",     Name = "Penguin Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 66,  Key = "egg_Platypus",    Name = "Platypus Egg",    Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 67,  Key = "egg_PolarBear",   Name = "Polar Bear Egg",  Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 68,  Key = "egg_Pterodactyl", Name = "Pterodactyl Egg", Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 69,  Key = "egg_Raccoon",     Name = "Raccoon Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 70,  Key = "egg_Rat",         Name = "Rat Egg",         Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 71,  Key = "egg_Robot",       Name = "Robot Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 72,  Key = "egg_Rock",        Name = "Rock Egg",        Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 73,  Key = "egg_Rooster",     Name = "Rooster Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 74,  Key = "egg_Sabretooth",  Name = "Sabretooth Egg",  Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 75,  Key = "egg_Seahorse",    Name = "Seahorse Egg",    Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 76,  Key = "egg_SeaSerpent",  Name = "Sea Serpent Egg", Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 77,  Key = "egg_Sheep",       Name = "Sheep Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 78,  Key = "egg_Slime",       Name = "Slime Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 79,  Key = "egg_Sloth",       Name = "Sloth Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 80,  Key = "egg_Snail",       Name = "Snail Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 81,  Key = "egg_Snake",       Name = "Snake Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 82,  Key = "egg_Spider",      Name = "Spider Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 83,  Key = "egg_Squirrel",    Name = "Squirrel Egg",    Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 84,  Key = "egg_Treeling",    Name = "Treeling Egg",    Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 85,  Key = "egg_TRex",        Name = "T-Rex Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 86,  Key = "egg_Triceratops", Name = "Triceratops Egg", Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 87,  Key = "egg_Turtle",      Name = "Turtle Egg",      Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 88,  Key = "egg_Unicorn",     Name = "Unicorn Egg",     Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 89,  Key = "egg_Velociraptor",Name = "Velociraptor Egg",Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 90,  Key = "egg_Whale",       Name = "Whale Egg",       Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },
                new GameItem { Id = 91,  Key = "egg_Yarn",        Name = "Yarn Egg",        Icon = "🥚", Type = ItemType.Egg, Rarity = ItemRarity.Common, GoldValue = 3, IsDroppable = false },

                // ── DROP POTIONS (10, Ids 12-21) ── IsDroppable = true
                new GameItem { Id = 12, Key = "potion_Base",            Name = "Base Potion",              Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Common,   GoldValue = 2,  IsDroppable = true  },
                new GameItem { Id = 13, Key = "potion_White",           Name = "White Potion",             Icon = "🤍", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Common,   GoldValue = 2,  IsDroppable = true  },
                new GameItem { Id = 14, Key = "potion_Desert",          Name = "Desert Potion",            Icon = "🏜️", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Common,   GoldValue = 2,  IsDroppable = true  },
                new GameItem { Id = 15, Key = "potion_Red",             Name = "Red Potion",               Icon = "❤️", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Uncommon, GoldValue = 3,  IsDroppable = true  },
                new GameItem { Id = 16, Key = "potion_Shade",           Name = "Shade Potion",             Icon = "🖤", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Uncommon, GoldValue = 3,  IsDroppable = true  },
                new GameItem { Id = 17, Key = "potion_Skeleton",        Name = "Skeleton Potion",          Icon = "💀", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Uncommon, GoldValue = 3,  IsDroppable = true  },
                new GameItem { Id = 18, Key = "potion_Zombie",          Name = "Zombie Potion",            Icon = "🧟", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare,     GoldValue = 5,  IsDroppable = true  },
                new GameItem { Id = 19, Key = "potion_CottonCandyPink", Name = "Cotton Candy Pink Potion", Icon = "🩷", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare,     GoldValue = 5,  IsDroppable = true  },
                new GameItem { Id = 20, Key = "potion_CottonCandyBlue", Name = "Cotton Candy Blue Potion", Icon = "💙", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare,     GoldValue = 5,  IsDroppable = true  },
                new GameItem { Id = 21, Key = "potion_Golden",          Name = "Golden Potion",            Icon = "✨", Type = ItemType.HatchingPotion, Rarity = ItemRarity.VeryRare, GoldValue = 10, IsDroppable = true  },

                // ── PREMIUM / WACKY POTIONS (Ids 123-174) ── IsDroppable = false
                new GameItem { Id = 123, Key = "potion_Amber",        Name = "Amber Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 124, Key = "potion_Aquatic",      Name = "Aquatic Potion",       Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 125, Key = "potion_Aurora",       Name = "Aurora Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 126, Key = "potion_AutumnLeaf",   Name = "Autumn Leaf Potion",   Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 127, Key = "potion_Balloon",      Name = "Balloon Potion",       Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 128, Key = "potion_BirchBark",    Name = "Birch Bark Potion",    Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 129, Key = "potion_BlackPearl",   Name = "Black Pearl Potion",   Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 130, Key = "potion_Bronze",       Name = "Bronze Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 131, Key = "potion_Celestial",    Name = "Celestial Potion",     Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 132, Key = "potion_Cupid",        Name = "Cupid Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 133, Key = "potion_Ember",        Name = "Ember Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 134, Key = "potion_Fairy",        Name = "Fairy Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 135, Key = "potion_Floral",       Name = "Floral Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 136, Key = "potion_Fluorite",     Name = "Fluorite Potion",      Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 137, Key = "potion_Frost",        Name = "Frost Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 138, Key = "potion_Ghost",        Name = "Ghost Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 139, Key = "potion_Gingerbread",  Name = "Gingerbread Potion",   Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 140, Key = "potion_Glass",        Name = "Glass Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 141, Key = "potion_Glow",         Name = "Glow Potion",          Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 142, Key = "potion_Holly",        Name = "Holly Potion",         Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 143, Key = "potion_IcySnow",      Name = "Icy Snow Potion",      Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 144, Key = "potion_Jade",         Name = "Jade Potion",          Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 145, Key = "potion_Koi",          Name = "Koi Potion",           Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 146, Key = "potion_Moonglow",     Name = "Moonglow Potion",      Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 147, Key = "potion_MossyStone",   Name = "Mossy Stone Potion",   Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 148, Key = "potion_Onyx",         Name = "Onyx Potion",          Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 149, Key = "potion_Opal",         Name = "Opal Potion",          Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 150, Key = "potion_Peppermint",   Name = "Peppermint Potion",    Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 151, Key = "potion_PinkMarble",   Name = "Pink Marble Potion",   Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 152, Key = "potion_PolkaDot",     Name = "Polka Dot Potion",     Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 153, Key = "potion_Porcelain",    Name = "Porcelain Potion",     Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 154, Key = "potion_Purple",       Name = "Purple Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 155, Key = "potion_Rainbow",      Name = "Rainbow Potion",       Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 156, Key = "potion_RoseGold",     Name = "Rose Gold Potion",     Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 157, Key = "potion_RoseQuartz",   Name = "Rose Quartz Potion",   Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 158, Key = "potion_RoyalPurple",  Name = "Royal Purple Potion",  Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 159, Key = "potion_Ruby",         Name = "Ruby Potion",          Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 160, Key = "potion_SandSculpture",Name = "Sand Sculpture Potion",Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 161, Key = "potion_Shadow",       Name = "Shadow Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 162, Key = "potion_Shimmer",      Name = "Shimmer Potion",       Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 163, Key = "potion_Silver",       Name = "Silver Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 164, Key = "potion_SolarSystem",  Name = "Solar System Potion",  Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 165, Key = "potion_Spooky",       Name = "Spooky Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 166, Key = "potion_StainedGlass", Name = "Stained Glass Potion", Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 167, Key = "potion_StarryNight",  Name = "Starry Night Potion",  Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 168, Key = "potion_Sunset",       Name = "Sunset Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 169, Key = "potion_Sunshine",     Name = "Sunshine Potion",      Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 170, Key = "potion_TeaShop",      Name = "Tea Shop Potion",      Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 171, Key = "potion_Thunderstorm", Name = "Thunderstorm Potion",  Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 172, Key = "potion_Turquoise",    Name = "Turquoise Potion",     Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 173, Key = "potion_Vampire",      Name = "Vampire Potion",       Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                new GameItem { Id = 174, Key = "potion_Watery",       Name = "Watery Potion",        Icon = "🧪", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 2, IsDroppable = false },
                // Wacky potions — no potion image for most, but pet images exist; pets cannot evolve into mounts (except Windup)
                new GameItem { Id = 175, Key = "potion_Veggie",      Name = "Veggie Potion",        Icon = "🥦", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 0, IsDroppable = false },
                new GameItem { Id = 176, Key = "potion_Dessert",     Name = "Dessert Potion",       Icon = "🍰", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 0, IsDroppable = false },
                new GameItem { Id = 177, Key = "potion_VirtualPet",  Name = "Virtual Pet Potion",   Icon = "🎮", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 0, IsDroppable = false },
                new GameItem { Id = 178, Key = "potion_Fungi",       Name = "Fungi Potion",         Icon = "🍄", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 0, IsDroppable = false },
                new GameItem { Id = 179, Key = "potion_Cryptid",     Name = "Cryptid Potion",       Icon = "👾", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 0, IsDroppable = false },
                new GameItem { Id = 180, Key = "potion_Alien",       Name = "Alien Potion",         Icon = "👽", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 0, IsDroppable = false },
                new GameItem { Id = 181, Key = "potion_Windup",      Name = "Windup Potion",        Icon = "🤖", Type = ItemType.HatchingPotion, Rarity = ItemRarity.Rare, GoldValue = 0, IsDroppable = false },

                // ── QUEST SCROLLS (Phase 8, IDs 182-296) ── IsDroppable = false
                // Pet quests (182-241)
                new GameItem { Id=182, Key="quest_alligator",        Name="The Insta-Gator",                Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=183, Key="quest_alpaca",           Name="The Overpacked Alpaca",          Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=184, Key="quest_armadillo",        Name="The Indulgent Armadillo",        Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=185, Key="quest_axolotl",          Name="The Magical Axolotl",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=186, Key="quest_badger",           Name="Stop Badgering Me!",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=187, Key="quest_beetle",           Name="The CRITICAL BUG",               Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=188, Key="quest_bunny",            Name="The Killer Bunny",               Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=189, Key="quest_butterfly",        Name="Bye, Bye, Butterfry",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=190, Key="quest_cat",              Name="A Purrplexing Predicament",      Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=191, Key="quest_chameleon",        Name="The Chaotic Chameleon",          Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=192, Key="quest_cheetah",          Name="Such a Cheetah",                 Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=193, Key="quest_cow",              Name="The Mootant Cow",                Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=194, Key="quest_crab",             Name="The Fiddling Crab",              Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=195, Key="quest_dilatory_derby",   Name="The Dilatory Derby",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=196, Key="quest_dog",              Name="Triple Dog Dare!",               Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=197, Key="quest_dolphin",          Name="The Dolphin of Doubt",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=198, Key="quest_falcon",           Name="The Birds of Preycrastination",  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=199, Key="quest_ferret",           Name="The Nefarious Ferret",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=200, Key="quest_frog",             Name="Swamp of the Clutter Frog",      Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=201, Key="quest_ghost_stag",       Name="The Spirit of Spring",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=202, Key="quest_giraffe",          Name="The Gear-affe",                  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=203, Key="quest_gryphon",          Name="The Fiery Gryphon",              Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=204, Key="quest_guineapig",        Name="The Guinea Pig Gang",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=205, Key="quest_harpy",            Name="Help! Harpy!",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=206, Key="quest_hedgehog",         Name="The Hedgebeast",                 Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=207, Key="quest_hippo",            Name="What a Hippo-Crite",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=208, Key="quest_horse",            Name="Ride the Night-Mare",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=209, Key="quest_kangaroo",         Name="Kangaroo Catastrophe",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=210, Key="quest_kraken",           Name="The Kraken of Inkomplete",       Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=211, Key="quest_monkey",           Name="Monstrous Mandrill",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=212, Key="quest_nudibranch",       Name="Infestation of the NowDo Nudibranchs",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4, IsDroppable=false },
                new GameItem { Id=213, Key="quest_octopus",          Name="The Call of Octothulu",          Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=214, Key="quest_otter",            Name="The Perfidious Plotter!",        Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=215, Key="quest_owl",              Name="The Night-Owl",                  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=216, Key="quest_peacock",          Name="The Push-and-Pull Peacock",      Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=217, Key="quest_penguin",          Name="The Fowl Frost",                 Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=218, Key="quest_platypus",         Name="The Perfectionist Platypus",     Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=219, Key="quest_pterodactyl",      Name="The Pterror-dactyl",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=220, Key="quest_raccoon",          Name="Raccoon Tycoon",                 Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=221, Key="quest_rat",              Name="The Rat King",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=222, Key="quest_rock",             Name="Escape the Cave Creature",       Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=223, Key="quest_rooster",          Name="Rooster Rampage",                Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=224, Key="quest_sabretooth",       Name="The Sabre Cat",                  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=225, Key="quest_seaserpent",       Name="Sea Serpent Strike!",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=226, Key="quest_sheep",            Name="The Thunder Ram",                Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=227, Key="quest_slime",            Name="The Jelly Regent",               Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=228, Key="quest_sloth",            Name="The Somnolent Sloth",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=229, Key="quest_snail",            Name="The Snail of Drudgery Sludge",   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=230, Key="quest_snake",            Name="The Serpent of Distraction",     Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=231, Key="quest_spider",           Name="The Icy Arachnid",               Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=232, Key="quest_squirrel",         Name="The Sneaky Squirrel",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=233, Key="quest_treeling",         Name="The Tangle Tree",                Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=234, Key="quest_trex",             Name="King of the Dinosaurs",          Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=235, Key="quest_trex_undead",      Name="The Dinosaur Unearthed",         Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=236, Key="quest_triceratops",      Name="The Trampling Triceratops",      Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=237, Key="quest_turtle",           Name="Guide the Turtle",               Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=238, Key="quest_unicorn",          Name="Convincing the Unicorn Queen",   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=239, Key="quest_velociraptor",     Name="The Veloci-Rapper",              Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=240, Key="quest_whale",            Name="Wail of the Whale",              Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=241, Key="quest_yarn",             Name="A Tangled Yarn",                 Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                // Potion boss (242-247)
                new GameItem { Id=242, Key="quest_amber",            Name="The Amber Alliance",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=243, Key="quest_blackPearl",       Name="A Startling Starry Idea",        Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=244, Key="quest_bronze",           Name="Brazen Beetle Battle",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=245, Key="quest_fluorite",         Name="A Bright Fluorite Fright",       Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=246, Key="quest_jade",             Name="A Jaded Jinx",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=247, Key="quest_pinkMarble",       Name="Calm the Corrupted Cupid",       Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                // Potion collection (248-253)
                new GameItem { Id=248, Key="quest_onyx",             Name="The Onyx Odyssey",               Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=249, Key="quest_ruby",             Name="Ruby Rapport",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=250, Key="quest_silver",           Name="The Silver Solution",            Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=251, Key="quest_stone",            Name="A Maze of Moss",                 Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=252, Key="quest_turquoise",        Name="Turquoise Treasure Toil",        Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=253, Key="quest_opal",             Name="The Legend of the Obscure Opals",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                // Seasonal (254-260)
                new GameItem { Id=254, Key="quest_evilsanta",        Name="Trapper Santa",                  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=255, Key="quest_evilsanta2",       Name="Find the Cub",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=256, Key="quest_egg",              Name="Egg Hunt",                       Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=1,   IsDroppable=false },
                new GameItem { Id=257, Key="quest_waffle",           Name="Waffling with the Fool",         Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=258, Key="quest_virtualpet",       Name="Virtual Mayhem with the April Fool",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4, IsDroppable=false },
                new GameItem { Id=259, Key="quest_fungi",            Name="The Moody Mushroom",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=260, Key="quest_alien",            Name="Invasion of the Motivation Snatchers",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4, IsDroppable=false },
                // Series (261-275)
                new GameItem { Id=261, Key="quest_atom1",            Name="Attack of the Mundane, Part 1",  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=262, Key="quest_atom2",            Name="Attack of the Mundane, Part 2",  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=263, Key="quest_atom3",            Name="Attack of the Mundane, Part 3",  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=264, Key="quest_goldenknight1",    Name="The Golden Knight, Part 1",      Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=265, Key="quest_goldenknight2",    Name="The Golden Knight, Part 2",      Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=266, Key="quest_goldenknight3",    Name="The Golden Knight, Part 3",      Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=267, Key="quest_moon1",            Name="Lunar Battle, Part 1",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=268, Key="quest_moon2",            Name="Lunar Battle, Part 2",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=269, Key="quest_moon3",            Name="Lunar Battle, Part 3",           Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=270, Key="quest_moonstone1",       Name="Recidivate, Part 1",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=271, Key="quest_moonstone2",       Name="Recidivate, Part 2",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=272, Key="quest_moonstone3",       Name="Recidivate, Part 3",             Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=273, Key="quest_vice1",            Name="Vice, Part 1",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=274, Key="quest_vice2",            Name="Vice, Part 2",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                new GameItem { Id=275, Key="quest_vice3",            Name="Vice, Part 3",                   Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common, GoldValue=4,   IsDroppable=false },
                // Masterclasser (276-291)
                new GameItem { Id=276, Key="quest_dilatoryDistress1",  Name="Dilatory Distress, Part 1",    Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=200,  IsDroppable=false },
                new GameItem { Id=277, Key="quest_dilatoryDistress2",  Name="Dilatory Distress, Part 2",    Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=300,  IsDroppable=false },
                new GameItem { Id=278, Key="quest_dilatoryDistress3",  Name="Dilatory Distress, Part 3",    Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=400,  IsDroppable=false },
                new GameItem { Id=279, Key="quest_mayhemMistiflying1", Name="Mayhem in Mistiflying, Part 1",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=200,  IsDroppable=false },
                new GameItem { Id=280, Key="quest_mayhemMistiflying2", Name="Mayhem in Mistiflying, Part 2",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=300,  IsDroppable=false },
                new GameItem { Id=281, Key="quest_mayhemMistiflying3", Name="Mayhem in Mistiflying, Part 3",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=400,  IsDroppable=false },
                new GameItem { Id=282, Key="quest_stoikalmCalamity1",  Name="Stoïkalm Calamity, Part 1",    Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=200,  IsDroppable=false },
                new GameItem { Id=283, Key="quest_stoikalmCalamity2",  Name="Stoïkalm Calamity, Part 2",    Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=300,  IsDroppable=false },
                new GameItem { Id=284, Key="quest_stoikalmCalamity3",  Name="Stoïkalm Calamity, Part 3",    Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=400,  IsDroppable=false },
                new GameItem { Id=285, Key="quest_taskwoodsTerror1",   Name="Terror in the Taskwoods, Part 1",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=200,IsDroppable=false },
                new GameItem { Id=286, Key="quest_taskwoodsTerror2",   Name="Terror in the Taskwoods, Part 2",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=300,IsDroppable=false },
                new GameItem { Id=287, Key="quest_taskwoodsTerror3",   Name="Terror in the Taskwoods, Part 3",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=400,IsDroppable=false },
                new GameItem { Id=288, Key="quest_lostMasterclasser1", Name="Mystery of the Masterclassers, Part 1",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=400, IsDroppable=false },
                new GameItem { Id=289, Key="quest_lostMasterclasser2", Name="Mystery of the Masterclassers, Part 2",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=500, IsDroppable=false },
                new GameItem { Id=290, Key="quest_lostMasterclasser3", Name="Mystery of the Masterclassers, Part 3",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=600, IsDroppable=false },
                new GameItem { Id=291, Key="quest_lostMasterclasser4", Name="Mystery of the Masterclassers, Part 4",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Rare, GoldValue=700, IsDroppable=false },
                // Time travel (292-294) — not purchasable
                new GameItem { Id=292, Key="quest_robot",            Name="Mysterious Mechanical Marvels!", Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.VeryRare, GoldValue=0,IsDroppable=false },
                new GameItem { Id=293, Key="quest_solarSystem",      Name="A Voyage of Cosmic Concentration",Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.VeryRare, GoldValue=0,IsDroppable=false },
                new GameItem { Id=294, Key="quest_windup",           Name="A Whirl with a Wind-Up Warrior",  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.VeryRare, GoldValue=0,IsDroppable=false },
                // Generic (295-296)
                new GameItem { Id=295, Key="quest_basilist",         Name="The Basi-List",                  Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Uncommon, GoldValue=100, IsDroppable=false },
                new GameItem { Id=296, Key="quest_dustbunnies",      Name="The Feral Dust Bunnies",         Icon="📜", Type=ItemType.QuestScroll, Rarity=ItemRarity.Common,   GoldValue=1,   IsDroppable=false }
            );

            // ===== GUILDS & PARTIES (Phase 7) =====

            // Guild → Leader (Restrict)
            modelBuilder.Entity<Guild>()
                .HasOne(g => g.Leader)
                .WithMany()
                .HasForeignKey(g => g.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Guild name unique
            modelBuilder.Entity<Guild>()
                .HasIndex(g => g.Name)
                .IsUnique();

            // GuildMember → Guild (Cascade)
            modelBuilder.Entity<GuildMember>()
                .HasOne(m => m.Guild)
                .WithMany(g => g.Members)
                .HasForeignKey(m => m.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            // GuildMember → User (Restrict)
            modelBuilder.Entity<GuildMember>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One membership per (guild, user)
            modelBuilder.Entity<GuildMember>()
                .HasIndex(m => new { m.GuildId, m.UserId })
                .IsUnique();

            // GuildMessage → Guild (Cascade)
            modelBuilder.Entity<GuildMessage>()
                .HasOne(m => m.Guild)
                .WithMany(g => g.Messages)
                .HasForeignKey(m => m.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            // GuildMessage → Author (Restrict)
            modelBuilder.Entity<GuildMessage>()
                .HasOne(m => m.Author)
                .WithMany()
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for chat pagination
            modelBuilder.Entity<GuildMessage>()
                .HasIndex(m => new { m.GuildId, m.SentAt });

            // GuildMessageLike composite PK
            modelBuilder.Entity<GuildMessageLike>()
                .HasKey(l => new { l.GuildMessageId, l.LikerUserId });

            modelBuilder.Entity<GuildMessageLike>()
                .HasOne(l => l.GuildMessage)
                .WithMany(m => m.Likes)
                .HasForeignKey(l => l.GuildMessageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GuildMessageLike>()
                .HasOne(l => l.LikerUser)
                .WithMany()
                .HasForeignKey(l => l.LikerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // GuildInvite → Guild (Cascade)
            modelBuilder.Entity<GuildInvite>()
                .HasOne(i => i.Guild)
                .WithMany(g => g.Invites)
                .HasForeignKey(i => i.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            // GuildInvite → Inviter (Restrict)
            modelBuilder.Entity<GuildInvite>()
                .HasOne(i => i.Inviter)
                .WithMany()
                .HasForeignKey(i => i.InviterId)
                .OnDelete(DeleteBehavior.Restrict);

            // GuildInvite → Invitee (Restrict)
            modelBuilder.Entity<GuildInvite>()
                .HasOne(i => i.Invitee)
                .WithMany()
                .HasForeignKey(i => i.InviteeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for pending invite lookup
            modelBuilder.Entity<GuildInvite>()
                .HasIndex(i => new { i.GuildId, i.InviteeId });

            // ── PARTY ────────────────────────────────────────────────────────────────

            // Party → Leader (Restrict)
            modelBuilder.Entity<Party>()
                .HasOne(p => p.Leader)
                .WithMany()
                .HasForeignKey(p => p.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // PartyMember → Party (Cascade)
            modelBuilder.Entity<PartyMember>()
                .HasOne(m => m.Party)
                .WithMany(p => p.Members)
                .HasForeignKey(m => m.PartyId)
                .OnDelete(DeleteBehavior.Cascade);

            // PartyMember → User (Restrict)
            modelBuilder.Entity<PartyMember>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One membership per (party, user)
            modelBuilder.Entity<PartyMember>()
                .HasIndex(m => new { m.PartyId, m.UserId })
                .IsUnique();

            // Enforce 1 party per user globally (unique index on UserId in PartyMembers)
            modelBuilder.Entity<PartyMember>()
                .HasIndex(m => m.UserId)
                .IsUnique();

            // PartyMessage → Party (Cascade)
            modelBuilder.Entity<PartyMessage>()
                .HasOne(m => m.Party)
                .WithMany(p => p.Messages)
                .HasForeignKey(m => m.PartyId)
                .OnDelete(DeleteBehavior.Cascade);

            // PartyMessage → Author (Restrict)
            modelBuilder.Entity<PartyMessage>()
                .HasOne(m => m.Author)
                .WithMany()
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for chat pagination
            modelBuilder.Entity<PartyMessage>()
                .HasIndex(m => new { m.PartyId, m.SentAt });

            // PartyMessageLike composite PK
            modelBuilder.Entity<PartyMessageLike>()
                .HasKey(l => new { l.PartyMessageId, l.LikerUserId });

            modelBuilder.Entity<PartyMessageLike>()
                .HasOne(l => l.PartyMessage)
                .WithMany(m => m.Likes)
                .HasForeignKey(l => l.PartyMessageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PartyMessageLike>()
                .HasOne(l => l.LikerUser)
                .WithMany()
                .HasForeignKey(l => l.LikerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // PartyInvite → Party (Cascade)
            modelBuilder.Entity<PartyInvite>()
                .HasOne(i => i.Party)
                .WithMany(p => p.Invites)
                .HasForeignKey(i => i.PartyId)
                .OnDelete(DeleteBehavior.Cascade);

            // PartyInvite → Inviter (Restrict)
            modelBuilder.Entity<PartyInvite>()
                .HasOne(i => i.Inviter)
                .WithMany()
                .HasForeignKey(i => i.InviterId)
                .OnDelete(DeleteBehavior.Restrict);

            // PartyInvite → Invitee (Restrict)
            modelBuilder.Entity<PartyInvite>()
                .HasOne(i => i.Invitee)
                .WithMany()
                .HasForeignKey(i => i.InviteeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for pending invite lookup
            modelBuilder.Entity<PartyInvite>()
                .HasIndex(i => new { i.PartyId, i.InviteeId });

            // ── BOSS QUESTS (Phase 8) ────────────────────────────────────────────────
            modelBuilder.Entity<BossQuest>()
                .HasIndex(b => b.Key)
                .IsUnique();
            modelBuilder.Entity<BossQuest>()
                .HasData(BossQuestSeed.GetAll());

            // PartyQuest → Party (Cascade)
            modelBuilder.Entity<PartyQuest>()
                .HasOne(pq => pq.Party)
                .WithMany()
                .HasForeignKey(pq => pq.PartyId)
                .OnDelete(DeleteBehavior.Cascade);

            // PartyQuest → BossQuest (Restrict)
            modelBuilder.Entity<PartyQuest>()
                .HasOne(pq => pq.BossQuest)
                .WithMany()
                .HasForeignKey(pq => pq.BossQuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // PartyQuest → Leader (Restrict)
            modelBuilder.Entity<PartyQuest>()
                .HasOne(pq => pq.Leader)
                .WithMany()
                .HasForeignKey(pq => pq.LeaderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // PartyQuestMember → PartyQuest (Cascade)
            modelBuilder.Entity<PartyQuestMember>()
                .HasOne(m => m.PartyQuest)
                .WithMany(pq => pq.Members)
                .HasForeignKey(m => m.PartyQuestId)
                .OnDelete(DeleteBehavior.Cascade);

            // PartyQuestMember → User (Restrict)
            modelBuilder.Entity<PartyQuestMember>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One membership per (partyQuest, user)
            modelBuilder.Entity<PartyQuestMember>()
                .HasIndex(m => new { m.PartyQuestId, m.UserId })
                .IsUnique();
        }
    }
}