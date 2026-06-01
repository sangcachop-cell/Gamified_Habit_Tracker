using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers;

public class MessageController : Controller
{
    private readonly IMessageService _messages;
    private readonly AppDbContext _context;

    public MessageController(IMessageService messages, AppDbContext context)
    {
        _messages = messages;
        _context = context;
    }

    private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);

    // GET /Message  — inbox with optional open conversation (?otherId=X)
    public async Task<IActionResult> Index(int? otherId, int page = 0)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var conversations = await _messages.GetConversationsAsync(userId.Value);
        var totalUnread = await _messages.GetUnreadCountAsync(userId.Value);

        ConversationViewModel? active = null;

        if (otherId.HasValue)
        {
            var otherUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == otherId.Value);
            if (otherUser != null)
            {
                await _messages.MarkReadAsync(userId.Value, otherId.Value);
                var entries = await _messages.GetConversationAsync(userId.Value, otherId.Value, page);
                var isBlocker = await _messages.IsBlockerAsync(userId.Value, otherId.Value);
                var isBlockedBy = await _messages.IsBlockerAsync(otherId.Value, userId.Value);

                active = new ConversationViewModel
                {
                    OtherUser = otherUser,
                    Messages = entries,
                    IsBlocked = isBlocker,
                    IsBlockedBy = isBlockedBy,
                    Page = page,
                    HasMore = entries.Count == 20
                };
            }
        }

        var model = new InboxViewModel
        {
            Conversations = conversations,
            TotalUnread = totalUnread,
            ActiveConversationUserId = otherId,
            ActiveConversation = active
        };

        return View(model);
    }

    // GET /Message/Conversation/{id} — redirect to Index with otherId
    public IActionResult Conversation(int id)
    {
        return RedirectToAction(nameof(Index), new { otherId = id });
    }

    // GET /Message/UnreadCount
    [HttpGet]
    public async Task<IActionResult> UnreadCount()
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { count = 0 });
        var count = await _messages.GetUnreadCountAsync(userId.Value);
        return Json(new { count });
    }

    // POST /Message/Send
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(int receiverId, string body)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error, msg) = await _messages.SendAsync(userId.Value, receiverId, body);
        if (!success) return Json(new { success = false, error });

        return Json(new
        {
            success = true,
            messageId = msg!.Id,
            body = msg.Body,
            sentAt = msg.SentAt.ToString("g")
        });
    }

    // POST /Message/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _messages.DeleteMessageAsync(userId.Value, id);
        return Json(new { success, error });
    }

    // POST /Message/Like/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Like(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (liked, count) = await _messages.ToggleLikeAsync(userId.Value, id);
        return Json(new { success = true, liked, count });
    }

    // POST /Message/Block/{userId}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Block(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        if (userId.Value == id) return Json(new { success = false, error = "Cannot block yourself." });

        var blocked = await _messages.ToggleBlockAsync(userId.Value, id);
        return Json(new { success = true, blocked });
    }

    // POST /Message/Report
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Report(int reportedUserId, int? reportedMessageId, string reason)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        (bool success, string? error) result;

        if (reportedMessageId.HasValue)
            result = await _messages.ReportMessageAsync(userId.Value, reportedMessageId.Value, reason);
        else
            result = await _messages.ReportUserAsync(userId.Value, reportedUserId, reason);

        return Json(new { success = result.success, error = result.error });
    }
}