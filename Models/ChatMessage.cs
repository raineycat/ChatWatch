namespace ChatWatchApp.Models;

public class ChatMessage 
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public required Player Sender { get; set; }
    public required string Content { get; set; }
    public required string Server { get; set; }
}