using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;

enum BazaarTable
{
    NAME,
    LIVE_PRICE,
    WANTED_PRICE,
}

public partial class DiscordEvents
{
    /// <summary>
    /// List of tracked bazaar buys that have already been alerted.
    /// </summary>
    List<BazaarItem> WatchBuy_CachedBazaarBuyAlerts = new List<BazaarItem>();
    /// <summary>
    /// List of tracked bazaar sells that have already been alerted.
    /// </summary>
    List<BazaarItem> WatchSell_CachedBazaarSellAlerts = new List<BazaarItem>();

    [DiscordEvents]
    public void watch_bazaar()
    {
        _BazaarTimer.Elapsed += Watch_Bazaar_Elapsed;
    }

    async void Watch_Bazaar_Elapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {
        Dictionary<string, BazaarRouteProduct>? liveItems = await BazaarRoute.GetRoute();

        if (liveItems == null)
            return;

        if (liveItems.Count == 0)
            return;

        List<BazaarItem> trackedBuys = await MongoBot.BazaarBuy.FindList(e => MongoBot.CachedBazaarItems.Keys.Contains(e.ID));
        List<BazaarItem> trackedSells = await MongoBot.BazaarSell.FindList(e => MongoBot.CachedBazaarItems.Keys.Contains(e.ID));

        List<BazaarItem> eligibleBuys = trackedBuys.Where(t =>
        {
            if (WatchBuy_CachedBazaarBuyAlerts.Select(e => e.ID).Contains(t.ID))
                return false;

            switch (t.OrderType)
            {
                case Enums.OrderType.INSTA:
                    if (t.Price >= liveItems[t.ID].quick_status.buyPrice)
                        return true;
                    else
                        return false;
                case Enums.OrderType.ORDER:
                    if (t.Price >= liveItems[t.ID].sell_summary.First().pricePerUnit)
                        return true;
                    else
                        return false;
                default:
                    throw new NotImplementedException("Implement this");
            }
        }).ToList();

        List<BazaarItem> eligibleSells = trackedSells.Where(t =>
        {
            if (WatchSell_CachedBazaarSellAlerts.Select(e => e.ID).Contains(t.ID))
                return false;

            switch (t.OrderType)
            {
                case Enums.OrderType.INSTA:
                    if (t.Price <= liveItems[t.ID].quick_status.sellPrice)
                        return true;
                    else
                        return false;
                case Enums.OrderType.ORDER:
                    if (t.Price <= liveItems[t.ID].buy_summary.First().pricePerUnit)
                        return true;
                    else
                        return false;
                default:
                    throw new NotImplementedException("Implement this");
            }
        }).ToList();


        if (eligibleBuys.Count == 0 && eligibleSells.Count == 0)
            return;

        SocketTextChannel? channel = (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)) as SocketTextChannel;

        if (channel == null)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, "Attempted to send a message to the hypixel alerts channel, but it is missing!");
            return;
        }

        Dictionary<ulong, SocketGuildUser> cacheUsers = new Dictionary<ulong, SocketGuildUser>();

        List<BazaarItem> allItems = [.. eligibleBuys, .. eligibleSells];

        foreach (BazaarItem tracked in allItems)
        {
            if (!cacheUsers.Keys.Contains(tracked.UserId))
            {
                SocketGuildUser user = (await (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)).GetUserAsync(tracked.UserId) as SocketGuildUser)!;
                cacheUsers.Add(user.Id, user);
            }
        }

        string response = "";

        // NOTE: contains unneeded o^2 notation, refactor if necessary.
        foreach (var (k, v) in cacheUsers)
        {
            DiscordTable<BazaarTable> buyTable = new DiscordTable<BazaarTable>("BUYS");
            DiscordTable<BazaarTable> sellTable = new DiscordTable<BazaarTable>("SELLS");

            foreach (BazaarItem tracked in eligibleBuys.Where(e => e.UserId == k))
            {
                buyTable.Table[BazaarTable.NAME].Add(tracked.Name);
                buyTable.Table[BazaarTable.LIVE_PRICE].Add(liveItems[tracked.ID].sell_summary.First().pricePerUnit.ToString());
                buyTable.Table[BazaarTable.WANTED_PRICE].Add(tracked.Price.ToString());
            }

            foreach (BazaarItem tracked in eligibleSells.Where(e => e.UserId == k))
            {
                sellTable.Table[BazaarTable.NAME].Add(tracked.Name);
                sellTable.Table[BazaarTable.LIVE_PRICE].Add(liveItems[tracked.ID].buy_summary.First().pricePerUnit.ToString());
                sellTable.Table[BazaarTable.WANTED_PRICE].Add(tracked.Price.ToString());
            }

            string result = DiscordTable<BazaarTable>.ConstructCombine(buyTable, sellTable);
            response += $"<@{v.Id}>\n";
            response += result;
        }

        await channel.SendMessageAsync(response);

        // caching items + removing after for those who want to be removed
        List<BazaarItem> toRemoveBuys = new List<BazaarItem>();
        List<BazaarItem> toRemoveSells = new List<BazaarItem>();

        foreach (BazaarItem tracked in eligibleBuys)
        {
            WatchBuy_CachedBazaarBuyAlerts.Add(tracked);

            if (tracked.RemovedAfter)
                toRemoveBuys.Add(tracked);
        }

        foreach (BazaarItem tracked in eligibleSells)
        {
            WatchSell_CachedBazaarSellAlerts.Add(tracked);

            if (tracked.RemovedAfter)
                toRemoveSells.Add(tracked);

            WatchSell_CachedBazaarSellAlerts.Add(tracked);
        }

        // NOTE: o^2 notation, refactor if needed.
        await MongoBot.BazaarBuy.DeleteManyAsync(e => toRemoveBuys.Select(buy => buy.ID).Contains(e.ID));
        await MongoBot.BazaarSell.DeleteManyAsync(e => toRemoveSells.Select(sell => sell.ID).Contains(e.ID));

        Program.Utility.Log(Enums.LogLevel.NONE, "'watch_bazaar' ran successfully!");
    }
}