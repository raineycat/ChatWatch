using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWatchApp.Pages;

[Authorize]
public class DashAnalysisModel : PageModel
{
    private readonly ILogger<DashAnalysisModel> _logger;

    public DashAnalysisModel(ILogger<DashAnalysisModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        
    }
}
