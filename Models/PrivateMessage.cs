namespace ChatWatchApp.Models;

public class PrivateMessage : IMessage
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public required Player Sender { get; set; }
    public required Player Recipient { get; set; }
    public required string Content { get; set; }
    public required string Server { get; set; }
}