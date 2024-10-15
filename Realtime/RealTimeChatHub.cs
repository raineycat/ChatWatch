using ChatWatchApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatWatchApp.Realtime
{
    public class RealTimeChatHub : Hub
    {
        private readonly ILogger<RealTimeChatHub> _logger;

        public RealTimeChatHub(ILogger<RealTimeChatHub> logger)
        {
            _logger = logger;
        }

        public async Task TestMe()
        {
            await Clients.Caller.SendAsync("TestMeBack", "meow");
            _logger.LogDebug("Answered TestMe method");
        }

        public const string SendOutMessage = "RTChatMsg";
    }
}
