using HabitTracker.Data;
using HabitTracker.Services;
using HabitTracker.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// ===== SERVICES REGISTRATION =====
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.Scope.Add("email");
    options.Scope.Add("profile");
});

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

// Logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// ===== MIDDLEWARE PIPELINE =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ===== ROUTE CONFIGURATION =====
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ===== DATABASE INITIALIZATION =====
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        // Apply migrations
        dbContext.Database.Migrate();

        // Seed admin user n?u ch?a t?n t?i
        var adminEmail = builder.Configuration["Admin:Email"] ?? "admin@gmail.com";
        var adminPassword = builder.Configuration["Admin:Password"] ?? "123456";

        var admin = dbContext.Users.FirstOrDefault(u => u.Email == adminEmail);

        if (admin == null)
        {
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            var hashedPassword = authService.HashPassword(adminPassword);

            admin = new HabitTracker.Models.User
            {
                Email = adminEmail,
                Username = "Admin",
                Password = hashedPassword,
                IsAdmin = true,
                Avatar = "default.png"
            };

            dbContext.Users.Add(admin);
            dbContext.SaveChanges();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"Admin user created: {adminEmail}");
        }
        else if (!admin.IsAdmin)
        {
            // N?u user n?y t?n t?i nh?ng ch?a l? admin, c?p nh?t
            admin.IsAdmin = true;
            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError($"Error during database initialization: {ex.Message}");
    }
}

app.Run();