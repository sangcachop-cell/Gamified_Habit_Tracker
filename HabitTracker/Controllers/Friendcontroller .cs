using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    public class FriendController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FriendController> _logger;

        public FriendController(AppDbContext context, ILogger<FriendController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /Friend — trang bạn bè + tìm kiếm
        public async Task<IActionResult> Index(string? q)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            // Danh sách bạn đã accepted
            var friends = await _context.Friendships
                .Where(f => f.Status == "Accepted" &&
                            (f.RequesterId == userId || f.ReceiverId == userId))
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .ToListAsync();

            // Lời mời đang chờ (tôi nhận)
            var pendingReceived = await _context.Friendships
                .Where(f => f.Status == "Pending" && f.ReceiverId == userId)
                .Include(f => f.Requester)
                .ToListAsync();

            // Lời mời tôi đã gửi
            var pendingSent = await _context.Friendships
                .Where(f => f.Status == "Pending" && f.RequesterId == userId)
                .Include(f => f.Receiver)
                .ToListAsync();

            // Tìm kiếm user
            List<User> searchResults = new();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var friendIds = friends
                    .Select(f => f.RequesterId == userId ? f.ReceiverId : f.RequesterId)
                    .ToHashSet();
                var pendingIds = pendingReceived.Select(f => f.RequesterId)
                    .Concat(pendingSent.Select(f => f.ReceiverId))
                    .ToHashSet();

                searchResults = await _context.Users
                    .Where(u => !u.IsAdmin && u.Id != userId &&
                                (u.Username.Contains(q) || u.Email.Contains(q)))
                    .Take(20)
                    .ToListAsync();

                // Tag trạng thái cho từng kết quả
                ViewBag.FriendIds = friendIds;
                ViewBag.PendingIds = pendingIds;
            }

            ViewBag.Friends = friends;
            ViewBag.PendingReceived = pendingReceived;
            ViewBag.PendingSent = pendingSent;
            ViewBag.SearchResults = searchResults;
            ViewBag.SearchQuery = q;
            ViewBag.CurrentUserId = userId;

            return View();
        }

        // GET /Friend/ViewProfile/{id} — xem profile người khác
        public async Task<IActionResult> ViewProfile(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (id == userId) return RedirectToAction("Profile", "Account");

            var target = await _context.Users
                .Include(u => u.UserBadges).ThenInclude((UserBadge ub) => ub.Badge)
                .Include(u => u.UserQuests)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsAdmin);

            if (target == null) return NotFound();

            // Friendship status
            var relation = await _context.Friendships
                .FirstOrDefaultAsync(f =>
                    (f.RequesterId == userId && f.ReceiverId == id) ||
                    (f.RequesterId == id && f.ReceiverId == userId));

            ViewBag.Relation = relation;
            ViewBag.CurrentUserId = userId;

            // Equipped gear thumbnails (weapon, armor, head, shield)
            var equippedKeys = new[] { target.EquippedWeapon, target.EquippedArmor, target.EquippedHead, target.EquippedShield }
                .Where(k => !string.IsNullOrEmpty(k))
                .Cast<string>()
                .ToList();

            var gearItems = equippedKeys.Count > 0
                ? await _context.GearItems.AsNoTracking()
                    .Where(g => equippedKeys.Contains(g.Key))
                    .ToListAsync()
                : new();

            ViewBag.EquippedGear = gearItems;

            // Block status (does viewer block target?)
            var isBlocked  = await _context.UserBlocks.AsNoTracking()
                .AnyAsync(b => b.BlockerId == userId && b.BlockedId == id);
            var isBlockedBy = await _context.UserBlocks.AsNoTracking()
                .AnyAsync(b => b.BlockerId == id && b.BlockedId == userId);

            ViewBag.IsBlocked   = isBlocked;
            ViewBag.IsBlockedBy = isBlockedBy;

            return View(target);
        }

        // POST /Friend/SendRequest
        [HttpPost]
        public async Task<IActionResult> SendRequest(int receiverId)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { ok = false, msg = "Chưa đăng nhập" });

            if (userId == receiverId) return Json(new { ok = false, msg = "Không thể tự kết bạn" });

            var exists = await _context.Friendships.AnyAsync(f =>
                (f.RequesterId == userId && f.ReceiverId == receiverId) ||
                (f.RequesterId == receiverId && f.ReceiverId == userId));

            if (exists) return Json(new { ok = false, msg = "Đã tồn tại quan hệ" });

            _context.Friendships.Add(new Friendship
            {
                RequesterId = userId.Value,
                ReceiverId = receiverId,
                Status = "Pending"
            });
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Friend request {userId} -> {receiverId}");

            return Json(new { ok = true, msg = "Đã gửi lời mời kết bạn" });
        }

        // POST /Friend/Accept
        [HttpPost]
        public async Task<IActionResult> Accept(int friendshipId)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var f = await _context.Friendships
                .FirstOrDefaultAsync(x => x.Id == friendshipId && x.ReceiverId == userId);

            if (f == null) return NotFound();

            f.Status = "Accepted";
            f.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ Đã chấp nhận lời mời kết bạn!";
            return RedirectToAction(nameof(Index));
        }

        // POST /Friend/Reject
        [HttpPost]
        public async Task<IActionResult> Reject(int friendshipId)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var f = await _context.Friendships
                .FirstOrDefaultAsync(x => x.Id == friendshipId &&
                                          (x.ReceiverId == userId || x.RequesterId == userId));
            if (f == null) return NotFound();

            _context.Friendships.Remove(f);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xoá lời mời.";
            return RedirectToAction(nameof(Index));
        }

        // POST /Friend/Unfriend
        [HttpPost]
        public async Task<IActionResult> Unfriend(int friendshipId)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var f = await _context.Friendships
                .FirstOrDefaultAsync(x => x.Id == friendshipId &&
                                          (x.RequesterId == userId || x.ReceiverId == userId));
            if (f == null) return NotFound();

            _context.Friendships.Remove(f);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã huỷ kết bạn.";
            return RedirectToAction(nameof(Index));
        }

        // GET /Friend/FindByUsername?username=X — used by message compose
        [HttpGet]
        public async Task<IActionResult> FindByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return NotFound();

            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Username.ToLower() == username.ToLower() && !u.IsAdmin)
                .Select(u => new { u.Id, u.Username, u.Avatar })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();
            return Json(user);
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}