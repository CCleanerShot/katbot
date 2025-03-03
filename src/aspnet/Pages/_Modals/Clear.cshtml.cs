
using Microsoft.AspNetCore.Mvc;

public class ClearModel : AuthorizedModel
{
    public ClearModel(ILogger<ClearModel> logger) : base(logger) { }

    public override async Task<IActionResult> HTMX_Get()
    {
        return Page();
    }
}
