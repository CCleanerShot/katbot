using System.Text.Json.Nodes;
using Cyotek.Data.Nbt;
using MongoDB.Driver;

public class MongoBot
{
    private static MongoClient _Client = default!;
    private static IMongoDatabase _DiscordDB = default!;
    private static IMongoDatabase _HypixelDB = default!;
    private static string _Uri = default!;

    public static Dictionary<string, AuctionBuy> CachedAuctionBuy = new Dictionary<string, AuctionBuy>();
    public static Dictionary<string, AuctionItemsAll> CachedAuctionItems = new Dictionary<string, AuctionItemsAll>();
    public static Dictionary<string, AuctionTags> CachedAuctionTags = new Dictionary<string, AuctionTags>();
    public static List<BazaarItem> CachedBazaarBuys = new List<BazaarItem>();
    public static Dictionary<string, BazaarItemsAll> CachedBazaarItems = new Dictionary<string, BazaarItemsAll>();
    public static List<BazaarItem> CachedBazaarSells = new List<BazaarItem>();

    public static IMongoCollection<AuctionBuy> AuctionBuy { get; protected set; } = default!;
    public static IMongoCollection<AuctionItemsAll> AuctionItemsAll { get; protected set; } = default!;
    public static IMongoCollection<AuctionTags> AuctionTags { get; protected set; } = default!;
    public static IMongoCollection<BazaarItem> BazaarBuy { get; protected set; } = default!;
    public static IMongoCollection<BazaarItemsAll> BazaarItemsAll { get; protected set; } = default!;
    public static IMongoCollection<BazaarItem> BazaarSell { get; protected set; } = default!;
    public static IMongoCollection<Starboards> Starboards { get; protected set; } = default!;
    public static IMongoCollection<RollStats> RollStats { get; protected set; } = default!;


