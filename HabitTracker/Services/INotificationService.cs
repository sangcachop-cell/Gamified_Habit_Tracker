using HabitTracker.Models;

namespace HabitTracker.Services
{
    public interface INotificationService
    {
        // Create notifications
        Task<Notification> CreateNotificationAsync(int userId, string title, string? message, string type, string? link, string? icon);

        // Get unread notifications
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);

        // Get all notifications
        Task<List<Notification>> GetNotificationsAsync(int userId, int limit = 20);

        // Mark as read
        Task<bool> MarkAsReadAsync(int notificationId);

        // Mark all as read
        Task<bool> MarkAllAsReadAsync(int userId);

        // Delete notification
        Task<bool> DeleteNotificationAsync(int notificationId);

        // Count unread
        Task<int> GetUnreadCountAsync(int userId);

        // Auto-create notifications for events
        Task NotifyBadgeEarnedAsync(int userId, Badge badge);
        Task NotifyStreakMilestoneAsync(int userId, int streak);
        Task NotifyLevelUpAsync(int userId, int newLevel);
        Task NotifyQuestCompletedAsync(int userId, Quest quest);
    }
}