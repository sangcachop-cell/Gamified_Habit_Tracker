namespace HabitTracker.Models.ViewModels
{
    public class HomeViewModel
    {
        // Stats strip
        public int TotalUsers { get; set; }
        public int TotalQuestsCompleted { get; set; }
        public int TotalBadges { get; set; }
        public int TotalQuests { get; set; }

        // Leaderboard top 5
        public List<LeaderboardEntry> TopUsers { get; set; } = new();
    }

    public class LeaderboardEntry
    {
        public string Username { get; set; } = "";
        public string? Avatar { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public int CurrentStreak { get; set; }
        public string? TopBadgeName { get; set; }
        public string? TopBadgeIcon { get; set; }
        public string? TopBadgeRarity { get; set; }
    }
}