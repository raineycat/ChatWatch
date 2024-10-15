using ChatWatchApp.Data;
using ChatWatchApp.Models;
using ChatWatchApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatWatchApp.Controllers;

[ApiController]
[Route("[controller]")]
public class HelperController : Controller
{
    private ApplicationDbContext _dbc;
    private IUsernameService _username;

    public HelperController(ApplicationDbContext dbc, IUsernameService username) 
    {
        _dbc = dbc;
        _username = username;
    }

    [HttpGet("UsernameFromUUID")]
    public async Task<IActionResult> GetDailyMessageCount([FromQuery] string uuid = ".")
    {
        if (!Guid.TryParse(uuid, out var actualId))
        {
            return BadRequest("Invalid UUID format!");
        }
        
        var name = await _username.GetUsernameAsync(actualId);
        return Ok(name);
    }
}