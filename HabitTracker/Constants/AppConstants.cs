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
        public const int XP_PER_LEVEL = 100; // kept for legacy reference
        public const int MAX_LEVEL = 100;

        // Scaling level system — XP to reach level N = 100 + (N-1)*50
        // Total cumulative XP to reach level N = 25*(N-1)*(N+2)
        public static class LevelSystem
        {
            // Total XP needed to reach the START of level N (level 1 = 0 XP)
            public static int TotalXPForLevel(int level)
            {
                if (level <= 1) return 0;
                level = Math.Min(level, MAX_LEVEL);
                return 25 * (level - 1) * (level + 2);
            }

            // XP gap between level N and N+1
            public static int XPToNextLevel(int level)
            {
                if (level >= MAX_LEVEL) return 0;
                return 100 + (level - 1) * 50;
            }

            // Derive level from total accumulated XP, capped at MAX_LEVEL
            public static int CalculateLevel(int xp)
            {
                if (xp <= 0) return 1;
                // Solving 25*(N-1)*(N+2) = xp  →  N^2+N-(2+xp/25)=0
                int level = (int)Math.Floor((-1.0 + Math.Sqrt(9.0 + 4.0 * xp / 25.0)) / 2.0) + 1;
                return Math.Min(level, MAX_LEVEL);
            }

            // Bonus base-stat points granted per level gained
            public const int STAT_POINTS_PER_LEVEL = 1; // +1 to every base stat per level
        }

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

        // ===== RPG STAT GROWTH =====
        public static class RpgStats
        {
            // Returns (str, will, intel, agl, end) bonus per quest completion by category
            public static (int str, int will, int intel, int agl, int end) GetCategoryBonus(string category) => category switch
            {
                "Sức khỏe" => (2, 0, 0, 1, 0),
                "Học tập"  => (0, 0, 2, 0, 0),
                "Tinh thần" => (0, 2, 0, 0, 1),
                "Tài chính" => (0, 1, 1, 0, 0),
                _           => (1, 0, 0, 0, 0) // custom categories → base STR
            };

            // Extra points added to the category's primary stat for harder quests
            public static int GetDifficultyBonus(string difficulty) => difficulty switch
            {
                "Medium" => 1,
                "Hard"   => 2,
                _        => 0
            };

            // Hard quests always grant +1 END
            public const int HARD_END_BONUS = 1;

            // Daily quests grant +1 END (consistency training)
            public const int DAILY_END_BONUS = 1;

            // Every quest completion grants +1 AGL (reflexes improve with practice)
            public const int QUEST_AGL_BONUS = 1;
        }
    }
}