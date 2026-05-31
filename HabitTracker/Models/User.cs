using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    /// <summary>
    /// Mô hình người dùng
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        // ===== ACCOUNT INFO =====
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        // ===== AVATAR =====
        [StringLength(255)]
        public string? Avatar { get; set; }

        // ===== GAMIFICATION =====
        public int Level { get; set; } = 1;

        public int XP { get; set; } = 0;

        public int CurrentStreak { get; set; } = 0;

        public int LongestStreak { get; set; } = 0;

        public DateTime? LastCheckInDate { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        public DateTime? LastCronDate { get; set; }

        // ===== PROFILE INFORMATION =====
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        // ===== SOCIAL LINKS =====
        [StringLength(255)]
        public string? FacebookLink { get; set; }

        [StringLength(255)]
        public string? LinkedInLink { get; set; }

        [StringLength(255)]
        public string? InstagramLink { get; set; }

        // ===== ADMIN & AUDIT =====
        public bool IsAdmin { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        // ===== RELATIONSHIPS =====
        public List<UserBadge>? UserBadges { get; set; }

        public List<UserQuest>? UserQuests { get; set; }

        // ===== METHODS =====
        /// <summary>
        /// Check if user has a specific badge
        /// </summary>
        public bool HasBadge(int badgeId)
        {
            return UserBadges?.Any(ub => ub.BadgeId == badgeId) ?? false;
        }

        /// <summary>
        /// Get user's earned badges
        /// </summary>
        public List<Badge> GetEarnedBadges()
        {
            return UserBadges?
                .Select(ub => ub.Badge)
                .Where(b => b != null)
                .ToList() ?? new List<Badge>();
        }

        /// <summary>
        /// Get number of quests completed today
        /// </summary>
        public int GetCompletedQuestsTodayCount()
        {
            return UserQuests?
                .Count(uq => uq.CompletedDate == DateTime.Today) ?? 0;
        }

        [StringLength(255)]
        public string? CoverImage { get; set; }

        [StringLength(255)]
        public string? FacebookUrl { get; set; }

        [StringLength(255)]
        public string? InstagramUrl { get; set; }

        [StringLength(255)]
        public string? LinkedInUrl { get; set; }

        [StringLength(255)]
        public string? TwitterUrl { get; set; }

        // Gamification
        public int TotalQuestsCompleted { get; set; } = 0;
        public int TotalXPEarned { get; set; } = 0;
        public DateTime? LastActiveDate { get; set; }

        // ===== ECONOMY (Phase 2) =====
        public double HP          { get; set; } = 50.0;
        public double Mana        { get; set; } = 30.0;
        public double Gold        { get; set; } = 0.0;
        public bool   IsSleeping  { get; set; } = false;

        // ===== GEMS (Phase 4.3) =====
        public int Gems { get; set; } = 0;
        public int    DailyDropCount    { get; set; } = 0;
        public DateTime? LastDropResetDate { get; set; }

        // ===== STATS (Phase 3) =====
        public int STR { get; set; } = 0;
        public int CON { get; set; } = 0;
        public int INT { get; set; } = 0;
        public int PER { get; set; } = 0;

        // ===== CHARACTER SYSTEM (Phase 3) =====

        /// <summary>warrior | mage | rogue | healer | null (unselected)</summary>
        [StringLength(20)]
        public string? Class { get; set; }

        /// <summary>Unallocated stat points (1 per level up)</summary>
        public int StatPoints { get; set; } = 0;

        // ===== AVATAR CUSTOMIZATION (Phase 3.7) =====
        [StringLength(20)]
        public string BodyType { get; set; } = "broad";       // broad | slim

        [StringLength(50)]
        public string SkinColor { get; set; } = "915533";     // hex code or named

        public int HairStyle   { get; set; } = 1;             // base style 1-20
        public int HairBangs   { get; set; } = 1;             // bangs style 1-4
        public int HairBeard   { get; set; } = 0;             // 0=none, 1-3
        public int HairMustache { get; set; } = 0;            // 0=none, 1-2

        [StringLength(50)]
        public string HairColor { get; set; } = "black";

        [StringLength(50)]
        public string? ShirtStyle { get; set; } = "black";

        // ===== EQUIPPED GEAR (string Keys, null=unequipped) =====
        [StringLength(100)]
        public string? EquippedWeapon         { get; set; }
        [StringLength(100)]
        public string? EquippedArmor          { get; set; }
        [StringLength(100)]
        public string? EquippedHead           { get; set; }
        [StringLength(100)]
        public string? EquippedShield         { get; set; }
        [StringLength(100)]
        public string? EquippedBack           { get; set; }
        [StringLength(100)]
        public string? EquippedHeadAccessory  { get; set; }
        [StringLength(100)]
        public string? EquippedEyewear        { get; set; }
        [StringLength(100)]
        public string? EquippedBody           { get; set; }

        // ===== COSTUME MODE (Phase 3.4) =====
        public bool CostumeModeEnabled { get; set; } = false;
        [StringLength(100)]
        public string? CostumeWeapon        { get; set; }
        [StringLength(100)]
        public string? CostumeArmor         { get; set; }
        [StringLength(100)]
        public string? CostumeHead          { get; set; }
        [StringLength(100)]
        public string? CostumeShield        { get; set; }
        [StringLength(100)]
        public string? CostumeBack          { get; set; }
        [StringLength(100)]
        public string? CostumeHeadAccessory { get; set; }
        [StringLength(100)]
        public string? CostumeEyewear       { get; set; }
        [StringLength(100)]
        public string? CostumeBody          { get; set; }

        // ===== BUFFS (Phase 3.5) =====
        public int BuffSTR { get; set; } = 0;
        public int BuffCON { get; set; } = 0;
        public int BuffINT { get; set; } = 0;
        public int BuffPER { get; set; } = 0;
        public DateTime? BuffExpiry { get; set; }

        // ===== CLASS SPELL BUFFS (Phase 3.2) =====
        /// <summary>Rogue Stealth: number of daily damage instances to absorb (decrements per missed daily on cron)</summary>
        public int StealthBuff { get; set; } = 0;
        /// <summary>Mage Chilling Frost: when true, cron skips all streak changes this day, then resets to false</summary>
        public bool FrozenStreaks { get; set; } = false;

        // ===== REBIRTH (Phase 3.6) =====
        public int RebirthCount { get; set; } = 0;

        // ===== STABLE (Phase 5) =====
        [StringLength(100)]
        public string? ActivePetKey   { get; set; }

        [StringLength(100)]
        public string? ActiveMountKey { get; set; }

        // ===== BACKGROUND (Phase 9) =====
        [StringLength(100)]
        public string? Background { get; set; }

        // ===== RELATIONSHIPS =====
        public virtual List<Notification>? Notifications { get; set; }
        public virtual List<Category>? CreatedCategories { get; set; }
        public virtual List<UserInventoryItem>? InventoryItems { get; set; }
        public virtual List<UserGearItem>? OwnedGear { get; set; }
        public virtual List<UserPet>? OwnedPets { get; set; }
    }
}