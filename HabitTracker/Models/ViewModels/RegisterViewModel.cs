using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public IFormFile? AvatarFile { get; set; }
    }
}