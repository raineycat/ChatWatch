using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ChatWatchApp.Data;
using ChatWatchApp.Models;
using Microsoft.AspNetCore.Authorization;
using ChatWatchApp.Services;

namespace ChatWatchApp.Pages;

[Authorize]
public class DashLogModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IUsernameService _username;

    public DashLogModel(ApplicationDbContext context, IUsernameService username)
    {
        _context = context;
        _username = username;
    }

    public List<EventLogEntry> Entries { get; set; } = new();

    [FromQuery]
    public DateOnly SelectedDay { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public async Task OnGetAsync()
    {
        var selectedDT = SelectedDay.ToDateTime(TimeOnly.MinValue);
        var chatMessages = await _context.ChatMessage
            .Where(m => m.Timestamp.Date == selectedDT)
            .Include(m => m.Sender)
            .ToListAsync();
        Entries.AddRange(chatMessages.Select(m => new EventLogEntry(m)));

        var privateMessages = await _context.PrivateMessage
            .Where(m => m.Timestamp.Date == selectedDT)
            .Include(m => m.Sender)
            .Include(m => m.Recipient)
            .ToListAsync();
        Entries.AddRange(privateMessages.Select(m => new EventLogEntry(m)));

        Entries = Entries.OrderByDescending(e => e.Data.Timestamp).ToList();
    }

    public string FormatName(Player p) {
        if(string.IsNullOrWhiteSpace(p.CustomName)) {
            return _username.GetUsername(p);
        }
        return $"{p.CustomName} ({_username.GetUsername(p)})";
    }
}

public class EventLogEntry
{
    public enum EntryType {
        ChatMessage,
        PrivateMessage
    }

    public EventLogEntry(ChatMessage m) {
        Type = EntryType.ChatMessage;
        Data = m;
    }

    public EventLogEntry(PrivateMessage m) {
        Type = EntryType.PrivateMessage;
        Data = m;
    }

    public EntryType Type { get; set; }
    public IMessage Data { get; set; }
}
