using HabitTracker.Constants;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;

public class NotificationsController : Controller
{
    private readonly INotificationService _notifications;

    public NotificationsController(INotificationService notifications)
    {
        _notifications = notifications;
    }

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
        if (userId == null) return RedirectToAction("Login", "Account");

        var items = await _notifications.GetNotificationsAsync(userId.Value, 100);
        return View(items);
    }
}
