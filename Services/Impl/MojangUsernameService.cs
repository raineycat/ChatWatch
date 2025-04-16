
namespace ChatWatchApp.Services.Impl;

public class MojangUsernameService : IUsernameService
{
    private ILogger<MojangUsernameService> _logger;
    private DateTime _nextClearTime;
    private Dictionary<Guid, string> _cache;
    private HttpClient _client;

    public MojangUsernameService(ILogger<MojangUsernameService> log)
    {
        _logger = log;
        _cache = new Dictionary<Guid, string>();
        ClearCache();

        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://sessionserver.mojang.com/");
    }

    public void ClearCache()
    {
        _logger.LogInformation("Clearing username cache!");
        _cache.Clear();
        _nextClearTime = DateTime.Now.AddDays(7);
        _logger.LogInformation("Next username cache clear is on {DateTime}", _nextClearTime);
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
        _logger.LogInformation("Username for {UUID} missed the cache!", uuid);
        return await CacheFreshUsername(uuid);
    }

    private async Task<string> CacheFreshUsername(Guid uuid)
    {
        // https://wiki.vg/Mojang_API#UUID_to_Profile_and_Skin.2FCape

        var endpoint = $"/session/minecraft/profile/{uuid}";
        MojangProfileResponse? json = null;

        try {
            var resp = await _client.GetAsync(endpoint);
            json = await resp.Content.ReadFromJsonAsync<MojangProfileResponse>();
        } catch(Exception e) {
            _logger.LogError("Failed to retrieve username for '{UUID}': {Exception}", uuid, e);
        }

        if (json?.Name != null)
        {
            _cache.Add(uuid, json.Name);
        }

        return json?.Name ?? "[Unknown]";
    }

    private class MojangProfileResponse
    {
        public required string ID { get; set; }
        public required string Name { get; set; }
    }
}
