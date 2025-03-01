using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

public class SkyblockBazaarModel : SkyblockBaseModel
{
    public List<BazaarItem> BazaarBuy = new List<BazaarItem>();
    public List<BazaarItem> BazaarItems = new List<BazaarItem>();
    public List<BazaarItem> BazaarSell = new List<BazaarItem>();
    public string Test = "b";
    public SkyblockBazaarModel(ILogger<SkyblockBazaarModel> logger) : base(logger) { }

    public override async Task<IActionResult> HTMX_Post()
    {
        await Task.Delay(1000);
        FilterDefinition<BazaarItem> filter = new FilterDefinitionBuilder<BazaarItem>()
            .Where(e => e.UserId == 208963262094639104);

        List<BazaarItem> result = await MongoBot.BazaarBuy.FindList(filter);

        if (result.Count == 0)
            BazaarBuy = new List<BazaarItem>();
        else
            BazaarBuy = result;

        Test += "a";

        return Content("asdasd");
    }
}
