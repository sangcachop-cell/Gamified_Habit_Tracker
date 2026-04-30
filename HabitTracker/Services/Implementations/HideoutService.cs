using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Services.Implementations
{
    public class HideoutService : IHideoutService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HideoutService> _logger;

        public HideoutService(AppDbContext context, ILogger<HideoutService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task EnsureUserFacilitiesAsync(int userId)
        {
            var allFacilityIds = await _context.Facilities
                .Where(f => f.IsActive)
                .Select(f => f.Id)
                .ToListAsync();

            var existingIds = await _context.UserFacilities
                .Where(uf => uf.UserId == userId)
                .Select(uf => uf.FacilityId)
                .ToListAsync();

            var missing = allFacilityIds.Except(existingIds).ToList();

            if (!missing.Any()) return;

            foreach (var facilityId in missing)
            {
                _context.UserFacilities.Add(new UserFacility
                {
                    UserId = userId,
                    FacilityId = facilityId,
                    Level = 1,
                    UnlockedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} unlocked {missing.Count} new facilities");
        }

        public async Task<List<UserFacility>> GetUserFacilitiesAsync(int userId)
        {
            return await _context.UserFacilities
                .Where(uf => uf.UserId == userId)
                .Include(uf => uf.Facility)
                .OrderBy(uf => uf.FacilityId)
                .ToListAsync();
        }

        public (int atk, int hp, int armor, double xpGain, int stamina) GetFacilityBuffs(List<UserFacility> facilities)
        {
            int atk = 0, hp = 0, armor = 0, stamina = 0;
            double xpGain = 0;

            foreach (var uf in facilities)
            {
                if (uf.Facility == null) continue;
                int total = uf.Facility.BuffPerLevel * uf.Level;

                switch (uf.Facility.StatAffected)
                {
                    case "ATK":     atk     += total; break;
                    case "HP":      hp      += total; break;
                    case "Armor":   armor   += total; break;
                    case "XPGain":  xpGain  += total; break;
                    case "Stamina": stamina += total; break;
                }
            }

            return (atk, hp, armor, xpGain, stamina);
        }

        public int GetStorageLevel(List<UserFacility> facilities) =>
            facilities.FirstOrDefault(uf => uf.FacilityId == FacilityCatalogue.STORAGE_FACILITY_ID)?.Level ?? 1;

        public async Task<(bool Success, string? Error)> StartUpgradeAsync(int userId, int facilityId)
        {
            var uf = await _context.UserFacilities
                .FirstOrDefaultAsync(x => x.UserId == userId && x.FacilityId == facilityId);
            if (uf == null) return (false, "Facility not found.");

            if (uf.Level >= 5) return (false, "Already at max level.");
            if (uf.UpgradeStartedAt != null) return (false, "Upgrade already in progress.");

            var cost = FacilityCatalogue.GetCost(facilityId, uf.Level);
            if (cost == null) return (false, "No upgrade available.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return (false, "User not found.");

            if (user.Wood < cost.Wood || user.Stone < cost.Stone)
                return (false, $"Need {cost.Wood} Wood and {cost.Stone} Stone.");

            user.Wood  -= cost.Wood;
            user.Stone -= cost.Stone;
            uf.UpgradeStartedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} started upgrade on facility {facilityId} (lv{uf.Level}→{uf.Level+1})");
            return (true, null);
        }

        public async Task CompleteReadyUpgradesAsync(int userId)
        {
            var upgrading = await _context.UserFacilities
                .Where(uf => uf.UserId == userId && uf.UpgradeStartedAt != null)
                .ToListAsync();

            bool any = false;
            foreach (var uf in upgrading)
            {
                var cost = FacilityCatalogue.GetCost(uf.FacilityId, uf.Level);
                if (cost == null) { uf.UpgradeStartedAt = null; any = true; continue; }

                if (DateTime.UtcNow >= uf.UpgradeStartedAt!.Value + cost.Duration)
                {
                    uf.Level++;
                    uf.UpgradeStartedAt = null;
                    any = true;
                    _logger.LogInformation($"User {userId} facility {uf.FacilityId} upgraded to lv{uf.Level}");
                }
            }

            if (any) await _context.SaveChangesAsync();
        }
    }
}
