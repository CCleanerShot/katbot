using System.Text;
using Microsoft.AspNetCore.Mvc;

public class BazaarBuyAddModel : AuthorizedModel
{
    public BazaarBuyAddModel(ILogger<BazaarBuyAddModel> logger) : base(logger) { }


    public override async Task<IActionResult> HTMX_Get()
    {
        return Page();
    }
}
