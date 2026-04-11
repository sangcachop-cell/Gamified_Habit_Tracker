using HabitTracker.Constants;
using HabitTracker.Data;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HabitTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            AppDbContext context,
            IAuthService authService,
            IFileService fileService,
            ILogger<AccountController> logger)
        {
            _context = context;
            _authService = authService;
            _fileService = fileService;
            _logger = logger;
        }

        // ===== REGISTER =====
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var (isValid, errorMessage) = _authService.ValidateRegister(model);
            if (!isValid)
            {
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", AppConstants.Messages.EMAIL_EXISTS);
                return View(model);
            }

            var (fileValid, fileError) = _fileService.ValidateFile(model.AvatarFile);
            if (!fileValid)
            {
                ModelState.AddModelError("AvatarFile", fileError);
                return View(model);
            }

            string avatarFileName = await _fileService.SaveAvatarAsync(model.AvatarFile);

            var user = new User
            {
                Username = model.Username.Trim(),
                Email = model.Email.ToLower().Trim(),
                Password = _authService.HashPassword(model.Password),
                Avatar = avatarFileName,
                XP = 0,
                Level = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User registered: {user.Email}");
            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";

            return RedirectToAction(nameof(Login));
        }

        // ===== LOGIN =====
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var (isValid, errorMessage) = _authService.ValidateLogin(model);
            if (!isValid)
            {
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == model.Email.ToLower().Trim());

            if (user == null || !_authService.VerifyPassword(model.Password, user.Password))
            {
                ModelState.AddModelError("", AppConstants.Messages.INVALID_CREDENTIALS);
                _logger.LogWarning($"Failed login attempt for: {model.Email}");
                return View(model);
            }

            SetUserSession(user);
            _logger.LogInformation($"User logged in: {user.Email}");

            return RedirectToAction("Index", "Dashboard");
        }

        // ===== PROFILE (VIEW & EDIT) =====
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction(nameof(Login));

            var user = await _context.Users
                .Include(u => u.UserBadges)
                    .ThenInclude(ub => ub.Badge)
                .Include(u => u.UserQuests)
                    .ThenInclude(uq => uq.Quest)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction(nameof(Login));

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User model, IFormFile? AvatarFile)
        {
            var user = await _context.Users.FindAsync(model.Id);
            if (user == null)
                return RedirectToAction(nameof(Login));

            try
            {
                // Update basic info
                user.Username = model.Username?.Trim() ?? user.Username;
                user.Email = model.Email?.ToLower().Trim() ?? user.Email;
                user.PhoneNumber = model.PhoneNumber?.Trim();
                user.Location = model.Location?.Trim();
                user.Gender = model.Gender?.Trim();
                user.DateOfBirth = model.DateOfBirth;
                user.Bio = model.Bio?.Trim();
                user.FacebookLink = model.FacebookLink?.Trim();
                user.LinkedInLink = model.LinkedInLink?.Trim();
                user.InstagramLink = model.InstagramLink?.Trim();

                // Handle avatar upload
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    if (!AvatarFile.ContentType.StartsWith("image/"))
                    {
                        ModelState.AddModelError("AvatarFile", "Please upload an image file");
                        return View(model);
                    }

                    if (AvatarFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("AvatarFile", "File size must be less than 5MB");
                        return View(model);
                    }

                    var fileName = $"{user.Id}_{DateTime.UtcNow.Ticks}{Path.GetExtension(AvatarFile.FileName)}";
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await AvatarFile.CopyToAsync(stream);
                    }

                    user.Avatar = fileName;
                    HttpContext.Session.SetString(AppConstants.SESSION_AVATAR, fileName);
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.SetString(AppConstants.SESSION_USERNAME, user.Username);

                _logger.LogInformation($"User updated profile: {user.Email}");
                TempData["Success"] = "✅ Profile updated successfully!";

                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating profile: {ex.Message}");
                TempData["Error"] = "❌ Error updating profile. Please try again.";
                return View(model);
            }
        }

        // ===== CHANGE PASSWORD =====
        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction(nameof(Login));

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return RedirectToAction(nameof(Login));

            try
            {
                if (!_authService.VerifyPassword(oldPassword, user.Password))
                {
                    ModelState.AddModelError("oldPassword", "Current password is incorrect");
                    return View();
                }

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    ModelState.AddModelError("newPassword", "New password must be at least 6 characters");
                    return View();
                }

                if (newPassword != confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "Passwords do not match");
                    return View();
                }

                user.Password = _authService.HashPassword(newPassword);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User changed password: {user.Email}");
                TempData["Success"] = "✅ Password changed successfully!";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error changing password: {ex.Message}");
                TempData["Error"] = "❌ Error changing password";
                return View();
            }
        }

        // ===== LOGOUT =====
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _logger.LogInformation("User logged out");
            TempData["Success"] = "Đã đăng xuất!";
            return RedirectToAction(nameof(Login));
        }

        // ===== GOOGLE LOGIN =====
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            properties.Items["prompt"] = "select_account";

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal == null)
                return RedirectToAction(nameof(Login));

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(Login));

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email.ToLower());

            if (user == null)
            {
                user = new User
                {
                    Email = email.ToLower(),
                    Username = name ?? "Google User",
                    Password = _authService.HashPassword(AppConstants.GOOGLE_LOGIN_PASSWORD),
                    Avatar = AppConstants.DEFAULT_AVATAR
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"New user from Google: {email}");
            }

            SetUserSession(user);
            return RedirectToAction("Index", "Dashboard");
        }

        // ===== LEADERBOARD =====
        public async Task<IActionResult> Leaderboard()
        {
            var topXP = await _context.Users
                .OrderByDescending(u => u.XP)
                .Take(AppConstants.LEADERBOARD_TOP_COUNT)
                .ToListAsync();

            var topStreak = await _context.Users
                .OrderByDescending(u => u.CurrentStreak)
                .Take(AppConstants.LEADERBOARD_TOP_COUNT)
                .ToListAsync();

            ViewBag.TopXP = topXP;
            ViewBag.TopStreak = topStreak;

            return View();
        }

        // ===== HELPER METHODS =====
        private void SetUserSession(User user)
        {
            HttpContext.Session.SetInt32(AppConstants.SESSION_USER_ID, user.Id);
            HttpContext.Session.SetString(AppConstants.SESSION_USERNAME, user.Username);
            HttpContext.Session.SetString(
                AppConstants.SESSION_AVATAR,
                user.Avatar ?? AppConstants.DEFAULT_AVATAR);
            HttpContext.Session.SetString(
                AppConstants.SESSION_IS_ADMIN,
                user.IsAdmin ? "true" : "false");
        }

        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID);
        }
    }
}