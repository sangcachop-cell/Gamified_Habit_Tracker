using HabitTracker.Constants;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;

namespace HabitTracker.Controllers;

public class GuildController : Controller
{
    private readonly IGuildService _guilds;
    private readonly AppDbContext _context;

    public GuildController(IGuildService guilds, AppDbContext context)
    {
        _guilds = guilds;
        _context = context;
    }

    private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);

    // GET /Guild — discovery + my guilds
    public async Task<IActionResult> Index(string? search, string tab = "discover")
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var publicGuilds = await _guilds.GetPublicGuildsAsync(search);
        var myGuilds = await _guilds.GetMyGuildsAsync(userId.Value);
        var pendingInvites = await _guilds.GetPendingInvitesAsync(userId.Value);

        var memberCounts = new Dictionary<int, int>();
        foreach (var g in publicGuilds)
        {
            var count = await _context.GuildMembers.CountAsync(m => m.GuildId == g.Id);
            memberCounts[g.Id] = count;
        }

        var myGuildIds = myGuilds.Select(g => g.Guild.Id).ToHashSet();

        var model = new GuildIndexViewModel
        {
            PublicGuilds = publicGuilds.Select(g => new GuildCardModel
            {
                Guild = g,
                MemberCount = memberCounts.TryGetValue(g.Id, out var c) ? c : 0,
                IsMember = myGuildIds.Contains(g.Id),
                MyRole = myGuilds.FirstOrDefault(mg => mg.Guild.Id == g.Id)?.MyRole
            }).ToList(),
            MyGuilds = myGuilds,
            PendingInvites = pendingInvites,
            Search = search,
            ActiveTab = tab
        };

        return View(model);
    }

    // GET /Guild/View/{id}
    public async Task<IActionResult> View(int id, int page = 0)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var vm = await _guilds.GetGuildViewAsync(id, userId.Value, page);
        if (vm == null) return NotFound();

        // Non-member can view public guild but cannot chat
        if (!vm.IsMember && vm.Guild.Privacy == "private")
        {
            TempData["ToastError"] = "This guild is private.";
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET /Guild/Create
    public IActionResult Create()
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");
        return View();
    }

    // POST /Guild/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string name, string? description, string? summary, string privacy)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var (success, error, guild) = await _guilds.CreateAsync(userId.Value, name, description, summary, privacy);
        if (!success)
        {
            TempData["ToastError"] = error;
            return View();
        }

        TempData["ToastSuccess"] = $"Guild \"{guild!.Name}\" created!";
        return RedirectToAction(nameof(View), new { id = guild.Id });
    }

    // POST /Guild/Join/{id}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Join(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _guilds.JoinPublicAsync(userId.Value, id);
        return Json(new { success, error });
    }

    // POST /Guild/Leave/{id}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Leave(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _guilds.LeaveAsync(userId.Value, id);
        return Json(new { success, error });
    }

    // GET /Guild/SearchInvitable?guildId=X&q=...
    [HttpGet]
    public async Task<IActionResult> SearchInvitable(int guildId, string q)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new List<object>());
        if (string.IsNullOrWhiteSpace(q)) return Json(new List<object>());

        var search = q.ToLower();
        var memberIds = await _context.GuildMembers
            .Where(m => m.GuildId == guildId)
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

    // POST /Guild/Invite/{id}  — body: targetUserId
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Invite(int id, int targetUserId)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _guilds.InviteAsync(userId.Value, id, targetUserId);
        return Json(new { success, error });
    }

    // POST /Guild/AcceptInvite/{inviteId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptInvite(int inviteId)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        var (success, error) = await _guilds.AcceptInviteAsync(userId.Value, inviteId);
        TempData[success ? "ToastSuccess" : "ToastError"] = success ? "Joined guild!" : error;
        return RedirectToAction(nameof(Index), new { tab = "my-guilds" });
    }

    // POST /Guild/DeclineInvite/{inviteId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclineInvite(int inviteId)
    {
        var userId = GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");

        await _guilds.DeclineInviteAsync(userId.Value, inviteId);
        return RedirectToAction(nameof(Index));
    }

    // POST /Guild/Kick/{guildId}/{memberId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Kick(int guildId, int memberId)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _guilds.KickMemberAsync(userId.Value, guildId, memberId);
        return Json(new { success, error });
    }

    // POST /Guild/Promote/{guildId}/{memberId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Promote(int guildId, int memberId)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _guilds.PromoteManagerAsync(userId.Value, guildId, memberId);
        return Json(new { success, error });
    }

    // POST /Guild/Demote/{guildId}/{memberId}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Demote(int guildId, int memberId)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _guilds.DemoteManagerAsync(userId.Value, guildId, memberId);
        return Json(new { success, error });
    }

    // POST /Guild/SendMessage/{id}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(int id, string body)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error, msg) = await _guilds.SendMessageAsync(userId.Value, id, body ?? "");
        if (!success) return Json(new { success = false, error });

        var author = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId.Value);
        var avatarSrc = string.IsNullOrEmpty(author?.Avatar) || author.Avatar == "default.png"
            ? "/images/default.png"
            : "/uploads/" + author.Avatar;

        var renderedBody = await _guilds.RenderBodyAsync(msg!.Body);

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

    // GET /Guild/Messages/{id}?page=N
    [HttpGet]
    public async Task<IActionResult> Messages(int id, int page = 0)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false });

        var entries = await _guilds.GetMessagesAsync(id, userId.Value, page);
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
                likeCount = e.LikeCount
            })
        });
    }

    // POST /Guild/LikeMessage/{id}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LikeMessage(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (liked, count) = await _guilds.ToggleLikeAsync(userId.Value, id);
        return Json(new { success = true, liked, count });
    }

    // POST /Guild/DeleteMessage/{id}
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, error = "Not logged in." });

        var (success, error) = await _guilds.DeleteMessageAsync(userId.Value, id);
        return Json(new { success, error });
    }
}