using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_check", "checks your current bazaar watchlist")]
    public async Task bz_check()
    {
        try
        {
            string result = "";
            List<BazaarItem> buys = MongoBot.CachedBuys.Where(e => e.UserId == Context.User.Id).ToList();
            List<BazaarItem> sells = MongoBot.CachedSells.Where(e => e.UserId == Context.User.Id).ToList();
            Dictionary<string, BazaarRouteProduct>? products = await BazaarRoute.GetRoute();

            foreach (BazaarItem buy in buys)
            {
                float? livePrice = products?[buy.ID].sell_summary.First().pricePerUnit;

                if (livePrice != null)
                    livePrice = MathF.Round((float)livePrice);

                result += $"(LIVE BUY PRICE: {livePrice?.ToString() ?? "N/A"}) **'{buy.Name}'** at {buy.Price} ({buy.OrderType})\n";
            }

            foreach (BazaarItem sell in sells)
            {
                float? livePrice = products?[sell.ID].buy_summary.First().pricePerUnit;

                if (livePrice != null)
                    livePrice = MathF.Round((float)livePrice);

                result += $"(LIVE SELL PRICE: {livePrice}) **'{sell.Name}'** at {sell.Price} ({sell.OrderType})\n";
            }

            await RespondAsync(result);
        }

        catch (Exception e)
        {
            Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}