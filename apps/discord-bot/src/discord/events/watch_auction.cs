using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cyotek.Data.Nbt;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;

struct Auctioneer
{
    public string Name;
    public string ID;
}

enum AuctionTable
{
    SELLER,
    LIVE_PRICE,
    LIVE_PROPERTIES,
}

public partial class DiscordEvents
{
    /// <summary>
    /// List of tracked auction IDs that have already been alerted for.
    /// </summary>
    Dictionary<string, double?> WatchBuy_CachedAuctionBuyAlerts = new Dictionary<string, double?>();

    [DiscordEvents]
    public void watch_auction()
    {
        _AuctionTimer.Elapsed += Watch_Auction_Elapsed;
    }

    async void Watch_Auction_Elapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {
        List<AuctionsRouteProduct>? liveItems = await AuctionsRoute.GetRoute(WatchBuy_CachedAuctionBuyAlerts, 100);

        if (liveItems == null)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, "Failed to fetch live items from the auction!");
            return;
        }

        List<AuctionBuy> eligibleBuys = await MongoBot.AuctionBuy.FindList(e => MongoBot.CachedAuctionBuys.Any(ee => ee.ID == e.ID));
        Dictionary<AuctionBuy, List<AuctionsRouteProduct>> matchingProducts = new Dictionary<AuctionBuy, List<AuctionsRouteProduct>>();

        foreach (AuctionBuy trackedBuy in eligibleBuys)
        {
            // items where the ID of an item from the API matches the same ID as a tracked item
            List<AuctionsRouteProduct> similarLive = liveItems.Where(e => e.ITEM_ID == trackedBuy.ID).ToList();

            if (similarLive.Count == 0)
                continue;

            // check if all the properties are matching
            foreach (AuctionsRouteProduct item in similarLive)
            {
                foreach (AuctionTag attribute in item.AuctionTags)
                {
                    if (trackedBuy.AuctionTags.All(attribute => item.AuctionTags.Any(e => e == attribute)))
                    {
                        if (!matchingProducts.ContainsKey(trackedBuy))
                            matchingProducts.Add(trackedBuy, new List<AuctionsRouteProduct>());

                        matchingProducts[trackedBuy].Add(item);
                    }
                }
            }
        }

        if (matchingProducts.Count == 0)
            return;

        SocketTextChannel? channel = (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)) as SocketTextChannel;

        if (channel == null)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, "Attempted to send a message to the hypixel alerts channel, but it is missing!");
            return;
        }

        Dictionary<string, Auctioneer> cachedSellers = new Dictionary<string, Auctioneer>();
        Dictionary<ulong, SocketGuildUser> cacheUsers = new Dictionary<ulong, SocketGuildUser>();
        List<AuctionBuy> allItems = [.. eligibleBuys];

        foreach (var (k, v) in matchingProducts)
        {

            foreach (AuctionsRouteProduct product in v)
            {
                if (!cachedSellers.ContainsKey(product.auctioneer))
                {
                    string id = product.auctioneer;

                    PlayerRoutePlayer? player = await PlayerRoute.GetRoute(id);

                    if (player == null)
                    {
                        Program.Utility.Log(Enums.LogLevel.WARN, $"Received null when expected a player ({id})!");
                        return;
                    }

                    cachedSellers.Add(id, new Auctioneer() { Name = player.displayname, ID = id });
                }
            }
        }

        foreach (AuctionBuy tracked in allItems)
        {
            if (!cacheUsers.Keys.Contains(tracked.UserId))
            {
                SocketGuildUser user = (await (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)).GetUserAsync(tracked.UserId) as SocketGuildUser)!;
                cacheUsers.Add(user.Id, user);
            }
        }


        List<string> responses = new List<string>();
        // NOTE: contains unneeded o^2 notation, refactor if necessary.
        foreach (var (k, v) in cacheUsers)
        {
            foreach (var (wanted, liveProducts) in matchingProducts)
            {
                string response = "";
                response += $"<@{v.Id}>\n";
                response += "****BUYS****";
                DiscordTable<AuctionTable> itemTable = new DiscordTable<AuctionTable>(wanted.Name);

                foreach (AuctionsRouteProduct product in liveProducts)
                {
                    itemTable.Table[AuctionTable.SELLER].Add(cachedSellers[product.auctioneer].Name);
                    itemTable.Table[AuctionTable.LIVE_PRICE].Add(MathF.Max(product.highest_bid_amount, product.starting_bid).ToString());

                    string liveProperties = "";

                    foreach (AuctionTag attribute in product.AuctionTags)
                    {
                        if (!wanted.AuctionTags.Select(e => e.Name).Contains(attribute.Name))
                            continue;

                        liveProperties += $"{attribute.Name} {attribute.Value}";
                    }

                    itemTable.Table[AuctionTable.LIVE_PROPERTIES].Add(liveProperties);
                }

                response += itemTable.Construct();
                responses.Add(response);
            }
        }

        foreach (string response in responses)
            await channel.SendMessageAsync(response);

        // caching items + removing after for those who want to be removed
        List<AuctionBuy> toRemoveBuys = new List<AuctionBuy>();

        foreach (AuctionBuy tracked in eligibleBuys)
            if (tracked.RemovedAfter)
                toRemoveBuys.Add(tracked);

        // NOTE: o^2 notation, refactor if needed.
        await MongoBot.AuctionBuy.DeleteManyAsync(e => toRemoveBuys.Select(buy => buy.ID).Contains(e.ID));

        Program.Utility.Log(Enums.LogLevel.NONE, "'watch_auction' ran successfully!");
    }
}