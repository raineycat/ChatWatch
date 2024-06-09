using System.Security.Cryptography;
using ChatWatchApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatWatchApp;

public static class StartupHelper 
{
    public static async Task SetupAppDbAsync(IServiceScope scope, ILogger logger)
    {
        var dbc = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if(dbc == null) 
        {
            logger.LogError("Couldn't get a DbContext to check initialization");
            return;
        } 
        
        var isNewDb = await dbc.Database.EnsureCreatedAsync();
        if(!isNewDb) return;
        
        logger.LogInformation("Fresh database!");
        
        var store = scope.ServiceProvider.GetService<IUserStore<IdentityUser>>();
        var mgr = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();

        if(store == null || mgr == null)
        {
            logger.LogError("Couldn't get services to create default user!");
            return;
        }

        var user = new IdentityUser();
        var pw = RandomNumberGenerator.GetHexString(20, true);

        await store.SetUserNameAsync(user, "admin", CancellationToken.None);
        var res = await mgr.CreateAsync(user, pw);

        if(!res.Succeeded)
        {
            logger.LogError("Failed to create default user! {}", res.Errors.Select(e => e.Description));
            return;
        }

        logger.LogInformation("Default login: admin/{}   <- change this as soon as you log in", pw);
    }
}
