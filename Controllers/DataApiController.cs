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
    public async Task<Dictionary<string, int>> GetMostActivePlayers()
    {
        var dict = new Dictionary<string, int>();
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

            dict.Add(_username.GetUsername(player), numChats + numPrivs);
        }

        return dict;
    }

    private int GetOccurenceOfDay(DayOfWeek dow)
    {
        var today = DateTime.Today.Date;
        var count = 0;

        for(var i = 0; i < DateTime.DaysInMonth(today.Year, today.Month); i++)
        {
            if(today.DayOfWeek == dow)
            {
                count++;
            }
            today = today.AddDays(1);
        }

        return count;
    }

    [HttpGet("AvgActiveDaysChat")]
    public async Task<List<int>> GetAverageChatsPerDay()
    {
        var today = DateTime.Now.Date;
        var avgs = new List<int>();

        // 0 = sunday, etc :(
        for(var i = 0; i < 7; i++)
        {
            var daySum = await _dbc.ChatMessage
                .Where(m => m.Timestamp.Year == today.Year && m.Timestamp.Month == today.Month)
                .Where(m => m.Timestamp.DayOfWeek == (DayOfWeek)i).CountAsync();

            daySum /= GetOccurenceOfDay((DayOfWeek)i);
            avgs.Add(daySum);
        }

        return avgs;
    }

    [HttpGet("AvgActiveHours")]
    public async Task<List<int>> GetAverageActiveHours()
    {
        var today = DateTime.Now.Date;
        var avgs = new List<int>();
        var daysThisMonth = DateTime.DaysInMonth(today.Year, today.Month);

        for(var hour = 0; hour < 24; hour++)
        {
            var count = await _dbc.ChatMessage
                .Where(m => m.Timestamp.Year == today.Year && m.Timestamp.Month == today.Month)
                .Where(m => m.Timestamp.Hour == hour)
                .CountAsync();

            avgs.Add(count / daysThisMonth);
        }

        return avgs;
    }
}
