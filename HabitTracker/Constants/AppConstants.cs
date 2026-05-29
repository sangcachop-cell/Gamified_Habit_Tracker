namespace HabitTracker.Constants
{
    /// <summary>
    /// Tập hợp tất cả constants để tránh magic strings
    /// </summary>
    public static class AppConstants
    {
        // ===== FILE UPLOAD =====
        public const string DEFAULT_AVATAR = "default.png";
        public const string IMAGES_FOLDER = "images";
        public const long MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
        public static readonly string[] ALLOWED_EXTENSIONS = { ".jpg", ".jpeg", ".png", ".gif" };

        // ===== QUEST CATEGORIES =====
        public static class Categories
        {
            public const string HEALTH = "Sức khỏe";
            public const string STUDY = "Học tập";
            public const string MINDFULNESS = "Tinh thần";
            public const string FINANCE = "Tài chính";

            public static readonly string[] All = { HEALTH, STUDY, MINDFULNESS, FINANCE };
        }

        // ===== QUEST DIFFICULTY =====
        public static class Difficulty
        {
            public const string EASY = "Easy";
            public const string MEDIUM = "Medium";
            public const string HARD = "Hard";

            public static readonly string[] All = { EASY, MEDIUM, HARD };
        }

        // ===== QUEST FREQUENCY =====
        public static class Frequency
        {
            public const string DAILY = "Daily";
            public const string WEEKLY = "Weekly";
            public const string MONTHLY = "Monthly";

            public static readonly string[] All = { DAILY, WEEKLY, MONTHLY };
        }

        // ===== XP REWARDS (tương ứng với Difficulty) =====
        public static class XPRewards
        {
            public const int EASY = 10;
            public const int MEDIUM = 25;
            public const int HARD = 50;

            public static int GetRewardByDifficulty(string difficulty) => difficulty switch
            {
                Difficulty.MEDIUM => MEDIUM,
                Difficulty.HARD => HARD,
                _ => EASY
            };
        }

        // ===== LEVEL & XP =====
        public const int XP_PER_LEVEL = 100;   // kept for badge backward-compat
        public const int MAX_LEVEL    = 100;    // Habitica cap

        // ===== BADGES =====
        public static class Badges
        {
            public const int BEGINNER_REQUIRED_XP = 50;
            public const int WARRIOR_REQUIRED_XP = 200;
            public const int LEGEND_REQUIRED_XP = 500;
        }

        // ===== SESSION & AUTHENTICATION =====
        public const string SESSION_USER_ID = "UserId";
        public const string SESSION_USERNAME = "Username";
        public const string SESSION_AVATAR = "Avatar";
        public const string SESSION_IS_ADMIN = "IsAdmin";
        public const string GOOGLE_LOGIN_PASSWORD = "GOOGLE_LOGIN";

        // ===== VALIDATION =====
        public const int MIN_USERNAME_LENGTH = 3;
        public const int MAX_USERNAME_LENGTH = 50;
        public const int MIN_PASSWORD_LENGTH = 6;
        public const int MAX_PASSWORD_LENGTH = 100;

        // ===== ERROR MESSAGES =====
        public static class Messages
        {
            // Register
            public const string EMAIL_EXISTS = "Email đã tồn tại!";
            public const string INVALID_EMAIL = "Email không hợp lệ!";
            public const string PASSWORD_TOO_SHORT = "Mật khẩu phải tối thiểu 6 ký tự!";

            // Login
            public const string INVALID_CREDENTIALS = "Email hoặc mật khẩu sai!";

            // Authorization
            public const string NOT_AUTHORIZED = "Bạn không có quyền truy cập!";
            public const string MUST_LOGIN = "Bạn phải đăng nhập!";

            // Quest
            public const string NO_QUEST_SELECTED = "Bạn chưa chọn nhiệm vụ nào!";
            public const string ALL_QUESTS_DONE = "Tất cả nhiệm vụ đã xác nhận hôm nay rồi!";

            // File
            public const string FILE_TOO_LARGE = "File quá lớn! Tối đa 5MB.";
            public const string INVALID_FILE_TYPE = "Định dạng file không được phép!";

            // Password
            public const string WRONG_PASSWORD = "Mật khẩu cũ không chính xác!";
        }

        // ===== SUCCESS MESSAGES =====
        public static class Toasts
        {
            public const string LOGIN_SUCCESS = "Đăng nhập thành công!";
            public const string LOGOUT_SUCCESS = "Đăng xuất thành công!";
            public const string PROFILE_UPDATED = "Cập nhật hồ sơ thành công!";
            public const string PASSWORD_CHANGED = "Đổi mật khẩu thành công!";
            public const string QUEST_CREATED = "✅ Đã thêm quest: ";
            public const string QUEST_UPDATED = "✅ Đã cập nhật quest: ";
            public const string QUEST_DELETED = "🗑️ Đã ẩn quest: ";
            public const string QUEST_RESTORED = "♻️ Đã khôi phục quest: ";
        }

        // ===== LEADERBOARD =====
        public const int LEADERBOARD_TOP_COUNT = 10;

        // ===== TASK SYSTEM (Phase 1) =====
        public static class TaskXPRewards
        {
            public const int TRIVIAL = 5;
            public const int EASY    = 10;
            public const int MEDIUM  = 15;
            public const int HARD    = 25;

            public static int GetByPriority(TaskPriority priority) => priority switch
            {
                TaskPriority.Trivial => TRIVIAL,
                TaskPriority.Easy    => EASY,
                TaskPriority.Medium  => MEDIUM,
                TaskPriority.Hard    => HARD,
                _                    => EASY
            };
        }

        public static class TaskValueLimits
        {
            public const double MIN   = -47.27;
            public const double MAX   =  21.27;
            public const double DECAY =   0.9747;
        }

        // ===== ECONOMY (Phase 2) =====
        public const double MAX_HP             = 50.0;
        // ===== SHOP (Phase 4) =====
        public const int    GEM_GOLD_COST           = 25;   // 25 GP = 1 Gem
        public const int    ARMOIRE_COST            = 100;  // GP per pull
        public const double ARMOIRE_GEAR_THRESHOLD  = 0.6;  // [0, 0.6) → gear
        public const double ARMOIRE_FOOD_THRESHOLD  = 0.8;  // [0.6, 0.8) → food; else XP
        public const int    ARMOIRE_XP_MIN          = 10;
        public const int    ARMOIRE_XP_MAX          = 49;
        public const double MANA_BASE          = 30.0;
        public const double CRIT_CHANCE        = 0.03;
        public const double CRIT_MULT          = 1.5;
        public const double HEALTH_POTION_COST = 25.0;
        public const double HEALTH_POTION_HEAL = 15.0;
        public const int    DAILY_DROP_CAP     = 5;

        public static class PriorityMultipliers
        {
            public static double Get(TaskPriority p) => p switch
            {
                TaskPriority.Trivial => 0.1,
                TaskPriority.Easy    => 1.0,
                TaskPriority.Medium  => 1.5,
                TaskPriority.Hard    => 2.0,
                _                    => 1.0
            };
        }
    }

    // ===== TASK ENUMS =====
    public enum TaskType
    {
        Habit  = 0,
        Daily  = 1,
        Todo   = 2,
        Reward = 3
    }

    public enum TaskPriority
    {
        Trivial = 0,
        Easy    = 1,
        Medium  = 2,
        Hard    = 3
    }

    public enum HabitResetFrequency
    {
        Daily   = 0,
        Weekly  = 1,
        Monthly = 2
    }

    public enum DailyFrequency
    {
        Daily   = 0,
        Weekly  = 1,
        Monthly = 2,
        Yearly  = 3
    }

    public enum ItemType   { Food = 0, Egg = 1, HatchingPotion = 2 }
    public enum ItemRarity { Common = 0, Uncommon = 1, Rare = 2, VeryRare = 3 }

    // ===== CHARACTER SYSTEM (Phase 3) =====
    public static class CharacterClass
    {
        public const string Warrior = "warrior";
        public const string Mage    = "mage";
        public const string Rogue   = "rogue";
        public const string Healer  = "healer";

        public static readonly string[] All = { Warrior, Mage, Rogue, Healer };

        /// <summary>Our "mage" maps to Habitica's "wizard" for image file paths.</summary>
        public static string ToHabiticaKey(string cls) =>
            cls == Mage ? "wizard" : cls;
    }

    public static class GearSlot
    {
        public const string Weapon          = "weapon";
        public const string Armor           = "armor";
        public const string Head            = "head";
        public const string Shield          = "shield";
        public const string Back            = "back";
        public const string HeadAccessory   = "headAccessory";
        public const string Eyewear         = "eyewear";
        public const string Body            = "body";
    }
}