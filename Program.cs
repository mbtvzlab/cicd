using CiCd.Data;
using CiCd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireDeveloperRole", policy => policy.RequireRole("Admin", "Developer"));
});

builder.Services.AddDbContext<CiCdDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CiCdDbContext")));

builder.Services.AddScoped<IProjectRepository, EfProjectRepository>();
builder.Services.AddScoped<IPipelineRepository, EfPipelineRepository>();
builder.Services.AddScoped<IRunLogRepository, EfRunLogRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<CiCdDbContext>();
        db.Database.Migrate();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var supportedCultures = new[]
{
    new CultureInfo("hr"),
    new CultureInfo("en-US")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "projects",
    pattern: "projekti",
    defaults: new { controller = "Project", action = "Index" });

app.MapControllerRoute(
    name: "projectDetails",
    pattern: "projekti/{id:int}/detalji",
    defaults: new { controller = "Project", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();