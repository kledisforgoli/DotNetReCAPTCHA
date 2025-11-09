using siguria.Data;
using Microsoft.EntityFrameworkCore;
using reCAPTCHA.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRecaptcha(options =>
{
    options.SiteKey = "6Le8ICMrAAAAANZ8Ix5-A9GKU_XJP8BZcoSvsZ37";
    options.SecretKey = "6Le8ICMrAAAAACtwR-LMXvhI71uLzMf-SOg6UWVw";
    
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();