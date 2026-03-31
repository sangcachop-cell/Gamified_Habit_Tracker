using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

namespace HabitTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ===== REGISTER =====
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingUser = _context.Users
                .FirstOrDefault(x => x.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại!");
                return View(model);
            }

            string fileName = "default.png";

            if (model.AvatarFile != null)
            {
                string path = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(path);

                fileName = Guid.NewGuid() + Path.GetExtension(model.AvatarFile.FileName);
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                model.AvatarFile.CopyTo(stream);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Avatar = fileName,
                XP = 0,
                Level = 1
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ===== LOGIN =====
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Avatar", user.Avatar ?? "default.png");

            return RedirectToAction("Index", "Dashboard");
        }

        // ===== PROFILE =====
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users
                .Include(u => u.UserBadges)
                .ThenInclude(ub => ub.Badge)
                .Include(u => u.Habits)
                .FirstOrDefault(u => u.Id == userId);

            return View(user);
        }

        // ===== EDIT PROFILE =====
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users.Find(userId);

            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile(User model, IFormFile? AvatarFile)
        {
            var user = _context.Users.Find(model.Id);

            if (user == null)
                return RedirectToAction("Login");

            user.Username = model.Username;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Location = model.Location;
            user.Gender = model.Gender;
            user.DateOfBirth = model.DateOfBirth;
            user.Bio = model.Bio;
            user.FacebookLink = model.FacebookLink;
            user.LinkedInLink = model.LinkedInLink;
            user.InstagramLink = model.InstagramLink;

            if (AvatarFile != null)
            {
                string path = Path.Combine(_env.WebRootPath, "images");

                string fileName = Guid.NewGuid() + Path.GetExtension(AvatarFile.FileName);
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                AvatarFile.CopyTo(stream);

                user.Avatar = fileName;
                HttpContext.Session.SetString("Avatar", fileName);
            }

            HttpContext.Session.SetString("Username", user.Username);

            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        // ===== CHANGE PASSWORD =====
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users.Find(userId);

            if (user.Password != oldPassword)
            {
                ViewBag.Error = "Sai mật khẩu cũ";
                return View();
            }

            user.Password = newPassword;
            _context.SaveChanges();

            ViewBag.Success = "Đổi mật khẩu thành công!";
            return View();
        }

        // ===== LOGOUT =====
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };

            // 🔥 BẮT GOOGLE CHỌN LẠI ACCOUNT
            properties.Items["prompt"] = "select_account";

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = name,
                    Password = "GOOGLE_LOGIN",
                    Avatar = "default.png"
                };

                _context.Users.Add(user);
                _context.SaveChanges();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Leaderboard()
        {
            var users = _context.Users.ToList();

            // TOP XP
            var topXP = users
                .OrderByDescending(u => u.XP)
                .Take(10)
                .ToList();

            // TOP STREAK
            var topStreak = users
                .OrderByDescending(u => u.CurrentStreak)
                .Take(10)
                .ToList();

            ViewBag.TopXP = topXP;
            ViewBag.TopStreak = topStreak;

            return View();
        }
    }
}