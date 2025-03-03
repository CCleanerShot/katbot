using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace skyblock_bot.Pages;

public class SkyblockModel : AuthorizedModel
{
    public SkyblockModel(ILogger<SkyblockModel> logger) : base(logger) { }

    public sealed override async Task<IActionResult> CustomCustomGet()
    {
        return Redirect(UtilityP.Routes[RouteP.SKYBLOCK_BAZAAR]);
    }
}
