namespace ChatWatchApp.Services.Impl;

public class ConfigServerSettings : IServerSettings
{
    public const string SectionName = "CWServerConfig";
    private IConfigurationSection _config;

    public ConfigServerSettings(IConfiguration config) {
        _config = config.GetSection(SectionName);
    }

    public string ServerName => _config.GetValue<string>("ServerName") ?? "ChatWatch Server";
    public Guid IngestToken => _config.GetValue<Guid>("IngestToken");
}