using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace skyblock_bot.Pages;

public class DiscordModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public DiscordModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (Request.IsHtmx())
            return Content("hello");
        else
            return Page();
    }
}