    public static async Task Load()
    {
        try
        {
            _Uri = Settings.MONGODB_URI;
            _Client = new MongoClient(_Uri);
            _DiscordDB = _Client.GetDatabase(Settings.MONGODB_DATABASE_DISCORD);
            _HypixelDB = _Client.GetDatabase(Settings.MONGODB_DATABASE_HYPIXEL);
            AuctionBuy = _HypixelDB.GetCollection<AuctionBuy>(Settings.MONGODB_COLLECTION_AUCTION_BUY);
            AuctionItemsAll = _HypixelDB.GetCollection<AuctionItemsAll>(Settings.MONGODB_COLLECTION_AUCTION_ITEMS);
            AuctionTags = _HypixelDB.GetCollection<AuctionTags>(Settings.MONGODB_COLLECTION_AUCTION_TAGS);
            BazaarBuy = _HypixelDB.GetCollection<BazaarItem>(Settings.MONGODB_COLLECTION_BAZAAR_BUY);
            BazaarSell = _HypixelDB.GetCollection<BazaarItem>(Settings.MONGODB_COLLECTION_BAZAAR_SELL);
            BazaarItemsAll = _HypixelDB.GetCollection<BazaarItemsAll>(Settings.MONGODB_COLLECTION_BAZAAR_ITEMS);
            Starboards = _DiscordDB.GetCollection<Starboards>(Settings.MONGODB_COLLECTION_DISCORD_STARBOARDS);
            RollStats = _DiscordDB.GetCollection<RollStats>(Settings.MONGODB_COLLECTION_DISCORD_ROLL_STATS);

            // Creating the cache
            List<AuctionBuy> currentAuctionBuy = (await AuctionBuy.FindAsync(e => true)).ToList();
            List<AuctionItemsAll> currentAuctionItems = (await AuctionItemsAll.FindAsync(e => true)).ToList();
            List<AuctionTags> currentAuctionTags = (await AuctionTags.FindAsync(e => true)).ToList();
            List<BazaarItem> currentBazaarBuys = (await BazaarBuy.FindAsync(e => true)).ToList();
            List<BazaarItemsAll> currentBazaarItems = (await BazaarItemsAll.FindAsync(e => true)).ToList();
            List<BazaarItem> currentSells = (await BazaarSell.FindAsync(e => true)).ToList();

            // Reset the cache just in case
            CachedAuctionBuy = currentAuctionBuy.Aggregate(new Dictionary<string, AuctionBuy>(), (pV, cV) => { pV.Add(cV.ID, cV); return pV; });
            CachedAuctionItems = currentAuctionItems.Aggregate(new Dictionary<string, AuctionItemsAll>(), (pV, cV) => { pV.Add(cV.ID, cV); return pV; });
            CachedAuctionTags = currentAuctionTags.Aggregate(new Dictionary<string, AuctionTags>(), (pV, cV) => { pV.Add(cV.Name, cV); return pV; });
            CachedBazaarBuys = currentBazaarBuys.Aggregate(new List<BazaarItem>(), (pV, cV) => { pV.Add(cV); return pV; });
            CachedBazaarItems = currentBazaarItems.Aggregate(new Dictionary<string, BazaarItemsAll>(), (pV, cV) => { pV.Add(cV.ID, cV); return pV; });
            CachedBazaarSells = currentSells.Aggregate(new List<BazaarItem>(), (pV, cV) => { pV.Add(cV); return pV; });

            Program.Utility.Log(Enums.LogLevel.NONE, "MongoDB has connected!");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, $"MongoDB failed to connect! {e}");
            throw;
        }
    }

    /// <summary>
    /// Fetches the /bazaar and /resources/items route to determine current bazaar items and update the database/cache accordingly.
    /// </summary>
    public static async Task LoadBazaarItems()
    {
        JsonNode? items = await ItemsRoute.GetRoute();
        Dictionary<string, BazaarRouteProduct>? products = await BazaarRoute.GetRoute();

        if (products == null || items == null)
        {
            Program.Utility.Log(Enums.LogLevel.WARN, "Failed to load items; one of the routes has failed");
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
                name = kvAllItems[id]["name"]!.ToString();
            else
                name = id;

            CachedBazaarItems.Add(id, new BazaarItemsAll(id, name));
        }

        await BazaarItemsAll.InsertManyAsync(CachedBazaarItems.Select(e => e.Value));
        Program.Utility.Log(Enums.LogLevel.NONE, "Updated bazaar items to database!");
    }

    /// <summary>
    /// TODO: add reset functionality
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
            Program.Utility.Log(Enums.LogLevel.WARN, "Failed to load auctions!");
            return;
        }

        // resetting the cached items
        CachedAuctionItems = new Dictionary<string, AuctionItemsAll>();


        List<string> auctionIDs = auctions.Select(e => e.NBT.ID.Value).ToList();
        IAsyncCursor<AuctionItemsAll> existingItemsResult = await AuctionItemsAll.FindAsync(e => auctionIDs.Contains(e.ID));
        IAsyncCursor<AuctionTags> existingTagsResult = await AuctionTags.FindAsync(e => e.Name != "");

        List<AuctionItemsAll> existingItems = existingItemsResult.Current?.ToList() ?? new List<AuctionItemsAll>();
        List<AuctionTags> existingTags = existingTagsResult.Current?.ToList() ?? new List<AuctionTags>();

        // theres actually alot of extra attributes, reducing from o^2
        Dictionary<string, AuctionTags> existingTagsDict = existingTags.Aggregate(new Dictionary<string, AuctionTags>(), (pV, cV) => { pV.Add(cV.Name, cV); return pV; });
        Dictionary<string, AuctionItemsAll> existingItemsDict = existingItems.Aggregate(new Dictionary<string, AuctionItemsAll>(), (pV, cV) => { pV.Add(cV.Name, cV); return pV; });
        // these will be added to the database after the fact.
        // key representation: Tags => Name, Items => ID
        Dictionary<string, AuctionTags> newTagsDict = new Dictionary<string, AuctionTags>();
        Dictionary<string, AuctionItemsAll> newItemsDict = new Dictionary<string, AuctionItemsAll>();

        // recursive function
        void EvalTag(Cyotek.Data.Nbt.Tag tag)
        {
            AuctionTags auctionTag;

            // check to update the tags if there are new values
            if (existingTagsDict.ContainsKey(tag.Name))
            {
                auctionTag = existingTagsDict[tag.Name];
                if (auctionTag.Type == TagType.String && !auctionTag.Values.Contains(tag.GetValue().ToString()!))
                    auctionTag.Values.Add(tag.GetValue().ToString()!);
            }

            // add the new tag
            else
            {
                auctionTag = new AuctionTags(tag.Name, tag.Type, tag.GetValue().ToString()!);
                newTagsDict.Add(tag.Name, auctionTag);
                // NOTE: this is to prevent duplicate tags in the adding. the mongo operation should check
                // if the tag is in new tag before updating, to prevent redundency
                existingTagsDict.Add(tag.Name, auctionTag);
            }

            if (tag is not TagCompound tagCompound)
                return;

            foreach (Cyotek.Data.Nbt.Tag innerTag in tagCompound.Value)
            {
                EvalTag(innerTag);

                if (!auctionTag.Tags.Contains(tag.Name))
                    auctionTag.Tags.Add(innerTag.Name);
            }
        }

        foreach (AuctionsRouteProduct auction in auctions)
        {
            AuctionItemsAll item;

            // update the item with new tags if present
            if (existingItemsDict.ContainsKey(auction.NBT.ID.Value))
            {
                item = existingItemsDict[auction.NBT.ID.Value.ToString()];
            }

            // add new items that havent been seen before
            else
            {
                string id = auction.NBT.ID.Value;
                string name = auction.NBT.NAME.Value;
                List<string> tags = auction.NBT.EXTRA_ATTRIBUTES.Value.Aggregate(new List<string>(), (pV, cV) => { pV.Add(cV.Name); return pV; });
                item = new AuctionItemsAll(id, name, tags);
                newItemsDict.Add(item.ID, item);
                existingItemsDict.Add(item.ID, item);
            }

            foreach (Cyotek.Data.Nbt.Tag tag in auction.NBT.EXTRA_ATTRIBUTES.Value)
            {
                EvalTag(tag);

                // NOTE: slightly unoptimized atm but idc this hopefully never matters
                if (!item.ExtraAttributes.Contains(tag.Name))
                    item.ExtraAttributes.Add(tag.Name);
            }

            CachedAuctionItems.Add(item.ID, item);
        }

        await AuctionItemsAll.InsertManyAsync(newItemsDict.Select(e => e.Value));
        await AuctionTags.InsertManyAsync(newTagsDict.Select(e => e.Value));

        // TODO: rewrite as a bulk-write operation. ill probably never do it.
        foreach (var (k, v) in existingItemsDict)
        {
            if (newItemsDict.ContainsKey(k))
                continue;

            UpdateDefinition<AuctionItemsAll> update = Builders<AuctionItemsAll>.Update
                .Set(e => e.ExtraAttributes, v.ExtraAttributes);

            await AuctionItemsAll.FindOneAndUpdateAsync(e => e.ID == v.ID, update);
        }

        foreach (var (k, v) in existingTagsDict)
        {
            if (newTagsDict.ContainsKey(k))
                continue;

            UpdateDefinition<AuctionTags> update = Builders<AuctionTags>.Update
                .Set(e => e.Tags, v.Tags)
                .Set(e => e.Values, v.Values);

            await AuctionTags.FindOneAndUpdateAsync(e => e.Name == v.Name, update);
        }

        Program.Utility.Log(Enums.LogLevel.NONE, "Updated auctions items to database!");
    }
}