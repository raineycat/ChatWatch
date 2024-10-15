using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChatWatchApp.Data;
using ChatWatchApp.Services;
using ChatWatchApp.Services.Impl;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ChatWatchApp;
using ChatWatchApp.Realtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var devMySql = builder.Configuration.GetSection("CWServerConfig").GetValue<bool>("UseMySqlInDevelopment");
if(builder.Environment.IsDevelopment() && !devMySql) {
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
} else {
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)), ServiceLifetime.Transient);
}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<IUsernameService, MojangUsernameService>();
builder.Services.AddSingleton<IServerSettings, ConfigServerSettings>();

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

using(var scope = app.Services.CreateScope())
{
    StartupHelper.SetupAppDbAsync(scope, app.Logger).Wait();
    app.Logger.LogInformation("Server name: '{}'", app.Services.GetService<IServerSettings>()?.ServerName);
}

app.MapRazorPages();
app.MapControllers();
app.MapHub<RealTimeChatHub>("/rtchat");

app.Run();
