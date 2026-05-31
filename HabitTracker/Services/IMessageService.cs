using HabitTracker.Models;
using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services;

public interface IMessageService
{
    // Core messaging
    Task<(bool Success, string? Error, Message? Msg)> SendAsync(int senderId, int receiverId, string body);
    Task<List<ConversationSummary>> GetConversationsAsync(int userId);
    Task<List<MessageEntry>> GetConversationAsync(int userId, int otherId, int page = 0);
    Task MarkReadAsync(int userId, int otherId);
    Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId);
    Task<int> GetUnreadCountAsync(int userId);

    // Likes
    Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId);

    // Block
    Task<bool> IsBlockedAsync(int userA, int userB);
    Task<bool> ToggleBlockAsync(int blockerId, int blockedId);
    Task<bool> IsBlockerAsync(int blockerId, int blockedId);

    // Report
    Task<(bool Success, string? Error)> ReportUserAsync(int reporterId, int reportedUserId, string reason);
    Task<(bool Success, string? Error)> ReportMessageAsync(int reporterId, int messageId, string reason);
}
