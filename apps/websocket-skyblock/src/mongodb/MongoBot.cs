using MongoDB.Driver;
using System.Text.Json.Nodes;

public class MongoBot
{
    /// <summary>
    /// A check for whether or not auction buys have been recently updated. Modified after MongoDB calls for Add/Update requests on auction buys.
    /// </summary>
    public static bool AuctionBuysRecentlyUpdated = false;

    private static MongoClient _Client = default!;
    private static IMongoDatabase _DiscordDB = default!;
    private static IMongoDatabase _GeneralDB = default!;
    private static IMongoDatabase _HypixelDB = default!;
    private static string _Uri = default!;

    public static Dictionary<string, AuctionItemsAll> CachedAuctionItems = new Dictionary<string, AuctionItemsAll>();
    public static Dictionary<string, AuctionTags> CachedAuctionTags = new Dictionary<string, AuctionTags>();
    public static Dictionary<string, BazaarItemsAll> CachedBazaarItems = new Dictionary<string, BazaarItemsAll>();
    public static Dictionary<ulong, Dictionary<AuctionBuy, AuctionSocketMessage>> EligibleAuctionBuys = new Dictionary<ulong, Dictionary<AuctionBuy, AuctionSocketMessage>>();
    public static Dictionary<ulong, List<BazaarSocketMessage>> EligibleBazaarBuys = new Dictionary<ulong, List<BazaarSocketMessage>>();
    public static Dictionary<ulong, List<BazaarSocketMessage>> EligibleBazaarSells = new Dictionary<ulong, List<BazaarSocketMessage>>();

    public static IMongoCollection<AuctionBuy> AuctionBuy { get; protected set; } = default!;
    public static IMongoCollection<AuctionItemsAll> AuctionItemsAll { get; protected set; } = default!;
    public static IMongoCollection<AuctionTags> AuctionTags { get; protected set; } = default!;
    public static IMongoCollection<BazaarItem> BazaarBuy { get; protected set; } = default!;
    public static IMongoCollection<BazaarItemsAll> BazaarItemsAll { get; protected set; } = default!;
    public static IMongoCollection<BazaarItem> BazaarSell { get; protected set; } = default!;
    public static IMongoCollection<MongoUser> MongoUser { get; protected set; } = default!;
    public static IMongoCollection<Session> Session { get; protected set; } = default!;
    public static IMongoCollection<Starboards> Starboards { get; protected set; } = default!;
    public static IMongoCollection<RollStats> RollStats { get; protected set; } = default!;


    public static async Task Load()
    {
        try
        {
            _Uri = Settings.MONGODB_BASE_URI + Settings.MONGODB_OPTIONS;
            _Client = new MongoClient(_Uri);
            _DiscordDB = _Client.GetDatabase(Settings.MONGODB_D_DISCORD);
            _GeneralDB = _Client.GetDatabase(Settings.MONGODB_D_GENERAL);
            _HypixelDB = _Client.GetDatabase(Settings.MONGODB_D_HYPIXEL);
            AuctionBuy = _HypixelDB.GetCollection<AuctionBuy>(Settings.MONGODB_C_AUCTION_BUY);
            AuctionItemsAll = _HypixelDB.GetCollection<AuctionItemsAll>(Settings.MONGODB_C_AUCTION_ITEMS);
            AuctionTags = _HypixelDB.GetCollection<AuctionTags>(Settings.MONGODB_C_AUCTION_TAGS);
            BazaarBuy = _HypixelDB.GetCollection<BazaarItem>(Settings.MONGODB_C_BAZAAR_BUY);
            BazaarSell = _HypixelDB.GetCollection<BazaarItem>(Settings.MONGODB_C_BAZAAR_SELL);
            BazaarItemsAll = _HypixelDB.GetCollection<BazaarItemsAll>(Settings.MONGODB_C_BAZAAR_ITEMS);
            MongoUser = _GeneralDB.GetCollection<MongoUser>(Settings.MONGODB_C_USERS);
            Session = _GeneralDB.GetCollection<Session>(Settings.MONGODB_C_SESSIONS);
            Starboards = _DiscordDB.GetCollection<Starboards>(Settings.MONGODB_C_STARBOARDS);
            RollStats = _DiscordDB.GetCollection<RollStats>(Settings.MONGODB_C_ROLL_STATS);

            List<AuctionBuy> currentAuctionBuy = await AuctionBuy.FindList(e => true);
            List<AuctionItemsAll> currentAuctionItems = await AuctionItemsAll.FindList(e => true);
            List<AuctionTags> currentAuctionTags = await AuctionTags.FindList(e => true);
            List<BazaarItem> currentBazaarBuys = await BazaarBuy.FindList(e => true);
            List<BazaarItemsAll> currentBazaarItems = await BazaarItemsAll.FindList(e => true);
            List<BazaarItem> currentSells = await BazaarSell.FindList(e => true);

            CachedAuctionItems = currentAuctionItems.Aggregate(new Dictionary<string, AuctionItemsAll>(), (pV, cV) => { pV.Add(cV.ID, cV); return pV; });
            CachedAuctionTags = currentAuctionTags.Aggregate(new Dictionary<string, AuctionTags>(), (pV, cV) => { pV.Add(cV.Name, cV); return pV; });
            CachedBazaarItems = currentBazaarItems.Aggregate(new Dictionary<string, BazaarItemsAll>(), (pV, cV) => { pV.Add(cV.ID, cV); return pV; });

            // setup ids for elgible users
            List<MongoUser> users = await MongoUser.FindList(e => true);

            foreach (MongoUser user in users)
            {
                EligibleAuctionBuys.Add(user.DiscordId, new Dictionary<AuctionBuy, AuctionSocketMessage>());
                EligibleBazaarBuys.Add(user.DiscordId, new List<BazaarSocketMessage>());
                EligibleBazaarSells.Add(user.DiscordId, new List<BazaarSocketMessage>());
            }

            Utility.Log(Enums.LogLevel.NONE, "MongoDB has connected!");
        }

        catch (Exception e)
        {
            Utility.Log(Enums.LogLevel.ERROR, $"MongoDB failed to connect! {e}");
            throw;
        }
    }

