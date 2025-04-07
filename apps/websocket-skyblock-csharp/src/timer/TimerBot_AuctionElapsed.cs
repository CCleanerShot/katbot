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

        foreach (var (k, v) in MongoBot.ElgibleAuctionBuys)
            MongoBot.ElgibleAuctionBuys[k] = new Dictionary<AuctionBuy, AuctionItemsWithBuy>();

        foreach (var (k, v) in MongoBot.ElgibleAuctionBuys)
        {
            foreach (AuctionBuy buy in allBuys)
            {
                // items where the ID of an item from the API matches the same ID as a tracked item

                if (similarLive.Count == 0)
                    continue;

                // check if all the properties are matching
                foreach (AuctionsRouteProduct item in similarLive)
                    foreach (AuctionTag attribute in item.AuctionTags)
                        if (buy.AuctionTags.All(attribute => item.AuctionTags.Any(e => e == attribute)))
                        {
                            if(!MongoBot.ElgibleAuctionBuys[buy.UserId].ContainsKey(buy))
                                MongoBot.ElgibleAuctionBuys[buy.UserId].Add(buy, new AuctionItemsWithBuy(new List<AuctionsRouteProductMinimal>(), buy));

                            MongoBot.ElgibleAuctionBuys[buy.UserId][buy].LiveItems.Add(item);
                        }
            }
        }

        Utility.LogPerformance("Finished Matching Products");
        WebSocketBot.SendAuctionData();
    }
}