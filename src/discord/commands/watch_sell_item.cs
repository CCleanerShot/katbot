using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("watch_sell_item", "adds a bazaar item to the watchlist for selling")]
    public async Task watch_sell_item(
        [Summary("item", "the item to track (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(ItemAutocomplete))] string itemID,
        [Summary("sell_price", "the maximum sell price (alerts if below)")] ulong sellPrice,
        [Summary("order_type", "the type of order it is")] Enums.OrderType orderType
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

                await MongoBot.BazaarSell.ReplaceOneAsync(e => e._id == response[0]._id, inserted);
            }

            else
            {
                inserted = new BazaarItem()
                {
                    ID = itemID,
                    Name = MongoBot.CachedItems[itemID].Name,
                    Price = sellPrice,
                    OrderType = orderType,
                    UserId = Context.User.Id,
                };

                await MongoBot.BazaarSell.InsertOneAsync(inserted);
            }

            if (MongoBot.CachedSells.Find(e => e.ID == inserted.ID && e.Name == inserted.Name) == null)
                MongoBot.CachedSells.Add(inserted);

            await RespondAsync($"OK! You will be alerted for '**{MongoBot.CachedItems[itemID].Name}**' if prices are **lower** than {sellPrice} ({orderType}).");
        }

        catch (Exception)
        {
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}