    /// <summary>
    /// Fetches the /bazaar and /resources/items route to determine current bazaar items and update the database/cache accordingly.
    /// </summary>
    public static async Task LoadBazaarItems()
    {
        if (_Uri == null)
        {
            Utility.Log(Enums.LogLevel.WARN, $"Cannot run 'LoadBazaarItems' without running 'Load' first!");
            return;
        }

        JsonNode? items = await ItemsRoute.GetRoute();
        Dictionary<string, BazaarRouteProduct>? products = await BazaarRoute.GetRoute();

        if (products == null || items == null)
        {
            Utility.Log(Enums.LogLevel.WARN, "Failed to load items; one of the routes has failed");
            return;
        }

        // resetting the cached items
        CachedBazaarItems = new Dictionary<string, BazaarItemsAll>();
        // dictionary here to prevent o^2 notation, and with how many items there are, probably a billion+ operation
        Dictionary<string, JsonNode> kvAllItems = new Dictionary<string, JsonNode>();

        foreach (var item in items["items"]!.AsArray())
            kvAllItems.Add(item!["id"]!.ToString(), item);

        foreach (BazaarRouteProduct product in products.Values)
        {
            string name;
            string id = product.product_id;

            // if for some reason, the name does not exist, set the name as the ID by default
            if (kvAllItems.ContainsKey(id))
                name = Utility.StripSpecial(kvAllItems[id]["name"]!.ToString());
            else
                name = id;

            CachedBazaarItems.Add(id, new BazaarItemsAll(id, name));
        }

        await BazaarItemsAll.InsertManyAsync(CachedBazaarItems.Select(e => e.Value));
        Utility.Log(Enums.LogLevel.NONE, "Updated bazaar items to database!");
    }

