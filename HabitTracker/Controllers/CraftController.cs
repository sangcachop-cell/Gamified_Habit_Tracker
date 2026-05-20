using HabitTracker.Constants;
using HabitTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class CraftController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CraftController> _logger;

        public CraftController(AppDbContext context, ILogger<CraftController> logger)
        {
            _context = context;
            _logger  = logger;
        }

        // POST /Craft/Start — instant craft (timer disabled for testing)
        [HttpPost("Start")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(string recipeId)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, error = "Not logged in" });

            var recipe = WorkbenchCatalogue.GetRecipe(recipeId);
            if (recipe == null) return Json(new { success = false, error = "Unknown recipe" });

            // Check workbench level
            int wbLevel = await _context.UserFacilities
                .Where(uf => uf.UserId == userId && uf.FacilityId == WorkbenchCatalogue.WORKBENCH_FACILITY_ID)
                .Select(uf => uf.Level)
                .FirstOrDefaultAsync();
            if (wbLevel == 0) wbLevel = 1;

            if (recipe.MinLevel > wbLevel)
                return Json(new { success = false, error = $"Requires Workbench level {recipe.MinLevel}" });

            // Find one input item in any container
            var inputItem = await _context.UserInventoryItems
                .Where(i => i.UserId == userId && i.ItemId == recipe.InputItemId)
                .FirstOrDefaultAsync();

            if (inputItem == null)
                return Json(new { success = false, error = $"No {recipe.InputItemId} in inventory" });

            // Remove input item
            _context.UserInventoryItems.Remove(inputItem);

            // Add output to user material field
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return Json(new { success = false, error = "User not found" });

            if (recipe.OutputField == "Wood")
                user.Wood += recipe.OutputQty;
            else if (recipe.OutputField == "Stone")
                user.Stone += recipe.OutputQty;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                $"User {userId} crafted {recipeId}: consumed {recipe.InputItemId}, gained {recipe.OutputQty} {recipe.OutputField}");

            return Json(new {
                success       = true,
                message       = $"{recipe.OutputLabel} added!",
                newWood       = user.Wood,
                newStone      = user.Stone,
                removedItemId = inputItem.Id,
                inputItemId   = recipe.InputItemId
            });
        }

        private int? GetUserId() => HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
    }
}
