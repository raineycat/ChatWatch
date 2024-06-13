namespace ChatWatchApp.Services;

public interface IServerSettings
{
    public string ServerName { get; }
    public Guid IngestToken { get; }
}