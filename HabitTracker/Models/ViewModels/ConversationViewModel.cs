using HabitTracker.Models;

namespace HabitTracker.Models.ViewModels;

public class ConversationViewModel
{
    public User OtherUser { get; set; } = null!;
    public List<MessageEntry> Messages { get; set; } = new();
    public bool IsBlocked { get; set; }      // current user has blocked other
    public bool IsBlockedBy { get; set; }    // other has blocked current user
    public int Page { get; set; }
    public bool HasMore { get; set; }
}

public class MessageEntry
{
    public Message Message { get; set; } = null!;
    public bool LikedByMe { get; set; }
    public int LikeCount { get; set; }
}
