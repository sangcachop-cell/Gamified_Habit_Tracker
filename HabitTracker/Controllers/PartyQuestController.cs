using HabitTracker.Constants;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [Route("[controller]")]
    public class PartyQuestController : Controller
    {
        private readonly IBossQuestService _bossQuestService;
        private readonly IPartyService _partyService;

        public PartyQuestController(IBossQuestService bossQuestService, IPartyService partyService)
        {
            _bossQuestService = bossQuestService;
            _partyService = partyService;
        }

        // POST /PartyQuest/Invite
        [HttpPost("Invite")]
        public async Task<IActionResult> Invite([FromBody] QuestKeyRequest req)
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { success = false, error = "Not logged in." });

            var (success, error) = await _bossQuestService.InvitePartyAsync(userId.Value, req.QuestKey);
            return Json(new { success, error });
        }

        // POST /PartyQuest/Accept
        [HttpPost("Accept")]
        public async Task<IActionResult> Accept()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { success = false, error = "Not logged in." });

            var (success, error) = await _bossQuestService.AcceptQuestAsync(userId.Value);
            return Json(new { success, error });
        }

        // POST /PartyQuest/Reject
        [HttpPost("Reject")]
        public async Task<IActionResult> Reject()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { success = false, error = "Not logged in." });

            var (success, error) = await _bossQuestService.RejectQuestAsync(userId.Value);
            return Json(new { success, error });
        }

        // POST /PartyQuest/ForceStart
        [HttpPost("ForceStart")]
        public async Task<IActionResult> ForceStart()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { success = false, error = "Not logged in." });

            var (success, error) = await _bossQuestService.ForceStartAsync(userId.Value);
            return Json(new { success, error });
        }

        // POST /PartyQuest/Cancel
        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { success = false, error = "Not logged in." });

            var (success, error) = await _bossQuestService.CancelQuestAsync(userId.Value);
            return Json(new { success, error });
        }

        // POST /PartyQuest/Abort
        [HttpPost("Abort")]
        public async Task<IActionResult> Abort()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { success = false, error = "Not logged in." });

            var (success, error) = await _bossQuestService.AbortQuestAsync(userId.Value);
            return Json(new { success, error });
        }

        // GET /PartyQuest/Status
        [HttpGet("Status")]
        public async Task<IActionResult> Status()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
            if (userId is null) return Json(new { status = "None" });

            var party = await _partyService.GetMyPartyAsync(userId.Value);
            if (party is null) return Json(new { status = "None" });

            var dto = await _bossQuestService.GetQuestStatusAsync(party.Id);
            return Json(dto);
        }

        public sealed class QuestKeyRequest
        {
            public string QuestKey { get; set; } = string.Empty;
        }
    }
}