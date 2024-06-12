using System.Data.Common;
using ChatWatchApp.Data;
using ChatWatchApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWatchApp.Pages;

[Authorize]
public class ManagePlayerModel : PageModel
{
    private readonly ILogger<ManagePlayerModel> _logger;
    private readonly IUsernameService _username;
    private ApplicationDbContext _dbc;

    [FromQuery(Name = "id")]
    public Guid PlayerId { get; set; }

    [BindProperty]
    public string PlayerCustomName { get; set; } = "";

    public ManagePlayerModel(ILogger<ManagePlayerModel> logger, IUsernameService username, ApplicationDbContext dbc)
    {
        _logger = logger;
        _username = username;
        _dbc = dbc;
    }

    public IActionResult OnGet()
    {
        if(PlayerId == Guid.Empty) {
            _logger.LogWarning("given invalid player UUID!");
            return LocalRedirect("/Admin/PlayerList");
        }

        var player = _dbc.Player.FirstOrDefault(p => p.ID == PlayerId);
        if(player == null) {
            return BadRequest("Player does not exist!");
        }

        PlayerCustomName = player.CustomName;
        return Page();
    }

    public IActionResult OnPost()
    {
        var player = _dbc.Player.FirstOrDefault(p => p.ID == PlayerId);
        if(player == null) {
            return BadRequest("Player does not exist!");
        }

        if(player.CustomName != PlayerCustomName) {
            _logger.LogInformation("Updating {}'s custom name to {}", GetUsername(), PlayerCustomName);
            if(PlayerCustomName == null) PlayerCustomName = "";
            player.CustomName = PlayerCustomName;
            _dbc.SaveChanges();
        }

        return LocalRedirect(Request.GetEncodedPathAndQuery());
    }

    public string GetUsername() => _username.GetUsername(PlayerId);
}
