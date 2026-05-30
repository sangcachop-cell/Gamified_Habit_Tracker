namespace HabitTracker.Models.ViewModels
{
    public class CustomizeViewModel
    {
        public User   User             { get; set; } = null!;
        public List<string> SkinColors { get; set; } = new();
        public List<string> HairColors { get; set; } = new();
        public int  HairStyleCount     { get; set; } = 20;
        public int  HairBangsCount     { get; set; } = 4;
        public int  HairBeardCount     { get; set; } = 3;
        public int  HairMustacheCount  { get; set; } = 2;
        public List<string> ShirtStyles  { get; set; } = new();
        public List<string> Backgrounds  { get; set; } = new();
    }
}
