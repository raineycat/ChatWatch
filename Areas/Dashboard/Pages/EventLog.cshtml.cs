using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWatchApp.Pages;

[Authorize]
public class DashLogModel : PageModel
{
    private readonly ILogger<DashLogModel> _logger;

    public DashLogModel(ILogger<DashLogModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        
    }
}
