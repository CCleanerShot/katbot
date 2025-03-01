using Microsoft.AspNetCore.Mvc;
using skyblock_bot.Pages;

public abstract class SkyblockBaseModel : RootModel
{
    public SkyblockBaseModel(ILogger<RootModel> logger) : base(logger) { }

    public virtual async Task<IActionResult> CustomCustomGet()
    {
        return Page();
    }

    public sealed override async Task<IActionResult> CustomGet()
    {
        if (!Credentials.IsValid)
            return Redirect(UtilityP.Routes[RouteP.LOGIN]);
        else
            return await CustomCustomGet();
    }
}