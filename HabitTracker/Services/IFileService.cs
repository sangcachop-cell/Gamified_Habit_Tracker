namespace HabitTracker.Services
{
    /// <summary>
    /// Quản lý upload file, xóa file, v.v.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Lưu avatar file vào wwwroot/images
        /// </summary>
        Task<string> SaveAvatarAsync(IFormFile? file);

        /// <summary>
        /// Xóa file cũ nếu tồn tại
        /// </summary>
        Task DeleteFileAsync(string? fileName);

        /// <summary>
        /// Validate file (kích thước, định dạng)
        /// </summary>
        (bool isValid, string? errorMessage) ValidateFile(IFormFile? file);
    }
}