using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Avatar { get; set; }

        public int Level { get; set; } = 1;
        public int XP { get; set; } = 0;

        public List<Habit>? Habits { get; set; }

        // ===== NEW FIELDS =====
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? Bio { get; set; }

        public string? FacebookLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? InstagramLink { get; set; }
        // ===== GAMIFICATION =====
        public int CurrentStreak { get; set; } = 0;
        public int LongestStreak { get; set; } = 0;
        public DateTime? LastCheckInDate { get; set; }

        public List<UserBadge>? UserBadges { get; set; }
        public DateTime? LastCompletedDate { get; set; }
    }
}