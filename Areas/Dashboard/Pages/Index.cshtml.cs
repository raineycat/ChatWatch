using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWatchApp.Pages;

[Authorize]
public class DashIndexModel : PageModel
{
    private readonly ILogger<DashIndexModel> _logger;

    public DashIndexModel(ILogger<DashIndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        
    }
}
