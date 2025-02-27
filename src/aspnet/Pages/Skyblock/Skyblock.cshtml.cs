using System.Threading.Tasks;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace skyblock_bot.Pages;

public class SkyblockModel : PageModel
{
    public List<BazaarItem> BazaarItems = new List<BazaarItem>();
    private readonly ILogger<IndexModel> _logger;

    public SkyblockModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (Request.IsHtmx())
        {
            return Content("hello");
        }
        else
        {
            return Page();
        }
    }
}
