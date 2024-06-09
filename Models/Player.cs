namespace ChatWatchApp.Models;

public class Player
{
    public required Guid ID { get; set; }
    public string CustomName { get; set; } = "";
}