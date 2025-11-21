using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;
using StudentOrg_A4_Website.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StudentOrgContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 44))));

builder.Services.AddIdentity<UserAccount, IdentityRole>()
    .AddEntityFrameworkStores<StudentOrgContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<SignInManager<UserAccount>>();
builder.Services.AddScoped<UserManager<UserAccount>>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<GoogleDriveServices>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/UserAccount/Login";
    options.LogoutPath = "/UserAccount/Logout";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
