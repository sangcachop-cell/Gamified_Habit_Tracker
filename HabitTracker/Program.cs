using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

// ===== GOOGLE LOGIN (SAFE) =====
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

    options.Scope.Add("email");
    options.Scope.Add("profile");
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

// ===== QUAN TR?NG =====
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// ===== SEED BADGE =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Badges.Any())
    {
        context.Badges.AddRange(
            new Badge { Name = "Beginner", Description = "Reach 50 XP", Icon = "badge1.png", RequiredXP = 50 },
            new Badge { Name = "Pro", Description = "Reach 100 XP", Icon = "badge2.png", RequiredXP = 100 },
            new Badge { Name = "Master", Description = "Reach 200 XP", Icon = "badge3.png", RequiredXP = 200 }
        );

        context.SaveChanges();
    }
}

app.Run();