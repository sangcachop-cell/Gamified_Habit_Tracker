using HabitTracker.Models;
using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services
{
    /// <summary>
    /// Quản lý đăng ký, đăng nhập, mật khẩu
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Hash mật khẩu bằng BCrypt
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verify mật khẩu plaintext với hash
        /// </summary>
        bool VerifyPassword(string password, string hash);

        /// <summary>
        /// Validate input register
        /// </summary>
        (bool isValid, string? errorMessage) ValidateRegister(RegisterViewModel model);

        /// <summary>
        /// Validate input login
        /// </summary>
        (bool isValid, string? errorMessage) ValidateLogin(LoginViewModel model);

        /// <summary>
        /// Validate password change
        /// </summary>
        (bool isValid, string? errorMessage) ValidatePasswordChange(string oldPassword, string newPassword, string hash);
    }
}