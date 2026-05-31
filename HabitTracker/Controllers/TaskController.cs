using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskService      _taskService;
        private readonly ISpellService     _spellService;
        private readonly ICharacterService _characterService;
        private readonly AppDbContext      _context;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskService taskService,
            ISpellService spellService,
            ICharacterService characterService,
            AppDbContext context,
            ILogger<TaskController> logger)
        {
            _taskService      = taskService;
            _spellService     = spellService;
            _characterService = characterService;
            _context          = context;
            _logger           = logger;
        }

        // ===== BOARD =====

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            await _taskService.RunCronAsync(userId.Value);

            var board = await _taskService.GetUserTasksAsync(userId.Value);

            // Load user + skills for skills bar
            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user != null)
            {
                board.User          = user;
                board.EffectiveStats = await _characterService.GetEffectiveStatsAsync(user);
                board.Skills        = _spellService.GetSpellsForClass(user.Class ?? "");
            }

            return View(board);
        }

        // ===== CREATE =====

        [HttpGet("Create")]
        public IActionResult Create([FromQuery] string type = "habit")
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var taskType = type.ToLower() switch
            {
                "daily"  => TaskType.Daily,
                "todo"   => TaskType.Todo,
                "reward" => TaskType.Reward,
                _        => TaskType.Habit
            };

            var model = new CreateTaskViewModel { Type = taskType };
            ViewBag.InitialType = type.ToLower();
            return View(model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.InitialType = model.Type.ToString().ToLower();
                return View(model);
            }

            await _taskService.CreateTaskAsync(userId.Value, model);
            TempData["Success"] = $"Task \"{model.Text}\" created!";
            return RedirectToAction(nameof(Index));
        }

        // ===== EDIT =====

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            // Load task with ownership check via service
            var board = await _taskService.GetUserTasksAsync(userId.Value);
            var allTasks = board.Habits
                .Concat(board.Dailies)
                .Concat(board.Todos)
                .Concat(board.Rewards)
                .ToList();

            var task = allTasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            var model = new EditTaskViewModel
            {
                Id              = task.Id,
                Type            = task.Type,
                Text            = task.Text,
                Notes           = task.Notes,
                Priority        = task.Priority,
                Up              = task.Up ?? true,
                Down            = task.Down ?? true,
                HabitFrequency  = task.HabitFrequency ?? HabitResetFrequency.Daily,
                DailyFrequency  = task.DailyFrequency ?? DailyFrequency.Weekly,
                EveryX          = task.EveryX,
                StartDate       = task.StartDate,
                RepeatMonday    = task.RepeatMonday,
                RepeatTuesday   = task.RepeatTuesday,
                RepeatWednesday = task.RepeatWednesday,
                RepeatThursday  = task.RepeatThursday,
                RepeatFriday    = task.RepeatFriday,
                RepeatSaturday  = task.RepeatSaturday,
                RepeatSunday    = task.RepeatSunday,
                DueDate         = task.DueDate,
                GoldCost        = task.GoldCost,
                ExistingChecklist = task.ChecklistItems?
                    .OrderBy(c => c.SortOrder)
                    .Select(c => new EditTaskViewModel.ExistingChecklistItem
                    {
                        ItemId      = c.Id,
                        Text        = c.Text,
                        IsCompleted = c.IsCompleted
                    }).ToList() ?? new()
            };

            return View(model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTaskViewModel model)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(model);

            await _taskService.UpdateTaskAsync(id, userId.Value, model);
            TempData["Success"] = $"Task updated!";
            return RedirectToAction(nameof(Index));
        }

        // ===== DELETE =====

        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            await _taskService.DeleteTaskAsync(id, userId.Value);
            return RedirectToAction(nameof(Index));
        }

        // ===== SCORE (AJAX → JSON) =====

        [HttpPost("Score/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Score(int id, [FromQuery] string direction = "up")
        {
            var userId = GetUserId();
            if (userId == null)
                return Json(new { success = false, error = "Not logged in" });

            var result = await _taskService.ScoreTaskAsync(id, userId.Value, direction);
            return Json(result);
        }

        // ===== CHECKLIST TOGGLE (AJAX → JSON) =====

        [HttpPost("ChecklistToggle/{itemId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChecklistToggle(int itemId)
        {
            var userId = GetUserId();
            if (userId == null)
                return Json(new { success = false });

            var newState = await _taskService.ToggleChecklistItemAsync(itemId, userId.Value);
            return Json(new { success = true, isCompleted = newState });
        }

        // ===== REORDER (AJAX → JSON) =====

        [HttpPost("Reorder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reorder([FromBody] ReorderRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                return Json(new { success = false });

            await _taskService.ReorderTasksAsync(userId.Value, request.Type, request.Ids);
            return Json(new { success = true });
        }

        // ===== CLEAR COMPLETED TODOS =====

        [HttpGet("ClearCompleted")]
        public async Task<IActionResult> ClearCompleted()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            await _taskService.ClearCompletedTodosAsync(userId.Value);
            return RedirectToAction(nameof(Index));
        }

        // ===== CAST SPELL (AJAX → JSON) =====

        [HttpPost("CastSpell")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CastSpell(string spellKey, int? taskId = null)
        {
            var userId = GetUserId();
            if (userId == null)
                return Json(new { success = false, error = "Not logged in." });

            var (success, error, data) = await _spellService.CastAsync(userId.Value, spellKey, taskId);
            if (!success)
                return Json(new { success = false, error });

            // Return updated user stats so the client can refresh the HUD
            var user = await _context.Users
                .Include(u => u.OwnedGear!).ThenInclude(ug => ug.GearItem)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            int maxMana = 30;
            if (user != null)
            {
                var es = await _characterService.GetEffectiveStatsAsync(user);
                maxMana = es.MaxMana;
            }

            return Json(new
            {
                success  = true,
                spellKey = spellKey,
                newMana  = (int)(user?.Mana ?? 0),
                maxMana  = maxMana,
                newHP    = user?.HP ?? 0,
                newGold  = user?.Gold ?? 0,
                newXP    = user?.XP ?? 0,
                data     = data
            });
        }

        // ===== HELPERS =====

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);

        public class ReorderRequest
        {
            public TaskType  Type { get; set; }
            public List<int> Ids  { get; set; } = new();
        }
    }
}
