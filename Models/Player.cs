namespace ChatWatchApp.Models;

public class Player
{
    public required Guid ID { get; set; }
    public required string Username { get; set; }
    public string CustomName { get; set; } = "";
}