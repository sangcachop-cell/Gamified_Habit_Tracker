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

            if (tx < 0 || ty < 0 || tx >= ForestMap.WIDTH || ty >= ForestMap.HEIGHT)
                return Json(new { ok = false, error = "Out of bounds" });
            if (ForestMap.IsWater(tx, ty))
                return Json(new { ok = false, error = "Cannot enter water" });

            int dist;
            var events = new List<object>();
            var rng    = new Random();

            if (req.Path is { Count: > 1 })
            {
                // Validate: each step adjacent (Chebyshev ≤ 1), no water
                for (int i = 1; i < req.Path.Count; i++)
                {
                    var prev = req.Path[i - 1];
                    var curr = req.Path[i];
                    if (Math.Abs(curr.X - prev.X) > 1 || Math.Abs(curr.Y - prev.Y) > 1)
                        return Json(new { ok = false, error = "Invalid path: non-adjacent step" });
                    if (ForestMap.IsWater(curr.X, curr.Y))
                        return Json(new { ok = false, error = "Invalid path: enters water" });
                }
                var last = req.Path[^1];
                if (last.X != tx || last.Y != ty)
                    return Json(new { ok = false, error = "Path end mismatch" });

                dist = req.Path.Count - 1;

                foreach (var cell in req.Path.Skip(1))
                {
                    if (rng.NextDouble() < ForestMap.GetEventChance(cell.X, cell.Y))
                    {
                        var z = ForestMap.GetZone(cell.X, cell.Y);
                        events.Add(new {
                            x    = cell.X,
                            y    = cell.Y,
                            tier = ForestMap.GetEventTier(cell.X, cell.Y),
                            zone = z?.Name
                        });
                    }
                }
            }
            else
            {
                // Single-step move (WASD)
                dist = 1;
                if (rng.NextDouble() < ForestMap.GetEventChance(tx, ty))
                {
                    var z = ForestMap.GetZone(tx, ty);
                    events.Add(new {
                        x    = tx,
                        y    = ty,
                        tier = ForestMap.GetEventTier(tx, ty),
                        zone = z?.Name
                    });
                }
            }

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
                extractId  = extract?.Id,
                events
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

    public class CellPoint  { public int X { get; set; } public int Y { get; set; } }
    public class MoveRequest { public int X { get; set; } public int Y { get; set; } public List<CellPoint>? Path { get; set; } }
}
