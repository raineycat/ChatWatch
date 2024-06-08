namespace ChatWatchApp.Models;

public interface IMessage
{
    Guid ID { get; set; }
    DateTime Timestamp { get; set; }
    Player Sender { get; set; }
    string Content { get; set; }
    string Server { get; set; }
}