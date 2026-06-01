using HabitTracker.Constants;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;

namespace HabitTracker.Controllers;

public class PartyController : Controller
{
    private readonly IPartyService _party;
    private readonly AppDbContext _context;

    public PartyController(IPartyService party, AppDbContext context)
    {
        _party = party;
        _context = context;
    }

    private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);

    // GET /Party
    public async Task<IActionResult> Index(int page = 0)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var myParty = await _party.GetMyPartyAsync(userId.Value);

        if (myParty == null)
        {
            // Show "no party" view with pending invites
            var pendingInvites = await _party.GetPendingInvitesAsync(userId.Value);
            return View(new HabitTracker.Models.ViewModels.PartyViewModel
            {
                Party = null,
                PendingInvites = pendingInvites
            });
        }

        var vm = await _party.GetPartyViewAsync(myParty.Id, userId.Value, page);
        return View(vm);
    }

    // POST /Party/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string name)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var (success, error, party) = await _party.CreateAsync(userId.Value, name ?? "");
        if (!success)
        {
            TempData["ToastError"] = error;
            return RedirectToAction(nameof(Index));
        }

        TempData["ToastSuccess"] = $"Party \"{party!.Name}\" created!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Party/SearchInvitable?q=...
    [HttpGet]
    public async Task<IActionResult> SearchInvitable(string q)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new List<object>());
        if (string.IsNullOrWhiteSpace(q)) return Json(new List<object>());

        var search = q.ToLower();
        var memberIds = await _context.PartyMembers
            .Select(m => m.UserId)
            .ToListAsync();

        var users = await _context.Users
            .Where(u => !u.IsAdmin && !memberIds.Contains(u.Id)
                && (u.Username.ToLower().Contains(search) || u.Email.ToLower().Contains(search)))
            .Select(u => new { u.Id, u.Username, u.Avatar, u.Level })
            .Take(8)
            .ToListAsync();

        return Json(users);
    }

    // POST /Party/Invite  — body: targetUserId
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Invite(int targetUserId)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _party.InviteAsync(userId.Value, targetUserId);
        return Json(new { success, error });
    }

    // POST /Party/AcceptInvite/{inviteId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptInvite(int inviteId)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var (success, error) = await _party.AcceptInviteAsync(userId.Value, inviteId);
        TempData[success ? "ToastSuccess" : "ToastError"] = success ? "Joined party!" : error;
        return RedirectToAction(nameof(Index));
    }

    // POST /Party/DeclineInvite/{inviteId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclineInvite(int inviteId)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        await _party.DeclineInviteAsync(userId.Value, inviteId);
        return RedirectToAction(nameof(Index));
    }

    // POST /Party/Leave
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Leave()
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _party.LeaveAsync(userId.Value);
        return Json(new { success, error });
    }

    // POST /Party/Kick/{memberId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Kick(int memberId)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _party.KickMemberAsync(userId.Value, memberId);
        return Json(new { success, error });
    }

    // POST /Party/SendMessage
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(string body)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error, msg) = await _party.SendMessageAsync(userId.Value, body ?? "");
        if (!success) return Json(new { success = false, error });

        var author = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId.Value);
        var avatarSrc = string.IsNullOrEmpty(author?.Avatar) || author.Avatar == "default.png"
            ? "/images/default.png"
            : "/uploads/" + author.Avatar;

        var renderedBody = await _party.RenderBodyAsync(msg!.Body);

        return Json(new
        {
            success = true,
            messageId = msg.Id,
            body = renderedBody,
            sentAt = msg.SentAt.ToLocalTime().ToString("g"),
            authorId = userId.Value,
            authorName = author?.Username ?? "Unknown",
            authorAvatar = avatarSrc
        });
    }

    // GET /Party/Messages?page=N
    [HttpGet]
    public async Task<IActionResult> Messages(int page = 0)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false });

        var myParty = await _party.GetMyPartyAsync(userId.Value);
        if (myParty == null) return Json(new { success = false, error = "Not in a party." });

        var entries = await _party.GetMessagesAsync(myParty.Id, userId.Value, page);
        return Json(new
        {
            success = true,
            hasMore = entries.Count == 20,
            messages = entries.Select(e => new
            {
                id = e.Message.Id,
                body = e.RenderedBody,
                sentAt = e.Message.SentAt.ToLocalTime().ToString("g"),
                authorId = e.Message.AuthorId,
                authorName = e.Message.Author?.Username ?? "?",
                authorAvatar = string.IsNullOrEmpty(e.Message.Author?.Avatar) || e.Message.Author.Avatar == "default.png"
                               ? "/images/default.png" : "/uploads/" + e.Message.Author.Avatar,
                likedByMe = e.LikedByMe,
                likeCount = e.LikeCount,
                isSystem = e.IsSystem
            })
        });
    }

    // POST /Party/LikeMessage/{id}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LikeMessage(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (liked, count) = await _party.ToggleLikeAsync(userId.Value, id);
        return Json(new { success = true, liked, count });
    }

    // POST /Party/DeleteMessage/{id}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _party.DeleteMessageAsync(userId.Value, id);
        return Json(new { success, error });
    }
}