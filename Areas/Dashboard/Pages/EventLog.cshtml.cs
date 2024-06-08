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

namespace ChatWatchApp.Pages;

[Authorize]
public class DashLogModel : PageModel
{
    private readonly ChatWatchApp.Data.ApplicationDbContext _context;

    public DashLogModel(ChatWatchApp.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public List<EventLogEntry> Entries { get; set; } = new();

    public async Task OnGetAsync()
    {
        var chatMessages = await _context.ChatMessage.Include(m => m.Sender).ToListAsync();
        Entries.AddRange(chatMessages.Select(m => new EventLogEntry(m)));

        var privateMessages = await _context.PrivateMessage.Include(m => m.Sender).Include(m => m.Recipient).ToListAsync();
        Entries.AddRange(privateMessages.Select(m => new EventLogEntry(m)));

        Entries = Entries.OrderByDescending(e => e.Data.Timestamp).Take(150).ToList();
    }

    public string FormatName(Player p) {
        if(string.IsNullOrWhiteSpace(p.CustomName)) {
            return p.Username;
        }
        return $"{p.CustomName} ({p.Username})";
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
