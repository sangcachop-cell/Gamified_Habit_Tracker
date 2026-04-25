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
    }
}
