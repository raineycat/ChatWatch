using ChatWatchApp.Models;

namespace ChatWatchApp.Services;

public interface IUsernameService
{
    string GetUsername(Guid uuid);
    string GetUsername(Player player) => GetUsername(player.ID);

    Task<string> GetUsernameAsync(Guid uuid);
    Task<string> GetUsernameAsync(Player player) => GetUsernameAsync(player.ID);
    
    void ClearCache();
}