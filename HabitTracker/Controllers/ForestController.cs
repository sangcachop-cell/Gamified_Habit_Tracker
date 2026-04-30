using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class ForestController : Controller
    {
        private const string SESSION_KEY = "ForestSession";
        private const string COMBAT_KEY  = "ForestCombat";
        private const int    MIA_LIMIT   = 5000;

        // Loot screen grid sizes
        private const int BODY_COLS  = 4, BODY_ROWS  = 2;
        private const int POUCH_COLS = 4, POUCH_ROWS = 2;
        private const double WOOD_DROP_CHANCE = 0.10;

        private readonly AppDbContext _context;
        public ForestController(AppDbContext context) => _context = context;

        // ── Index ─────────────────────────────────────────────────────────────

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

        // ── Enter ─────────────────────────────────────────────────────────────

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
                PlayerX           = cx,
                PlayerY           = cy,
                SpawnId           = spawn.Id,
                RequiredExtractId = spawn.RequiredExtract,
                MovesSpent        = 0,
                IsActive          = true,
                StartedAt         = DateTime.UtcNow,
            };
            SaveSession(session);
            ClearCombat();
            return RedirectToAction(nameof(Map));
        }

        // ── Map ───────────────────────────────────────────────────────────────

        [HttpGet("Map")]
        public async Task<IActionResult> Map()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            if (session == null || !session.IsActive)
                return RedirectToAction(nameof(Index));

            if (session.PendingCombat)
                return RedirectToAction(nameof(Combat));

            // Equipped inventory for inline panel
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId.Value);
            var pocketItems = await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ContainerType == ItemCatalogue.BACKPACK).ToListAsync();
            var bpItems = user?.EquippedBackpackItem != null
                ? await _context.UserInventoryItems
                    .Where(i => i.UserId == userId && i.ContainerType == ItemCatalogue.EQUIPPED_BACKPACK).ToListAsync()
                : new List<UserInventoryItem>();
            var rigItems = user?.EquippedRigItem != null
                ? await _context.UserInventoryItems
                    .Where(i => i.UserId == userId && i.ContainerType == ItemCatalogue.EQUIPPED_RIG).ToListAsync()
                : new List<UserInventoryItem>();

            ViewBag.Session       = session;
            ViewBag.SessionJson   = JsonSerializer.Serialize(session);
            ViewBag.PocketItems   = BuildPlacedLoot(pocketItems);
            ViewBag.BackpackItems = BuildPlacedLoot(bpItems);
            ViewBag.RigItems      = BuildPlacedLoot(rigItems);
            ViewBag.HasBackpack   = user?.EquippedBackpackItem != null;
            ViewBag.HasRig        = user?.EquippedRigItem != null;
            return View();
        }

        // ── Move ──────────────────────────────────────────────────────────────

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

            var rng = new Random();

            if (req.Path is { Count: > 1 })
            {
                // Validate path
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

                var pathArr = req.Path.ToArray();

                // Cell-by-cell — stop at MIA limit or first combat event
                int stepsSoFar = 0;
                for (int si = 1; si < pathArr.Length; si++)
                {
                    var cell = pathArr[si];
                    stepsSoFar++;

                    // MIA check
                    if (session.MovesSpent + stepsSoFar > MIA_LIMIT)
                    {
                        session.PlayerX    = cell.X;
                        session.PlayerY    = cell.Y;
                        session.MovesSpent += stepsSoFar;
                        session.IsActive   = false;
                        session.AccumulatedLoot.Clear();
                        session.MonsterBody.Clear();
                        session.Pouch.Clear();
                        session.PendingPath = null;
                        SaveSession(session);
                        return Json(new { ok = true, mia = true,
                            x = cell.X, y = cell.Y, moves = session.MovesSpent });
                    }

                    // Combat event check — stop here
                    if (rng.NextDouble() < ForestMap.GetEventChance(cell.X, cell.Y))
                    {
                        session.PlayerX           = cell.X;
                        session.PlayerY           = cell.Y;
                        session.MovesSpent        += stepsSoFar;
                        session.PendingCombat      = true;
                        session.PendingMonsterTier = ForestMap.GetEventTier(cell.X, cell.Y);
                        // Store remaining path (from combat cell to original destination)
                        session.PendingPath = pathArr[si..]
                            .Select(c => new[] { c.X, c.Y }).ToList();
                        SaveSession(session);

                        return Json(new {
                            ok              = true,
                            combatTriggered = true,
                            monsterTier     = session.PendingMonsterTier,
                            x               = cell.X,
                            y               = cell.Y,
                            moves           = session.MovesSpent,
                            dist            = stepsSoFar
                        });
                    }
                }

                // No interruption — reached destination
                int dist = req.Path.Count - 1;
                session.PlayerX    = tx;
                session.PlayerY    = ty;
                session.MovesSpent += dist;
                SaveSession(session);

                var zone    = ForestMap.GetZone(tx, ty);
                var extract = ForestMap.GetExtract(tx, ty);
                bool canExtract = extract != null && extract.Id == session.RequiredExtractId;

                return Json(new {
                    ok = true,
                    x = session.PlayerX, y = session.PlayerY,
                    moves = session.MovesSpent, dist,
                    zoneName = zone?.Name, zoneDesc = zone?.Description,
                    canExtract, extractId = extract?.Id
                });
            }
            else
            {
                // Single-step (WASD)
                if (session.MovesSpent + 1 > MIA_LIMIT)
                {
                    session.PlayerX    = tx; session.PlayerY = ty;
                    session.MovesSpent += 1;
                    session.IsActive   = false;
                    session.AccumulatedLoot.Clear();
                    session.MonsterBody.Clear();
                    session.Pouch.Clear();
                    SaveSession(session);
                    return Json(new { ok = true, mia = true,
                        x = tx, y = ty, moves = session.MovesSpent });
                }

                if (rng.NextDouble() < ForestMap.GetEventChance(tx, ty))
                {
                    session.PlayerX           = tx; session.PlayerY = ty;
                    session.MovesSpent        += 1;
                    session.PendingCombat      = true;
                    session.PendingMonsterTier = ForestMap.GetEventTier(tx, ty);
                    session.PendingPath        = null; // no remaining path for single step
                    SaveSession(session);
                    return Json(new {
                        ok = true, combatTriggered = true,
                        monsterTier = session.PendingMonsterTier,
                        x = tx, y = ty, moves = session.MovesSpent, dist = 1
                    });
                }

                session.PlayerX = tx; session.PlayerY = ty;
                session.MovesSpent += 1;
                SaveSession(session);

                var zone    = ForestMap.GetZone(tx, ty);
                var extract = ForestMap.GetExtract(tx, ty);
                bool canExtract = extract != null && extract.Id == session.RequiredExtractId;

                return Json(new {
                    ok = true,
                    x = session.PlayerX, y = session.PlayerY,
                    moves = session.MovesSpent, dist = 1,
                    zoneName = zone?.Name, zoneDesc = zone?.Description,
                    canExtract, extractId = extract?.Id
                });
            }
        }

        // ── Combat GET ────────────────────────────────────────────────────────

        [HttpGet("Combat")]
        public async Task<IActionResult> Combat()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            if (session == null || !session.IsActive) return RedirectToAction(nameof(Index));
            if (!session.PendingCombat) return RedirectToAction(nameof(Map));

            var combat = LoadCombat();
            if (combat == null)
            {
                var user = await _context.Users.FindAsync(userId.Value);
                if (user == null) return RedirectToAction("Login", "Account");

                var monster = ForestMap.MakeMonster(session.PendingMonsterTier, user.Level);
                int playerHP = session.PlayerCurrentHP > 0 ? session.PlayerCurrentHP : user.HP;

                combat = new ForestCombatState
                {
                    MonsterTier              = monster.Tier,
                    MonsterName              = monster.Name,
                    MonsterIcon              = monster.Icon,
                    MonsterMaxHP             = monster.HP,
                    MonsterCurrentHP         = monster.HP,
                    MonsterAttack            = monster.Attack,
                    MonsterArmor             = monster.Armor,
                    MonsterSpeed             = monster.Speed,
                    PlayerMaxHP              = user.HP,
                    PlayerCurrentHP          = playerHP,
                    PlayerAttack             = user.AttackDamage,
                    PlayerArmor              = user.Armor,
                    PlayerSpeed              = user.Speed,
                    PlayerDamageReductionPct = user.ArmorDamageReductionPct,
                    Log = new List<string> { $"A {monster.Name} blocks your path!" }
                };
                SaveCombat(combat);

                session.PlayerCurrentHP = playerHP;
                SaveSession(session);
            }

            ViewBag.Combat  = combat;
            ViewBag.Session = session;
            return View();
        }

        // ── Combat/Attack POST ────────────────────────────────────────────────

        [HttpPost("Combat/Attack")]
        [ValidateAntiForgeryToken]
        public IActionResult CombatAttack()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            var combat  = LoadCombat();
            if (session == null || !session.IsActive || combat == null)
                return RedirectToAction(nameof(Map));

            // Aggressive action — guard chain resets
            combat.ConsecutiveDefends = 0;

            // Speed-based turn order (mirrors BattleController logic)
            bool playerFirst   = combat.PlayerSpeed >= combat.MonsterSpeed;
            bool playerBonus   = combat.PlayerSpeed >= (int)(combat.MonsterSpeed * 1.5);
            bool monsterBonus  = combat.MonsterSpeed >= (int)(combat.PlayerSpeed * 1.5);

            // Speed summary line
            string speedLine = playerFirst
                ? (playerBonus
                    ? $"Speed advantage! You strike twice. (SPD {combat.PlayerSpeed} > {combat.MonsterSpeed})"
                    : $"You act first. (SPD {combat.PlayerSpeed} vs {combat.MonsterSpeed})")
                : (monsterBonus
                    ? $"{combat.MonsterName} is faster and strikes twice! (SPD {combat.MonsterSpeed} > {combat.PlayerSpeed})"
                    : $"{combat.MonsterName} acts first. (SPD {combat.MonsterSpeed} vs {combat.PlayerSpeed})");
            combat.Log.Insert(0, speedLine);

            // Damage helpers (local functions)
            int PlayerDmg()   => Math.Max(1, combat.PlayerAttack - combat.MonsterArmor);
            int MonsterDmg()  => (int)Math.Max(1.0,
                Math.Max(0.0, combat.MonsterAttack - combat.PlayerArmor)
                * (1.0 - combat.PlayerDamageReductionPct / 100.0));

            bool monsterDead = false, playerDead = false;

            void DoPlayerHit(string prefix = "")
            {
                if (monsterDead) return;
                int d = PlayerDmg();
                combat.MonsterCurrentHP -= d;
                combat.Log.Insert(0, $"{prefix}You strike {combat.MonsterName} for {d} damage.");
                if (combat.MonsterCurrentHP <= 0)
                {
                    combat.MonsterCurrentHP = 0;
                    monsterDead = true;
                    combat.Log.Insert(0, $"{combat.MonsterName} is defeated!");
                }
            }

            void DoMonsterHit(string prefix = "")
            {
                if (playerDead) return;
                int d = MonsterDmg();
                combat.PlayerCurrentHP -= d;
                combat.Log.Insert(0, $"{prefix}{combat.MonsterName} hits you for {d} damage.");
                if (combat.PlayerCurrentHP <= 0)
                {
                    combat.PlayerCurrentHP = 0;
                    playerDead = true;
                    combat.Log.Insert(0, "You have been slain!");
                }
            }

            // Execute round in speed order
            if (playerFirst)
            {
                DoPlayerHit();
                if (playerBonus && !monsterDead) DoPlayerHit("Bonus strike — ");
                if (!monsterDead)
                {
                    DoMonsterHit();
                    if (monsterBonus && !playerDead) DoMonsterHit("Counter-strike — ");
                }
            }
            else
            {
                DoMonsterHit();
                if (monsterBonus && !playerDead) DoMonsterHit("Bonus strike — ");
                if (!playerDead)
                {
                    DoPlayerHit();
                    if (playerBonus && !monsterDead) DoPlayerHit("Counter-strike — ");
                }
            }

            // ── Win ───────────────────────────────────────────────────────────
            if (monsterDead)
            {
                session.PendingCombat   = false;
                session.PlayerCurrentHP = combat.PlayerCurrentHP;

                // Roll loot drop into monster body (10% wood for now)
                var lootRng = new Random();
                bool dropped = lootRng.NextDouble() < WOOD_DROP_CHANCE;
                if (dropped)
                {
                    session.MonsterBody.Clear();
                    session.MonsterBody.Add(new LootItem {
                        ItemId = "wood", GridX = 0, GridY = 0, Rotated = false
                    });
                }

                string lootMsg = combat.MonsterTier == "rare"
                    ? "Greater loot (placeholder)"
                    : (dropped ? "Found wood!" : "No loot.");
                session.AccumulatedLoot.Add(lootMsg);

                SaveSession(session);
                ClearCombat();

                TempData["ForestCombatWin"] = "true";
                TempData["ForestLootMsg"]   = lootMsg;

                // If loot dropped, show loot screen first; else go straight to map
                if (dropped) return RedirectToAction(nameof(Loot));

                if (session.PendingPath is { Count: > 1 })
                {
                    TempData["ForestPendingPath"] = JsonSerializer.Serialize(session.PendingPath);
                    session.PendingPath = null;
                    SaveSession(session);
                }
                return RedirectToAction(nameof(Map));
            }

            // ── Player dead ───────────────────────────────────────────────────
            if (playerDead)
            {
                session.IsActive = false;
                session.AccumulatedLoot.Clear();
                session.MonsterBody.Clear();
                session.Pouch.Clear();
                SaveSession(session);
                ClearCombat();
                return RedirectToAction(nameof(Dead));
            }

            // ── Continue fight ────────────────────────────────────────────────
            session.PlayerCurrentHP = combat.PlayerCurrentHP;
            SaveSession(session);
            SaveCombat(combat);
            return RedirectToAction(nameof(Combat));
        }

        // ── Combat/Defend POST ────────────────────────────────────────────────
        // Full block on success; consecutive defends raise failure chance (0/35/65/85%)
        [HttpPost("Combat/Defend")]
        [ValidateAntiForgeryToken]
        public IActionResult CombatDefend()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            var combat  = LoadCombat();
            if (session == null || !session.IsActive || combat == null)
                return RedirectToAction(nameof(Map));

            int[] failChances = { 0, 35, 65, 85 };
            int idx     = Math.Min(combat.ConsecutiveDefends, 3);
            int failPct = failChances[idx];

            var rng = new Random();
            bool defendFails = rng.Next(100) < failPct;

            string risk = idx == 0 ? "always blocks" : $"{failPct}% fail risk";
            combat.Log.Insert(0, $"You raise your guard. ({risk})");
            combat.ConsecutiveDefends++;

            bool monsterBonus = combat.MonsterSpeed >= (int)(combat.PlayerSpeed * 1.5);
            int  MonsterDmg() => (int)Math.Max(1.0,
                Math.Max(0.0, combat.MonsterAttack - combat.PlayerArmor)
                * (1.0 - combat.PlayerDamageReductionPct / 100.0));

            bool playerDead = false;

            if (defendFails)
            {
                combat.Log.Insert(0, "💥 Your guard breaks!");

                int d = MonsterDmg();
                combat.PlayerCurrentHP -= d;
                combat.Log.Insert(0, $"{combat.MonsterName} hits you for {d} damage.");
                if (combat.PlayerCurrentHP <= 0)
                {
                    combat.PlayerCurrentHP = 0;
                    playerDead = true;
                    combat.Log.Insert(0, "You have been slain!");
                }

                if (monsterBonus && !playerDead)
                {
                    d = MonsterDmg();
                    combat.PlayerCurrentHP -= d;
                    combat.Log.Insert(0, $"Bonus strike — hits for {d}.");
                    if (combat.PlayerCurrentHP <= 0)
                    {
                        combat.PlayerCurrentHP = 0;
                        playerDead = true;
                        combat.Log.Insert(0, "You have been slain!");
                    }
                }
            }
            else
            {
                combat.Log.Insert(0, $"🛡️ All damage from {combat.MonsterName} blocked!");
            }

            if (playerDead)
            {
                session.IsActive = false;
                session.AccumulatedLoot.Clear();
                session.MonsterBody.Clear();
                session.Pouch.Clear();
                SaveSession(session);
                ClearCombat();
                return RedirectToAction(nameof(Dead));
            }

            session.PlayerCurrentHP = combat.PlayerCurrentHP;
            SaveSession(session);
            SaveCombat(combat);
            return RedirectToAction(nameof(Combat));
        }

        // ── Combat/Flee POST ──────────────────────────────────────────────────
        // Requires "EscapeScroll" item — consumes one and ends combat without loot
        [HttpPost("Combat/Flee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CombatFlee()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            var combat  = LoadCombat();
            if (session == null || !session.IsActive || combat == null)
                return RedirectToAction(nameof(Map));

            var scroll = await _context.UserInventoryItems
                .FirstOrDefaultAsync(i => i.UserId == userId.Value && i.ItemId == "EscapeScroll");

            if (scroll == null)
            {
                combat.Log.Insert(0, "❌ You need an Escape Scroll to flee!");
                SaveCombat(combat);
                return RedirectToAction(nameof(Combat));
            }

            // Consume the scroll
            _context.UserInventoryItems.Remove(scroll);
            await _context.SaveChangesAsync();

            // End combat — back to map; no loot from this fight, drop pending path
            session.PlayerCurrentHP = combat.PlayerCurrentHP;
            session.PendingCombat   = false;
            session.PendingPath     = null;
            SaveSession(session);
            ClearCombat();

            TempData["ForestFleeSuccess"] = "true";
            return RedirectToAction(nameof(Map));
        }

        // ── Dead ──────────────────────────────────────────────────────────────

        [HttpGet("Dead")]
        public IActionResult Dead()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            ViewBag.MovesSpent = session?.MovesSpent ?? 0;

            if (session != null && session.IsActive)
            {
                session.IsActive = false;
                SaveSession(session);
            }
            return View();
        }

        // ── Extract ───────────────────────────────────────────────────────────

        [HttpPost("Extract")]
        [ValidateAntiForgeryToken]
        public IActionResult Extract()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            if (session == null || !session.IsActive) return RedirectToAction(nameof(Index));

            var extract = ForestMap.GetExtract(session.PlayerX, session.PlayerY);
            if (extract == null || extract.Id != session.RequiredExtractId)
            {
                TempData["ForestError"] = "Not at the required extract point.";
                return RedirectToAction(nameof(Map));
            }

            session.IsActive = false;
            session.MonsterBody.Clear();
            SaveSession(session);
            ClearCombat();

            TempData["ExtractSpawn"]    = session.SpawnId;
            TempData["ExtractId"]       = extract.Id;
            TempData["ExtractMoves"]    = session.MovesSpent;
            TempData["ExtractDuration"] = (int)(DateTime.UtcNow - session.StartedAt).TotalSeconds;
            TempData["ForestOutcome"]   = "success";
            TempData["ForestLoot"]      = JsonSerializer.Serialize(session.AccumulatedLoot);

            return RedirectToAction(nameof(Result));
        }

        // ── Result ────────────────────────────────────────────────────────────

        [HttpGet("Result")]
        public IActionResult Result()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");
            return View();
        }

        // ── Loot screen ───────────────────────────────────────────────────────

        [HttpGet("Loot")]
        public async Task<IActionResult> Loot()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            if (session == null || !session.IsActive) return RedirectToAction(nameof(Index));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId.Value);

            var pocketItems = await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ContainerType == ItemCatalogue.BACKPACK)
                .ToListAsync();
            var bpItems = user?.EquippedBackpackItem != null
                ? await _context.UserInventoryItems
                    .Where(i => i.UserId == userId && i.ContainerType == ItemCatalogue.EQUIPPED_BACKPACK)
                    .ToListAsync()
                : new List<UserInventoryItem>();
            var rigItems = user?.EquippedRigItem != null
                ? await _context.UserInventoryItems
                    .Where(i => i.UserId == userId && i.ContainerType == ItemCatalogue.EQUIPPED_RIG)
                    .ToListAsync()
                : new List<UserInventoryItem>();

            ViewBag.Session       = session;
            ViewBag.BodyCols      = BODY_COLS;
            ViewBag.BodyRows      = BODY_ROWS;
            ViewBag.PocketItems   = BuildPlacedLoot(pocketItems);
            ViewBag.BackpackItems = BuildPlacedLoot(bpItems);
            ViewBag.RigItems      = BuildPlacedLoot(rigItems);
            ViewBag.HasBackpack   = user?.EquippedBackpackItem != null;
            ViewBag.HasRig        = user?.EquippedRigItem != null;
            return View();
        }

        public class LootPickupRequest
        {
            public int    Index   { get; set; }
            public string To      { get; set; } = ""; // "Backpack" | "EquippedBackpack" | "EquippedRig"
            public int    GridX   { get; set; }
            public int    GridY   { get; set; }
            public bool   Rotated { get; set; }
        }

        [HttpPost("Loot/Pickup")]
        public async Task<IActionResult> LootPickup([FromBody] LootPickupRequest req)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { ok = false, error = "Not logged in" });

            var session = LoadSession();
            if (session == null || !session.IsActive)
                return Json(new { ok = false, error = "No active session" });

            if (req.Index < 0 || req.Index >= session.MonsterBody.Count)
                return Json(new { ok = false, error = "Bad index" });

            var loot = session.MonsterBody[req.Index];
            if (!ItemCatalogue.Items.TryGetValue(loot.ItemId, out var def))
                return Json(new { ok = false, error = "Unknown item" });

            string container = req.To;
            if (container != ItemCatalogue.BACKPACK &&
                container != ItemCatalogue.EQUIPPED_BACKPACK &&
                container != ItemCatalogue.EQUIPPED_RIG)
                return Json(new { ok = false, error = "Invalid container" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (container == ItemCatalogue.EQUIPPED_BACKPACK && user?.EquippedBackpackItem == null)
                return Json(new { ok = false, error = "No backpack equipped" });
            if (container == ItemCatalogue.EQUIPPED_RIG && user?.EquippedRigItem == null)
                return Json(new { ok = false, error = "No rig equipped" });

            int w = req.Rotated ? def.Height : def.Width;
            int h = req.Rotated ? def.Width  : def.Height;

            var sc = ItemCatalogue.SlotConstraint(container);
            if (sc != null && (w != sc.Value.W || h != sc.Value.H))
                return Json(new { ok = false, error = "Item shape does not fit container slot" });

            var (cols, rows) = ItemCatalogue.ContainerSize(container);
            if (req.GridX < 0 || req.GridY < 0 ||
                req.GridX + w > cols || req.GridY + h > rows)
                return Json(new { ok = false, error = "Out of bounds" });

            var existing = await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ContainerType == container)
                .ToListAsync();
            foreach (var o in existing)
            {
                if (!ItemCatalogue.Items.TryGetValue(o.ItemId, out var od)) continue;
                int ow = o.IsRotated ? od.Height : od.Width;
                int oh = o.IsRotated ? od.Width  : od.Height;
                bool noOv = req.GridX >= o.GridX + ow || req.GridX + w <= o.GridX ||
                            req.GridY >= o.GridY + oh || req.GridY + h <= o.GridY;
                if (!noOv) return Json(new { ok = false, error = "Overlap" });
            }

            _context.UserInventoryItems.Add(new UserInventoryItem
            {
                UserId        = userId.Value,
                ItemId        = loot.ItemId,
                ContainerType = container,
                GridX         = req.GridX,
                GridY         = req.GridY,
                IsRotated     = req.Rotated,
                AcquiredAt    = DateTime.Now
            });

            session.MonsterBody.RemoveAt(req.Index);
            SaveSession(session);
            await _context.SaveChangesAsync();

            return Json(new { ok = true });
        }

        [HttpPost("Loot/Close")]
        [ValidateAntiForgeryToken]
        public IActionResult LootClose()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var session = LoadSession();
            if (session == null) return RedirectToAction(nameof(Index));

            // Items left in monster body are abandoned
            session.MonsterBody.Clear();

            // Resume pending path if any
            if (session.PendingPath is { Count: > 1 })
            {
                TempData["ForestPendingPath"] = JsonSerializer.Serialize(session.PendingPath);
                session.PendingPath = null;
            }

            SaveSession(session);
            return RedirectToAction(nameof(Map));
        }

        private static List<PlacedItem> BuildPlacedLoot(IEnumerable<UserInventoryItem> items)
        {
            var result = new List<PlacedItem>();
            foreach (var item in items)
            {
                if (!ItemCatalogue.Items.TryGetValue(item.ItemId, out var def)) continue;
                int w = item.IsRotated ? def.Height : def.Width;
                int h = item.IsRotated ? def.Width  : def.Height;
                result.Add(new PlacedItem(
                    item.Id, item.ItemId, def.Name, def.Icon,
                    def.Description, def.Category, def.TileColor,
                    item.GridX, item.GridY, w, h, item.IsRotated,
                    ItemCatalogue.CanRotate(item.ItemId), item.ContainerType));
            }
            return result;
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

        private ForestCombatState? LoadCombat()
        {
            var json = HttpContext.Session.GetString(COMBAT_KEY);
            if (string.IsNullOrEmpty(json)) return null;
            try { return JsonSerializer.Deserialize<ForestCombatState>(json); }
            catch { return null; }
        }

        private void SaveCombat(ForestCombatState c) =>
            HttpContext.Session.SetString(COMBAT_KEY, JsonSerializer.Serialize(c));

        private void ClearCombat() =>
            HttpContext.Session.Remove(COMBAT_KEY);

        private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }

    public class CellPoint  { public int X { get; set; } public int Y { get; set; } }
    public class MoveRequest { public int X { get; set; } public int Y { get; set; } public List<CellPoint>? Path { get; set; } }
}
