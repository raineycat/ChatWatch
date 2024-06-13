using ChatWatchApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWatchApp.Pages;

[Authorize]
public class AdminIndexModel : PageModel
{
    private readonly ILogger<AdminIndexModel> _logger;

    [BindProperty]
    public string ServerName { get; set; }
    [BindProperty]
    public string IngestToken { get; set; }

    public AdminIndexModel(ILogger<AdminIndexModel> logger, IServerSettings settings)
    {
        _logger = logger;
        ServerName = settings.ServerName;
        IngestToken = settings.IngestToken.ToString();
    }

    public void OnGet()
    {
        
    }
}
