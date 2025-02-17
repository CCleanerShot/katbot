using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cyotek.Data.Nbt;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;

public partial class DiscordEvents
{
    /// <summary>
    /// List of tracked auction buys that have already been alerted.
    /// </summary>
    List<AuctionBuy> WatchBuy_CachedAuctionBuyAlerts = new List<AuctionBuy>();

    [DiscordEvents]
    public void watch_auction()
    {
        // _Timer.Elapsed += Watch_Auction_Elapsed;
    }

    async void Watch_Auction_Elapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {

        List<AuctionsRouteProduct>? liveItems = await AuctionsRoute.GetRoute();

        if (liveItems == null)
            return;

        List<AuctionBuy> trackedBuys = (await MongoBot.AuctionBuy.FindAsync(e => MongoBot.CachedAuctionBuys.Any(ee => ee.ID == e.ID))).ToList();

        Dictionary<AuctionBuy, List<AuctionsRouteProduct>> matchingProducts = new Dictionary<AuctionBuy, List<AuctionsRouteProduct>>();

        void AddToTrackedBuys(AuctionBuy target, AuctionsRouteProduct source, Cyotek.Data.Nbt.Tag tag, bool fromCompound)
        {
            List<AuctionBuy.ExtraAttribute> similarAttr = target.ExtraAttributes.Where(targetAttr => source.NBT.ExistingTags.Select(e => e.Name).Contains(targetAttr.Name)).ToList();

            switch (tag)
            {
                case TagCompound tagCompound:

                    if (target.ExtraAttributes.Any(e => e.Name == tagCompound.Name))
                        foreach (Cyotek.Data.Nbt.Tag innerTag in tagCompound.Value)
                            AddToTrackedBuys(target, source, innerTag, true);
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

        foreach (AuctionBuy trackedBuy in trackedBuys)
        {
            // items where the ID of an item from the API matches the same ID as a tracked item
            List<AuctionsRouteProduct> similarLive = liveItems.Where(e => e.NBT.ID.Value == trackedBuy.ID).ToList();

            if (similarLive.Count == 0)
                continue;

            // check if all the properties are matching
            foreach (AuctionsRouteProduct item in similarLive)
                foreach (Cyotek.Data.Nbt.Tag tag in item.NBT.ExistingTags)
                    AddToTrackedBuys(trackedBuy, item, tag, false);
        }

        if (matchingProducts.Count == 0)
            return;

        SocketTextChannel? channel = (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)) as SocketTextChannel;

        if (channel == null)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, "Attempted to send a message to the hypixel alerts channel, but it is missing!");
            return;
        }

        int maxName = "NAME".Length;
        int maxPrice = "LIVE_PRICE".Length;
        Dictionary<ulong, SocketGuildUser> cacheUsers = new Dictionary<ulong, SocketGuildUser>();

        // CHECKING MAX BUY STRING LENGTHS
        foreach (var (tracked, matchingItems) in matchingProducts)
        {
            int itemMaxPrice = (int)matchingItems.Max(e => MathF.Max(e.starting_bid, e.highest_bid_amount));

            if (maxName < tracked.Name.Length)
                maxName = tracked.Name.Length;
            if (maxPrice < itemMaxPrice)
                maxPrice = itemMaxPrice;

            SocketGuildUser user;
            // preventing spam to discord
            if (!cacheUsers.Keys.Contains(tracked.UserId))
            {
                user = (await (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)).GetUserAsync(tracked.UserId) as SocketGuildUser)!;
                cacheUsers.Add(user.Id, user);
            }
        }

        string response = "";

        // // NOTE: contains unneeded o^2 notation, refactor if necessary.
        // foreach (var (k, v) in cacheUsers)
        // {
        //     response += $"<@{v.Id}>\n";
        //     response += $"```\n";

        //     response += $"{"**BUYS**"}\n";
        //     List<AuctionBuy> buys = matchingProducts.Where(e => e.UserId == k).ToList();
        //     response += $"{Program.Utility.SS("NAME", maxName)}|{Program.Utility.SS("LIVE_PRICE", maxPrice)}|WANTED_PRICE\n";
        //     foreach (AuctionBuy tracked in buys)
        //     {
        //         string name = Program.Utility.SS(tracked.Name, maxName);
        //         string livePrice = Program.Utility.SS(liveItems[tracked.ID].sell_summary.First().pricePerUnit.ToString(), maxPrice);
        //         string wantedPrice = tracked.Price.ToString();
        //         response += $"{name}|{livePrice}|{wantedPrice}\n";
        //     }

        //     response += $"```\n";
        // }

        await channel.SendMessageAsync(response);

        // caching items + removing after for those who can
        List<AuctionBuy> toRemoveBuys = new List<AuctionBuy>();
        List<AuctionBuy> toRemoveSells = new List<AuctionBuy>();

        foreach (var (k, v) in matchingProducts)
        {
            WatchBuy_CachedAuctionBuyAlerts.Add(k);

            if (k.RemovedAfter)
                toRemoveBuys.Add(k);
        }

        // NOTE: o^2 notation, refactor if needed.
        await MongoBot.AuctionBuy.DeleteManyAsync(e => toRemoveBuys.Select(buy => buy.ID).Contains(e.ID));
    }
}