    /// <summary>
    /// <br></br>
    /// Fetches all pages from /auctions?page={x} and does the following:
    /// <br></br>
    /// <br>- Adds new items to the database</br>
    /// <br>- Adds new extra attributes (tags) to the database</br>
    /// <br>- Updates existing items where new extra attributes are found</br>
    /// <br>- Updates extra attributes (tag) where new values (references) are found</br>
    /// </summary>
    /// <param name="reset">Whether or not the database will be reset. Used when a major patch happens.</param>
    /// <returns></returns>
    public static async Task LoadAuctionItems(bool reset = false)
    {
        List<AuctionsRouteProduct>? auctions = await AuctionsRoute.GetRoute();

        if (auctions == null)
        {
            Utility.Log(Enums.LogLevel.WARN, "Failed to load auctions!");
            return;
        }

        if (reset)
        {
            await AuctionItemsAll.DeleteManyAsync(e => e.ID != "");
            await AuctionTags.DeleteManyAsync(e => e.Name != "");
        }

        // resetting the cached items
        CachedAuctionItems = new Dictionary<string, AuctionItemsAll>();

        List<string> auctionIDs = auctions.Select(e => e.ITEM_ID).ToList();
        IAsyncCursor<AuctionItemsAll> existingItemsResult = await AuctionItemsAll.FindAsync(e => auctionIDs.Contains(e.ID));
        IAsyncCursor<AuctionTags> existingTagsResult = await AuctionTags.FindAsync(e => e.Name != "");

        List<AuctionItemsAll> existingItems = existingItemsResult.Current?.ToList() ?? new List<AuctionItemsAll>();
        List<AuctionTags> existingTags = existingTagsResult.Current?.ToList() ?? new List<AuctionTags>();

        // theres actually alot of extra attributes, reducing from o^2
        // NOTE: key representation: Tags => Name, Items => ID
        Dictionary<string, AuctionTags> existingTagsDict = existingTags.Aggregate(new Dictionary<string, AuctionTags>(), (pV, cV) => { pV.Add(cV.Name, cV); return pV; });
        Dictionary<string, AuctionItemsAll> existingItemsDict = existingItems.Aggregate(new Dictionary<string, AuctionItemsAll>(), (pV, cV) => { pV.Add(cV.Name, cV); return pV; });
        Dictionary<string, AuctionTags> newTagsDict = new Dictionary<string, AuctionTags>();
        Dictionary<string, AuctionItemsAll> newItemsDict = new Dictionary<string, AuctionItemsAll>();

        foreach (var (k, v) in existingItemsDict)
            CachedAuctionItems.Add(v.ID, v);


        foreach (AuctionsRouteProduct auction in auctions)
        {
            AuctionItemsAll item;

            if (existingItemsDict.ContainsKey(auction.ITEM_ID))
            {
                item = existingItemsDict[auction.ITEM_ID];
            }

            else
            {
                string id = auction.ITEM_ID;
                string name = auction.ITEM_NAME;
                item = new AuctionItemsAll(id, name);
                newItemsDict.Add(item.ID, item);
                existingItemsDict.Add(item.ID, item);
                CachedAuctionItems.Add(item.ID, item);
            }

            foreach (AuctionTag attribute in auction.AuctionTags)
            {
                AuctionTags auctionTag;

                if (existingTagsDict.ContainsKey(attribute.Name))
                    auctionTag = existingTagsDict[attribute.Name];

                else
                {
                    auctionTag = new AuctionTags(attribute.Name, attribute.Type);
                    newTagsDict.Add(attribute.Name, auctionTag);
                    // NOTE: this is to prevent duplicate tags in the adding. the mongo operation should check
                    // if the tag is in new tag before updating, to prevent redundency
                    existingTagsDict.Add(attribute.Name, auctionTag);
                }

                if (!item.AuctionTags.Contains(auctionTag.Name))
                    item.AuctionTags.Add(auctionTag.Name);


                if (!auctionTag.Values.Contains(attribute.Value))
                    auctionTag.Values.Add(attribute.Value);
            }
        }

        await AuctionItemsAll.InsertManyAsync(newItemsDict.Select(e => e.Value));
        await AuctionTags.InsertManyAsync(newTagsDict.Select(e => e.Value));

        // TODO: rewrite as a bulk-write operation. ill probably never do it.
        foreach (var (k, v) in existingItemsDict)
        {
            if (newItemsDict.ContainsKey(k))
                continue;

            UpdateDefinition<AuctionItemsAll> update = Builders<AuctionItemsAll>.Update
                .Set(e => e.AuctionTags, v.AuctionTags);

            await AuctionItemsAll.FindOneAndUpdateAsync(e => e.ID == v.ID, update);
        }

        foreach (var (k, v) in existingTagsDict)
        {
            if (newTagsDict.ContainsKey(k))
                continue;

            UpdateDefinition<AuctionTags> update = Builders<AuctionTags>.Update
                .Set(e => e.Values, v.Values);

            await AuctionTags.FindOneAndUpdateAsync(e => e.Name == v.Name, update);
        }

        Utility.Log(Enums.LogLevel.NONE, "Updated auctions items to database!");
    }
}