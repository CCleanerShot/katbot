using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("check_watchlist", "checks your current bazaar watchlist")]
    public async Task check_watchlist()
    {
        try
        {
            List<BazaarItem> buys = MongoBot.CachedBuys.Where(e => e.UserId == Context.User.Id).ToList();
            List<BazaarItem> sells = MongoBot.CachedSells.Where(e => e.UserId == Context.User.Id).ToList();
            string result = "";

            foreach (BazaarItem buy in buys)
                result += $"WAITING TO BUY **'{buy.Name}'** for {buy.Price} ({buy.OrderType})\n";
            foreach (BazaarItem sell in sells)
                result += $"WAITING TO SELL **'{sell.Name}'** for {sell.Price} ({sell.OrderType})\n";

            await RespondAsync(result);
        }

        catch (Exception)
        {
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}