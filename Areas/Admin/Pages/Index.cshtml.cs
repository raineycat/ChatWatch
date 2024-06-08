using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWatchApp.Pages;

[Authorize]
public class AdminIndexModel : PageModel
{
    private readonly ILogger<AdminIndexModel> _logger;

    public AdminIndexModel(ILogger<AdminIndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        
    }
}
