using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

public class BazaarBuyModel : BazaarTableModel
{
    public BazaarBuyModel(ILogger<BazaarBuyModel> logger) : base(logger) { }

    public override async Task<IActionResult> HTMX_Get()
    {
        FilterDefinition<BazaarItem> filter = new FilterDefinitionBuilder<BazaarItem>()
            .Where(e => e.UserId == 208963262094639104);

        List<BazaarItem> result = await MongoBot.BazaarBuy.FindList(filter);

        if (result.Count == 0)
            BazaarItems = new List<BazaarItem>();
        else
            BazaarItems = result;

        return Page();
    }

    public override async Task<IActionResult> HTMX_Post()
    {
        FilterDefinition<BazaarItem> filter = new FilterDefinitionBuilder<BazaarItem>()
            .Where(e => e.UserId == 208963262094639104);

        List<BazaarItem> result = await MongoBot.BazaarBuy.FindList(filter);

        if (result.Count == 0)
            BazaarItems = new List<BazaarItem>();
        else
            BazaarItems = result;

        return Page();
    }
}
