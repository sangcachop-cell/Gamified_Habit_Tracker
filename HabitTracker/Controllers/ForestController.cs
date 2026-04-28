using HabitTracker.Constants;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class ForestController : Controller
    {
        private const string SESSION_KEY = "ForestSession";

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            ViewBag.HasActiveSession = session?.IsActive ?? false;
            return View();
        }

        [HttpPost("Enter")]
        [ValidateAntiForgeryToken]
        public IActionResult Enter()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var spawn = ForestMap.RandomSpawn();
            var (cx, cy) = ForestMap.SpawnCenter(spawn);

            var session = new ForestSession
            {
                PlayerX            = cx,
                PlayerY            = cy,
                SpawnId            = spawn.Id,
                RequiredExtractId  = spawn.RequiredExtract,
                MovesSpent         = 0,
                IsActive           = true,
                StartedAt          = DateTime.UtcNow,
            };
            SaveSession(session);
            return RedirectToAction(nameof(Map));
        }

        [HttpGet("Map")]
        public IActionResult Map()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            if (session == null || !session.IsActive)
                return RedirectToAction(nameof(Index));

            ViewBag.Session    = session;
            ViewBag.SessionJson = JsonSerializer.Serialize(session);
            return View();
        }

        [HttpPost("Move")]
        public IActionResult Move([FromBody] MoveRequest req)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { ok = false, error = "Not logged in" });

            var session = LoadSession();
            if (session == null || !session.IsActive)
                return Json(new { ok = false, error = "No active session" });

            int tx = req.X, ty = req.Y;

            if (ForestMap.IsWater(tx, ty))
                return Json(new { ok = false, error = "Cannot enter water" });
            if (tx < 0 || ty < 0 || tx >= ForestMap.WIDTH || ty >= ForestMap.HEIGHT)
                return Json(new { ok = false, error = "Out of bounds" });

            int dist = ForestMap.Distance(session.PlayerX, session.PlayerY, tx, ty);
            session.PlayerX    = tx;
            session.PlayerY    = ty;
            session.MovesSpent += dist;
            SaveSession(session);

            var zone    = ForestMap.GetZone(tx, ty);
            var extract = ForestMap.GetExtract(tx, ty);
            bool canExtract = extract != null && extract.Id == session.RequiredExtractId;

            return Json(new
            {
                ok         = true,
                x          = session.PlayerX,
                y          = session.PlayerY,
                moves      = session.MovesSpent,
                dist,
                zoneName   = zone?.Name,
                zoneDesc   = zone?.Description,
                canExtract,
                extractId  = extract?.Id
            });
        }

        [HttpPost("Extract")]
        [ValidateAntiForgeryToken]
        public IActionResult Extract()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            if (session == null || !session.IsActive)
                return RedirectToAction(nameof(Index));

            var extract = ForestMap.GetExtract(session.PlayerX, session.PlayerY);
            if (extract == null || extract.Id != session.RequiredExtractId)
            {
                TempData["ForestError"] = "Not at the required extract point.";
                return RedirectToAction(nameof(Map));
            }

            session.IsActive = false;
            SaveSession(session);

            TempData["ExtractSpawn"]    = session.SpawnId;
            TempData["ExtractId"]       = extract.Id;
            TempData["ExtractMoves"]    = session.MovesSpent;
            TempData["ExtractDuration"] = (int)(DateTime.UtcNow - session.StartedAt).TotalSeconds;

            return RedirectToAction(nameof(Result));
        }

        [HttpGet("Result")]
        public IActionResult Result()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");
            return View();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private ForestSession? LoadSession()
        {
            var json = HttpContext.Session.GetString(SESSION_KEY);
            if (string.IsNullOrEmpty(json)) return null;
            try { return JsonSerializer.Deserialize<ForestSession>(json); }
            catch { return null; }
        }

        private void SaveSession(ForestSession s) =>
            HttpContext.Session.SetString(SESSION_KEY, JsonSerializer.Serialize(s));

        private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }

    public class MoveRequest { public int X { get; set; } public int Y { get; set; } }
}
