
namespace ChatWatchApp.Services.Impl;

public class MojangUsernameService : IUsernameService
{
    private DateTime _nextClearTime;
    private Dictionary<Guid, string> _cache;
    private HttpClient _client;

    public MojangUsernameService()
    {
        _cache = new Dictionary<Guid, string>();
        ClearCache();

        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://sessionserver.mojang.com/");
    }

    public void ClearCache()
    {
        _cache.Clear();
        _nextClearTime = DateTime.Now.AddDays(1);
    }

    public string GetUsername(Guid uuid)
    {
        var task = Task.Run(() => GetUsernameAsync(uuid));
        return task.Result;
    }

    public async Task<string> GetUsernameAsync(Guid uuid)
    {
        if(DateTime.Now >= _nextClearTime)
        {
            ClearCache();
            // save some extra checks by just doing it here
            return await CacheFreshUsername(uuid);
        }

        if(_cache.TryGetValue(uuid, out var name)) return name;
        return await CacheFreshUsername(uuid);
    }

    private async Task<string> CacheFreshUsername(Guid uuid)
    {
        // https://wiki.vg/Mojang_API#UUID_to_Profile_and_Skin.2FCape

        var endpoint = $"/session/minecraft/profile/{uuid}";
        var resp = await _client.GetAsync(endpoint);
        var json = await resp.Content.ReadFromJsonAsync<MojangProfileResponse>();
        return json?.Name ?? "Unknown";
    }

    private class MojangProfileResponse
    {
        public required string ID { get; set; }
        public required string Name { get; set; }
    }
}
