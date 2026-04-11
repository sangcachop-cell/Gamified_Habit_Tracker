using HabitTracker.Constants;
using HabitTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(int limit = 20)
    {
        var userId = int.Parse(HttpContext.Session.GetString(AppConstants.SESSION_USER_ID) ?? "0");
        var notifications = await _notificationService.GetNotificationsAsync(userId, limit);
        return Ok(notifications);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var userId = int.Parse(HttpContext.Session.GetString(AppConstants.SESSION_USER_ID) ?? "0");
        var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
        return Ok(notifications);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = int.Parse(HttpContext.Session.GetString(AppConstants.SESSION_USER_ID) ?? "0");
        var count = await _notificationService.GetUnreadCountAsync(userId);
        return Ok(new { count });
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var success = await _notificationService.MarkAsReadAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = int.Parse(HttpContext.Session.GetString(AppConstants.SESSION_USER_ID) ?? "0");
        await _notificationService.MarkAllAsReadAsync(userId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var success = await _notificationService.DeleteNotificationAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}