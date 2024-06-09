namespace ChatWatchApp.Models;

public class MessageCountData
{
    public int NumChatMessages { get; set; }
    public int NumPrivateMessages { get; set; }

    public MessageCountData(int numChats, int numPrivs)
    {
        NumChatMessages = numChats;
        NumPrivateMessages = numPrivs;
    }
}