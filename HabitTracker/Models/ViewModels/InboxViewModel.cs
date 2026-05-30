using HabitTracker.Models;

namespace HabitTracker.Models.ViewModels;

public class ConversationSummary
{
    public User OtherUser { get; set; } = null!;
    public Message LastMessage { get; set; } = null!;
    public int UnreadCount { get; set; }
    public bool IsBlocked { get; set; }
}

public class InboxViewModel
{
    public List<ConversationSummary> Conversations { get; set; } = new();
    public int TotalUnread { get; set; }
    public int? ActiveConversationUserId { get; set; }
    public ConversationViewModel? ActiveConversation { get; set; }
}
