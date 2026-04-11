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
        public const int XP_PER_LEVEL = 100;

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
    }
}