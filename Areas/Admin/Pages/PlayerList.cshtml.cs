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
public class PlayerListModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IUsernameService _username;

    public List<Player> Players { get; set; }

    public PlayerListModel(ApplicationDbContext context, IUsernameService username)
    {
        _context = context;
        _username = username;
    }

    public async Task OnGetAsync()
    {
        Players = await _context.Player.ToListAsync();
    }

    public string FormatName(Player p) {
        if(string.IsNullOrWhiteSpace(p.CustomName)) {
            return _username.GetUsername(p);
        }
        return $"{p.CustomName} ({_username.GetUsername(p)})";
    }
}
