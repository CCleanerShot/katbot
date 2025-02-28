using MongoDB.Driver;

public partial class AspnetRoutes
{
    [APIRoute]
    public void items()
    {
        WebApplication.MapPost("/items", async (HttpContext e) =>
        {
            FilterDefinition<BazaarItem> filter = new FilterDefinitionBuilder<BazaarItem>()
                .Where(e => e.UserId == 1);

            List<BazaarItem> result = await MongoBot.BazaarBuy.FindList(filter);

            if (result.Count == 0)
                return new List<BazaarItem>();

            return result;
        });
    }
}