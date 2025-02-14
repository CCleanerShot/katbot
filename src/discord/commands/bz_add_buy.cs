using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_add_buy", "adds a bazaar item to the watchlist for buying")]
    public async Task bz_add_buy(
        [Summary("item", "the item to track (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(BazaarItemAutocomplete))] string itemID,
        [Summary("buy_price", "the maximum buy price (alerts if lower)")] ulong buyPrice,
        [Summary("order_type", "the type of order it is")] Enums.OrderType orderType,
        [Summary("remove_after", "whether or not to remove this item after it alerts the user")] bool removeAfter = true
    )
    {
        try
        {
            BazaarItem inserted;
            List<BazaarItem> response = (await MongoBot.BazaarBuy.FindAsync(e => e.ID == itemID && e.UserId == Context.User.Id)).ToList();

            if (response.Count != 0)
            {
                inserted = response[0];
                inserted.Price = buyPrice;
                inserted.OrderType = orderType;
                inserted.RemovedAfter = removeAfter;
                await MongoBot.BazaarBuy.ReplaceOneAsync(e => e._id == response[0]._id, inserted);
            }

            else
            {
                inserted = new BazaarItem()
                {
                    ID = itemID,
                    Name = MongoBot.CachedBazaarItems[itemID].Name,
                    Price = buyPrice,
                    OrderType = orderType,
                    UserId = Context.User.Id,
                    RemovedAfter = removeAfter,
                };

                await MongoBot.BazaarBuy.InsertOneAsync(inserted);
            }

            if (MongoBot.CachedBazaarBuys.Find(e => e.ID == inserted.ID && e.Name == inserted.Name) == null)
                MongoBot.CachedBazaarBuys.Add(inserted);

            await RespondAsync($"OK! You will be alerted for '**{MongoBot.CachedBazaarItems[itemID].Name}**' if prices are **lower** than {buyPrice} ({orderType}).");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}