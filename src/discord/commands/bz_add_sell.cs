using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_add_sell", "adds a bazaar item to the watchlist for selling")]
    public async Task bz_add_sell(
        [Summary("item", "the item to track (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(BazaarItemAutocomplete))] string itemID,
        [Summary("sell_price", "the minimum sell price (alerts if higher)")] ulong sellPrice,
        [Summary("order_type", "the type of order it is")] Enums.OrderType orderType,
        [Summary("remove_after", "whether or not to remove this item after it alerts the user")] bool removeAfter = true
    )
    {
        try
        {
            BazaarItem inserted;
            List<BazaarItem> response = (await MongoBot.BazaarSell.FindAsync(e => e.ID == itemID && e.UserId == Context.User.Id)).ToList();

            if (response.Count != 0)
            {
                inserted = response[0];
                inserted.Price = sellPrice;
                inserted.OrderType = orderType;
                inserted.RemovedAfter = removeAfter;
                await MongoBot.BazaarSell.ReplaceOneAsync(e => e._id == response[0]._id, inserted);
            }

            else
            {
                inserted = new BazaarItem()
                {
                    ID = itemID,
                    Name = MongoBot.CachedBazaarItems[itemID].Name,
                    Price = sellPrice,
                    OrderType = orderType,
                    UserId = Context.User.Id,
                    RemovedAfter = removeAfter,
                };

                await MongoBot.BazaarSell.InsertOneAsync(inserted);
            }

            if (MongoBot.CachedBazaarSells.Find(e => e.ID == inserted.ID && e.Name == inserted.Name) == null)
                MongoBot.CachedBazaarSells.Add(inserted);

            await RespondAsync($"OK! You will be alerted for '**{MongoBot.CachedBazaarItems[itemID].Name}**' if prices are **higher** than {sellPrice} ({orderType}).");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}