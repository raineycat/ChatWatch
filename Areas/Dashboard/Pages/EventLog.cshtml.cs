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

    public IList<ChatMessage> Messages { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Messages = await _context.ChatMessage.Include(m => m.Sender).ToListAsync();
        Messages = Messages.OrderByDescending(m => m.Timestamp).Take(25).ToList();
    }
}
