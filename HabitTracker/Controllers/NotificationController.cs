using HabitTracker.Constants;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);

    [HttpGet]
    public async Task<IActionResult> GetNotifications(int limit = 20)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        var notifications = await _notificationService.GetNotificationsAsync(userId.Value, limit);
        return Ok(notifications);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        var notifications = await _notificationService.GetUnreadNotificationsAsync(userId.Value);
        return Ok(notifications);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = GetUserId();
        if (userId == null) return Ok(new { count = 0 });
        var count = await _notificationService.GetUnreadCountAsync(userId.Value);
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
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        await _notificationService.MarkAllAsReadAsync(userId.Value);
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