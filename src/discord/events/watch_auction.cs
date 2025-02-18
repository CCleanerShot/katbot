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
    /// List of tracked auction buys that have already been alerted.
    /// </summary>
    List<AuctionBuy> WatchBuy_CachedAuctionBuyAlerts = new List<AuctionBuy>();

    [DiscordEvents]
    public void watch_auction()
    {
        _Timer.Elapsed += Watch_Auction_Elapsed;
    }

    async void Watch_Auction_Elapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {

        List<AuctionsRouteProduct>? liveItems = await AuctionsRoute.GetRoute();

        if (liveItems == null)
            return;

        List<AuctionBuy> elgibleBuys = (await MongoBot.AuctionBuy.FindAsync(e => MongoBot.CachedAuctionBuys.Any(ee => ee.ID == e.ID))).ToList();

        Dictionary<AuctionBuy, List<AuctionsRouteProduct>> matchingProducts = new Dictionary<AuctionBuy, List<AuctionsRouteProduct>>();

        void AddToMatchingProducts(AuctionBuy target, AuctionsRouteProduct source, Cyotek.Data.Nbt.Tag tag, bool fromCompound)
        {
            List<AuctionBuy.ExtraAttribute> similarAttr = target.ExtraAttributes.Where(targetAttr => source.NBT.ExistingTags.ContainsKey(targetAttr.Name)).ToList();

            switch (tag)
            {
                case TagCompound tagCompound:

                    if (target.ExtraAttributes.Any(e => e.Name == tagCompound.Name))
                        foreach (Cyotek.Data.Nbt.Tag innerTag in tagCompound.Value)
                            AddToMatchingProducts(target, source, innerTag, true);
                    break;
                case TagString tagString:
                    if (fromCompound)
                    {
                        if (target.ExtraAttributes.Any(e => e.Value == $"{tagString.Name} {tagString.Value}"))
                        {
                            if (!matchingProducts.ContainsKey(target))
                                matchingProducts.Add(target, new List<AuctionsRouteProduct>());

                            matchingProducts[target].Add(source);
                        }
                    }
                    else
                    {
                        if (target.ExtraAttributes.Any(e => e.Name == tagString.Name && e.Value == tagString.Value))
                        {
                            if (!matchingProducts.ContainsKey(target))
                                matchingProducts.Add(target, new List<AuctionsRouteProduct>());

                            matchingProducts[target].Add(source);
                        }
                    }
                    break;
                case TagInt tagInt:
                    if (fromCompound)
                    {
                        Regex regex = new Regex($"{tagInt.Name} [1-{tagInt.Value}]");
                        if (target.ExtraAttributes.Any(e => regex.Match(e.Value).Success))
                        {
                            if (!matchingProducts.ContainsKey(target))
                                matchingProducts.Add(target, new List<AuctionsRouteProduct>());

                            matchingProducts[target].Add(source);
                        }
                    }
                    else
                    {
                        if (target.ExtraAttributes.Any(e => e.Name == tagInt.Name && e.Value == tagInt.Value.ToString()))
                        {
                            if (!matchingProducts.ContainsKey(target))
                                matchingProducts.Add(target, new List<AuctionsRouteProduct>());

                            matchingProducts[target].Add(source);
                        }
                    }

                    if (target.ExtraAttributes.Any(e => e.Name == tagInt.Name && e.Value == tagInt.Value.ToString()))
                    {
                        if (!matchingProducts.ContainsKey(target))
                            matchingProducts.Add(target, new List<AuctionsRouteProduct>());

                        matchingProducts[target].Add(source);
                    }
                    break;
                default:
                    Program.Utility.Log(Enums.LogLevel.WARN, $"Unimplemented tag type at {tag.Type} (from {source.NBT.ID})!");
                    break;
            }
        }

        foreach (AuctionBuy trackedBuy in elgibleBuys)
        {
            // items where the ID of an item from the API matches the same ID as a tracked item
            List<AuctionsRouteProduct> similarLive = liveItems.Where(e => e.NBT.ID.Value == trackedBuy.ID).ToList();

            if (similarLive.Count == 0)
                continue;

            // check if all the properties are matching
            foreach (AuctionsRouteProduct item in similarLive)
                foreach (var (k, v) in item.NBT.ExistingTags)
                    AddToMatchingProducts(trackedBuy, item, v, false);
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
        List<AuctionBuy> allItems = [.. elgibleBuys];

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

        string response = "";

        // NOTE: contains unneeded o^2 notation, refactor if necessary.
        foreach (var (k, v) in cacheUsers)
        {
            response += $"<@{v.Id}>\n";
            response += "****BUYS****";

            foreach (var (wanted, liveProducts) in matchingProducts)
            {
                DiscordTable<AuctionTable> itemTable = new DiscordTable<AuctionTable>(wanted.Name);

                foreach (AuctionsRouteProduct product in liveProducts)
                {
                    itemTable.Table[AuctionTable.SELLER].Add(cachedSellers[product.auctioneer].Name);
                    itemTable.Table[AuctionTable.LIVE_PRICE].Add(MathF.Max(product.highest_bid_amount, product.starting_bid).ToString());

                    string liveProperties = "";

                    foreach (var (key, tag) in product.NBT.ExistingTags)
                    {
                        if (!wanted.ExtraAttributes.Select(e => e.Name).Contains(key))
                            continue;

                        if (tag is TagCompound tagCompound)
                        {
                            liveProperties += $"{tag.Name} [";
                            for (int i = 0; i < tagCompound.Value.Count; i++)
                            {
                                Cyotek.Data.Nbt.Tag innerTag = tagCompound.Value[i];
                                liveProperties += $"{innerTag.Name} {innerTag.GetValue()}";

                                if (i != tagCompound.Count - 1)
                                    liveProperties += ", ";
                            }
                        }

                        else
                        {
                            liveProperties += $"{tag.Name} {tag.GetValue()}";
                        }
                    }

                    itemTable.Table[AuctionTable.LIVE_PROPERTIES].Add(liveProperties);
                }

                response += itemTable.Construct();
            }
        }

        await channel.SendMessageAsync(response);

        // caching items + removing after for those who want to be removed
        List<AuctionBuy> toRemoveBuys = new List<AuctionBuy>();

        foreach (AuctionBuy tracked in elgibleBuys)
        {
            WatchBuy_CachedAuctionBuyAlerts.Add(tracked);

            if (tracked.RemovedAfter)
                toRemoveBuys.Add(tracked);
        }

        // NOTE: o^2 notation, refactor if needed.
        await MongoBot.AuctionBuy.DeleteManyAsync(e => toRemoveBuys.Select(buy => buy.ID).Contains(e.ID));
    }
}