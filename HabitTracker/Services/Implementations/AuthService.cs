using HabitTracker.Constants;
using HabitTracker.Models.ViewModels;
using BCrypt.Net;

namespace HabitTracker.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
        }

        public string HashPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error hashing password: {ex.Message}");
                throw;
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error verifying password: {ex.Message}");
                return false;
            }
        }

        public (bool isValid, string? errorMessage) ValidateRegister(RegisterViewModel model)
        {
            // Check null/empty
            if (string.IsNullOrWhiteSpace(model?.Email) ||
                string.IsNullOrWhiteSpace(model?.Username) ||
                string.IsNullOrWhiteSpace(model?.Password))
            {
                return (false, "Vui lòng điền đầy đủ thông tin!");
            }

            // Check email format
            if (!model.Email.Contains("@"))
                return (false, AppConstants.Messages.INVALID_EMAIL);

            // Check username length
            if (model.Username.Length < AppConstants.MIN_USERNAME_LENGTH ||
                model.Username.Length > AppConstants.MAX_USERNAME_LENGTH)
            {
                return (false, $"Username phải từ {AppConstants.MIN_USERNAME_LENGTH} " +
                    $"đến {AppConstants.MAX_USERNAME_LENGTH} ký tự!");
            }

            // Check password length
            if (model.Password.Length < AppConstants.MIN_PASSWORD_LENGTH)
                return (false, AppConstants.Messages.PASSWORD_TOO_SHORT);

            return (true, null);
        }

        public (bool isValid, string? errorMessage) ValidateLogin(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Email) || string.IsNullOrWhiteSpace(model?.Password))
                return (false, "Email và mật khẩu không được để trống!");

            if (!model.Email.Contains("@"))
                return (false, AppConstants.Messages.INVALID_EMAIL);

            return (true, null);
        }

        public (bool isValid, string? errorMessage) ValidatePasswordChange(
            string oldPassword, string newPassword, string hash)
        {
            // Verify old password
            if (!VerifyPassword(oldPassword, hash))
                return (false, AppConstants.Messages.WRONG_PASSWORD);

            // Validate new password length
            if (newPassword.Length < AppConstants.MIN_PASSWORD_LENGTH)
                return (false, AppConstants.Messages.PASSWORD_TOO_SHORT);

            // New password không được giống old password
            if (oldPassword == newPassword)
                return (false, "Mật khẩu mới phải khác mật khẩu cũ!");

            return (true, null);
        }
    }
}