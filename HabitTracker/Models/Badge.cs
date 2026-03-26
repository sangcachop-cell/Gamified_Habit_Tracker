namespace HabitTracker.Models
{
    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int RequiredXP { get; set; }

        public List<UserBadge>? UserBadges { get; set; }
    }
}