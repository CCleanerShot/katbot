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
        _Timer.Elapsed += Watch_Auction_Elapsed;
    }

    async void Watch_Auction_Elapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {

        List<AuctionsRouteProduct>? liveItems = await AuctionsRoute.GetRoute(WatchBuy_CachedAuctionBuyAlerts);

        if (liveItems == null)
            return;

        List<AuctionBuy> elgibleBuys = (await MongoBot.AuctionBuy.FindAsync(e => MongoBot.CachedAuctionBuys.Any(ee => ee.ID == e.ID))).ToList();

        Dictionary<AuctionBuy, List<AuctionsRouteProduct>> matchingProducts = new Dictionary<AuctionBuy, List<AuctionsRouteProduct>>();

        bool AddToMatchingProducts(AuctionBuy target, AuctionsRouteProduct source)
        {
            if (WatchBuy_CachedAuctionBuyAlerts.ContainsKey(source.uuid))
                return false;

            bool TagIsValid(AuctionBuy.ExtraAttribute attribute, Cyotek.Data.Nbt.TagCompound? parentTag = null)
            {
                if (parentTag == null)
                {
                    if (!source.NBT.ExistingTags.ContainsKey(attribute.Name))
                        return false;

                    Cyotek.Data.Nbt.Tag tag = source.NBT.ExistingTags[attribute.Name];

                    switch (tag)
                    {
                        case TagCompound tagCompound:
                            string[] splitText = attribute.Value.Split(" ");
                            AuctionBuy.ExtraAttribute childAttribute = new AuctionBuy.ExtraAttribute(splitText[0], splitText[1]);
                            return TagIsValid(childAttribute, tagCompound);
                        case TagString tagString:
                            return tagString.Value == attribute.Value;
                        case TagInt tagInt:
                            Regex regex = new Regex($"[1-{tagInt.Value}]");
                            return regex.Match(tagInt.Value.ToString()).Success;
                        default:
                            Program.Utility.Log(Enums.LogLevel.WARN, $"Unimplemented tag type at {tag.Type} (from {source.NBT.ID})!");
                            return false;
                    }
                }

                else
                {
                    if (!parentTag.Value.Select(e => e.Name).Contains(attribute.Name))
                        return false;

                    Cyotek.Data.Nbt.Tag tag = parentTag.Value[attribute.Name];

                    switch (tag)
                    {
                        case TagString tagString:
                            return $"{tagString.Name} {tagString.Value}" == $"{attribute.Name} {attribute.Value}";
                        case TagInt tagInt:
                            Regex regex = new Regex($"{tagInt.Name} [1-{tagInt.Value}]");
                            return regex.Match($"{tagInt.Name} {tagInt.Value}").Success;
                        default:
                            Program.Utility.Log(Enums.LogLevel.WARN, $"Unimplemented tag type at {tag.Type} (from {source.NBT.ID})!");
                            return false;
                    }
                }

            }

            List<AuctionBuy.ExtraAttribute> similarAttr = target.ExtraAttributes.Where(targetAttr => source.NBT.ExistingTags.ContainsKey(targetAttr.Name)).ToList();

            if (target.ExtraAttributes.All(e => TagIsValid(e)))
                return true;
            else
                return false;
        }

        foreach (AuctionBuy trackedBuy in elgibleBuys)
        {
            // items where the ID of an item from the API matches the same ID as a tracked item
            List<AuctionsRouteProduct> similarLive = liveItems.Where(e => e.NBT.ID.Value == trackedBuy.ID).ToList();

            if (similarLive.Count == 0)
                continue;

            // check if all the properties are matching
            foreach (AuctionsRouteProduct item in similarLive)
            {
                foreach (var (k, v) in item.NBT.ExistingTags)
                {
                    if (AddToMatchingProducts(trackedBuy, item))
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

                            liveProperties += $"]";
                        }

                        else
                        {
                            liveProperties += $"{tag.Name} {tag.GetValue()}";
                        }
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

        foreach (AuctionBuy tracked in elgibleBuys)
            if (tracked.RemovedAfter)
                toRemoveBuys.Add(tracked);

        // NOTE: o^2 notation, refactor if needed.
        await MongoBot.AuctionBuy.DeleteManyAsync(e => toRemoveBuys.Select(buy => buy.ID).Contains(e.ID));
    }
}