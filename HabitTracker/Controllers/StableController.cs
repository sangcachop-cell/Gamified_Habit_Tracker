using HabitTracker.Constants;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class StableController : Controller
    {
        private readonly IStableService _stableService;

        public StableController(IStableService stableService)
        {
            _stableService = stableService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var vm = await _stableService.GetStableViewModelAsync(userId.Value);
            return View(vm);
        }

        [HttpPost("Hatch")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Hatch([FromForm] int eggGameItemId, [FromForm] int potionGameItemId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _stableService.HatchAsync(userId.Value, eggGameItemId, potionGameItemId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                petKey = result.HatchedPetKey,
                petName = result.HatchedPetName,
                petImagePath = result.PetImagePath
            });
        }

        [HttpPost("Feed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feed([FromForm] string petKey, [FromForm] int foodGameItemId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _stableService.FeedAsync(userId.Value, petKey, foodGameItemId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                evolved = result.Evolved,
                newFeedingPoints = result.NewFeedingPoints,
                mountIconPath = result.MountIconPath,
                newFoodQuantity = result.NewFoodQuantity,
                foodGameItemId = foodGameItemId
            });
        }

        [HttpPost("SetActivePet")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActivePet([FromForm] string? petKey)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _stableService.SetActivePetAsync(userId.Value, petKey);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                activePetKey = result.NewActivePetKey
            });
        }

        [HttpPost("SetActiveMount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActiveMount([FromForm] string? mountKey)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _stableService.SetActiveMountAsync(userId.Value, mountKey);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                activeMountKey = result.NewActiveMountKey
            });
        }

        private int? GetUserId() =>
            HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}