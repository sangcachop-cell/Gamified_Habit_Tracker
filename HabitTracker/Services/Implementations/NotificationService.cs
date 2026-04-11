using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(AppDbContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Notification> CreateNotificationAsync(
            int userId, string title, string? message, string type, string? link, string? icon)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                Link = link,
                Icon = icon ?? "ℹ️",
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Notification created for user {userId}: {title}");
            return notification;
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetNotificationsAsync(int userId, int limit = 20)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var unread = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var n in unread)
            {
                n.IsRead = true;
                n.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task NotifyBadgeEarnedAsync(int userId, Badge badge)
        {
            await CreateNotificationAsync(
                userId,
                $"🏆 Badge mới: {badge.Name}",
                $"Bạn đã đạt được badge \"{badge.Name}\"!",
                "Achievement",
                "/dashboard",
                "🏆"
            );
        }

        public async Task NotifyStreakMilestoneAsync(int userId, int streak)
        {
            await CreateNotificationAsync(
                userId,
                $"🔥 Streak {streak} ngày!",
                $"Tuyệt vời! Bạn đã duy trì streak {streak} ngày liên tiếp!",
                "Streak",
                "/dashboard",
                "🔥"
            );
        }

        public async Task NotifyLevelUpAsync(int userId, int newLevel)
        {
            await CreateNotificationAsync(
                userId,
                $"⬆️ Level up: {newLevel}",
                $"Chúc mừng! Bạn đã lên level {newLevel}!",
                "LevelUp",
                "/dashboard",
                "⬆️"
            );
        }

        public async Task NotifyQuestCompletedAsync(int userId, Quest quest)
        {
            await CreateNotificationAsync(
                userId,
                $"✅ Đã hoàn thành: {quest.Name}",
                $"Bạn vừa hoàn thành quest \"{quest.Name}\" và nhận {quest.XPReward} XP!",
                "Quest",
                $"/task/details/{quest.Id}",
                quest.Icon ?? "✅"
            );
        }
    }
}