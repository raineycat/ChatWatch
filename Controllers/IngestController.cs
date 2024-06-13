using System.Data.SqlTypes;
using ChatWatchApp.Data;
using ChatWatchApp.Models;
using ChatWatchApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatWatchApp.Controllers;

[ApiController]
[Route("[controller]")]
public class IngestController : ControllerBase
{
    private readonly ApplicationDbContext _dbc;
    private readonly IServerSettings _settings;

    [FromQuery]
    public Guid IngestToken { get; set; } = Guid.Empty;

    public IngestController(ApplicationDbContext dbc, IServerSettings iss) 
    {
        _dbc = dbc;
        _settings = iss;
    }

    [HttpPost("Chat")]
    public async Task<IActionResult> IngestChatMessage([FromBody] ChatMessage msg) 
    {
        if(IngestToken != _settings.IngestToken) {
            return Unauthorized();
        }

        var sender = await _dbc.Player.FindAsync(msg.Sender.ID);
        if(sender != null) {
            _dbc.Entry(sender).State = EntityState.Unchanged;
            msg.Sender = sender;
        }

        var row = await _dbc.ChatMessage.AddAsync(msg);
        await IngestFlush();
        return Ok(row.Entity.ID);
    }

    [HttpPost("Private")]
    public async Task<IActionResult> IngestPrivateMessage([FromBody] PrivateMessage msg) 
    {
        if(IngestToken != _settings.IngestToken) {
            return Unauthorized();
        }

        var sender = await _dbc.Player.FindAsync(msg.Sender.ID);
        if(sender != null) {
            _dbc.Entry(sender).State = EntityState.Unchanged;
            msg.Sender = sender;
        }

        var recip = await _dbc.Player.FindAsync(msg.Recipient.ID);
        if(recip != null) {
            _dbc.Entry(recip).State = EntityState.Unchanged;
            msg.Recipient = recip;
        }

        var row = await _dbc.PrivateMessage.AddAsync(msg);
        await IngestFlush();
        return Ok(row.Entity.ID);
    }

    [HttpPost("Flush")]
    public async Task<IActionResult> IngestFlush()
    {
        if(IngestToken != _settings.IngestToken) {
            return Unauthorized();
        }

        var count = await _dbc.SaveChangesAsync();
        return Ok(count);
    }
}
