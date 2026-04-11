using HabitTracker.Constants;

namespace HabitTracker.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment env, ILogger<FileService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> SaveAvatarAsync(IFormFile? file)
        {
            // Nếu không có file, trả về avatar mặc định
            if (file == null)
                return AppConstants.DEFAULT_AVATAR;

            // Validate file
            var (isValid, errorMessage) = ValidateFile(file);
            if (!isValid)
                return AppConstants.DEFAULT_AVATAR;

            try
            {
                // Tạo thư mục nếu chưa tồn tại
                string uploadPath = Path.Combine(_env.WebRootPath, AppConstants.IMAGES_FOLDER);
                Directory.CreateDirectory(uploadPath);

                // Tạo tên file duy nhất
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string fullPath = Path.Combine(uploadPath, fileName);

                // Lưu file
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation($"Avatar uploaded: {fileName}");
                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading avatar: {ex.Message}");
                return AppConstants.DEFAULT_AVATAR;
            }
        }

        public async Task DeleteFileAsync(string? fileName)
        {
            // Không xóa avatar mặc định
            if (string.IsNullOrEmpty(fileName) || fileName == AppConstants.DEFAULT_AVATAR)
                return;

            try
            {
                string fullPath = Path.Combine(_env.WebRootPath, AppConstants.IMAGES_FOLDER, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation($"Avatar deleted: {fileName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting avatar: {ex.Message}");
            }
        }

        public (bool isValid, string? errorMessage) ValidateFile(IFormFile? file)
        {
            if (file == null)
                return (true, null);

            // Kiểm tra kích thước
            if (file.Length > AppConstants.MAX_FILE_SIZE)
                return (false, AppConstants.Messages.FILE_TOO_LARGE);

            // Kiểm tra extension
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!AppConstants.ALLOWED_EXTENSIONS.Contains(extension))
                return (false, AppConstants.Messages.INVALID_FILE_TYPE);

            return (true, null);
        }
    }
}