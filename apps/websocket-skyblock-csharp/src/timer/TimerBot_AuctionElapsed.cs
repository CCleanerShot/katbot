public static partial class TimerBot
{
    static async void _AuctionElapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {

        List<AuctionsRouteProduct>? liveItems = await AuctionsRoute.GetRoute();

        if (liveItems == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Failed to fetch live items from the auction!");
            return;
        }

        List<AuctionBuy> allBuys = await MongoBot.AuctionBuy.FindList(e => true);
        List<AuctionsRouteProduct> similarLive = new List<AuctionsRouteProduct>();

        // so we dont have to check literally every single possible item later here
        foreach (AuctionsRouteProduct item in liveItems)
            if (allBuys.Find(e => e.ID == item.ITEM_ID) != null)
                similarLive.Add(item);


        // NOTE: we dont reset the list like we do in bazaar, because sometimes the fetch is only for the latest 'x' pages, which needs the old items.
        foreach (var (k, v) in MongoBot.EligibleAuctionBuys)
        {
            foreach (AuctionBuy buy in allBuys)
            {
                // check if any are currently gone
                if (allBuys.Where(e => e._id == buy._id).Count() == 0)
                {
                    v.Remove(buy);
                    continue;
                }

                foreach (AuctionsRouteProduct product in similarLive)
                {
                    bool condition1 = buy.Price <= MathF.Max(product.starting_bid, product.highest_bid_amount);
                    bool condition2 = buy.AuctionTags.All(e => product.AuctionTags.Any(ee =>
                    {
                        if (ee.Name != e.Name)
                            return false;

                        switch (ee.Name)
                        {
                            case "attributes":
                                return ee >= e;
                            default:
                                return ee == e;
                        }
                    }));

                    // check if all the properties are matching
                    if (condition1 && condition2)
                    {
                        if (!MongoBot.EligibleAuctionBuys[buy.UserId].ContainsKey(buy))
                            MongoBot.EligibleAuctionBuys[buy.UserId].Add(buy, new AuctionSocketMessage(new List<AuctionsRouteProductMinimal>(), buy));

                        if (MongoBot.EligibleAuctionBuys[buy.UserId][buy].LiveItems.Where(e => e.uuid == product.uuid).Count() == 0)
                            MongoBot.EligibleAuctionBuys[buy.UserId][buy].LiveItems.Add(product);
                    }

                    // check if this is a product that existed and is no longer eligible
                    else
                    {
                        if (!MongoBot.EligibleAuctionBuys[buy.UserId].ContainsKey(buy))
                            continue;

                        if (MongoBot.EligibleAuctionBuys[buy.UserId][buy].LiveItems.Where(e => e.uuid == product.uuid).Count() == 1)
                            MongoBot.EligibleAuctionBuys[buy.UserId][buy].LiveItems.Remove(product);
                    }
                }
            }
        }

        Utility.LogPerformance("Finished Matching Products");
        WebSocketBot.SendAuctionData();
    }
}