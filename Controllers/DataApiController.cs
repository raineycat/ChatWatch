using ChatWatchApp.Data;
using ChatWatchApp.Models;
using ChatWatchApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatWatchApp.Controllers;

[ApiController]
[Route("[controller]")]
public class DataApiController : ControllerBase
{
    private ApplicationDbContext _dbc;
    private IUsernameService _username;

    public DataApiController(ApplicationDbContext dbc, IUsernameService username) 
    {
        _dbc = dbc;
        _username = username;
    }

    [HttpGet("DailyMessageCount")]
    public async Task<MessageCountData> GetDailyMessageCount()
    {
        var numChats = await _dbc.ChatMessage.Where(m => m.Timestamp.Date == DateTime.Today).CountAsync();
        var numPrivs = await _dbc.PrivateMessage.Where(m => m.Timestamp.Date == DateTime.Today).CountAsync();
        return new MessageCountData(numChats, numPrivs);
    }

    [HttpGet("WeeklyMessageCount")]
    public async Task<List<MessageCountData>> GetWeeklyMessageCount()
    {
        var counts = new MessageCountData[7];

        for(int i = 6; i >= 0; i--)
        {
            var thisday = DateTime.Today.AddDays(-i);
            var chatsThisDay = await _dbc.ChatMessage.Where(m => m.Timestamp.Date == thisday).CountAsync();
            var pmsThisDay = await _dbc.PrivateMessage.Where(m => m.Timestamp.Date == thisday).CountAsync();
            counts[6 - i] = new MessageCountData(chatsThisDay, pmsThisDay);
        }

        return counts.ToList();
    }

    [HttpGet("MostActivePlayers")]
    public async Task<Dictionary<string, MessageCountData>> GetMostActivePlayers()
    {
        var dict = new Dictionary<string, MessageCountData>();
        var today = DateTime.Now.Date;

        foreach(var player in await _dbc.Player.ToListAsync())
        {
            var numChats = await _dbc.ChatMessage
                .Where(m => m.Timestamp.Year == today.Year && m.Timestamp.Month == today.Month)
                .Where(m => m.Sender == player)
                .CountAsync();

            var numPrivs = await _dbc.PrivateMessage
                .Where(m => m.Timestamp.Year == today.Year && m.Timestamp.Month == today.Month)
                .Where(m => m.Sender == player)
                .CountAsync();

            dict.Add(_username.GetUsername(player), new MessageCountData(numChats, numPrivs));
        }

        return dict;
    }
}
