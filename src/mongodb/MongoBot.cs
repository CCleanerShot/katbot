using System.Text.Json.Nodes;
using MongoDB.Driver;

public class MongoBot
{
    private static MongoClient _Client = default!;
    private static IMongoDatabase _DiscordDB = default!;
    private static IMongoDatabase _HypixelDB = default!;
    private static string _Uri = default!;

    public static List<BazaarItem> CachedBuys = new List<BazaarItem>();
    public static Dictionary<string, ItemsAll> CachedItems = new Dictionary<string, ItemsAll>();
    public static List<BazaarItem> CachedSells = new List<BazaarItem>();

    public static IMongoCollection<BazaarItem> BazaarBuy { get; protected set; } = default!;
    public static IMongoCollection<BazaarItem> BazaarSell { get; protected set; } = default!;
    public static IMongoCollection<ItemsAll> ItemsAll { get; protected set; } = default!;
    public static IMongoCollection<StarboardMessage> Starboards { get; protected set; } = default!;

    public static async Task Load()
    {
        try
        {
            _Uri = Settings.MONGODB_URI;
            _Client = new MongoClient(_Uri);
            _DiscordDB = _Client.GetDatabase(Settings.MONGODB_DATABASE_DISCORD);
            _HypixelDB = _Client.GetDatabase(Settings.MONGODB_DATABASE_HYPIXEL);
            BazaarBuy = _HypixelDB.GetCollection<BazaarItem>(Settings.MONGODB_COLLECTION_BAZAAR_BUY);
            BazaarSell = _HypixelDB.GetCollection<BazaarItem>(Settings.MONGODB_COLLECTION_BAZAAR_SELL);
            ItemsAll = _HypixelDB.GetCollection<ItemsAll>(Settings.MONGODB_COLLECTION_ITEMS_ALL);
            Starboards = _DiscordDB.GetCollection<StarboardMessage>(Settings.MONGODB_COLLECTION_DISCORD_STARBOARDS);

            // Test the connection
            await BazaarBuy.FindAsync(e => e.Name != "");
            await BazaarSell.FindAsync(e => e.Name != "");
            await Starboards.FindAsync(e => e.MessageId != 0);

            // Creating the cache
            List<BazaarItem> currentBuys = (await BazaarBuy.FindAsync(e => true)).ToList();
            List<ItemsAll> currentItems = (await ItemsAll.FindAsync(e => true)).ToList();
            List<BazaarItem> currentSells = (await BazaarSell.FindAsync(e => true)).ToList();

            // Reset the cache just in case
            CachedBuys = currentBuys.Aggregate(new List<BazaarItem>(), (pV, cV) => { pV.Add(cV); return pV; });
            CachedItems = currentItems.Aggregate(new Dictionary<string, ItemsAll>(), (pV, cV) => { pV.Add(cV.ID, cV); return pV; });
            CachedSells = currentSells.Aggregate(new List<BazaarItem>(), (pV, cV) => { pV.Add(cV); return pV; });

            Utility.Log(Enums.LogLevel.NONE, "MongoDB has connected!");
        }

        catch (Exception)
        {
            Utility.Log(Enums.LogLevel.ERROR, "MongoDB failed to connect! Throwing...");
            throw;
        }
    }

    /// <summary>
    /// Fetches the /bazaar and /resources/items route to determine current bazaar items and update the database/cache accordingly.
    /// </summary>
    public static async Task LoadItems()
    {
        JsonNode? items = await ItemsRoute.GetRoute();
        Dictionary<string, BazaarRouteProduct>? products = await BazaarRoute.GetRoute();

        if (products == null || items == null)
        {
            Utility.Log(Enums.LogLevel.WARN, "Failed to load items; one of the routes has failed");
            return;
        }

        // resetting the cached items
        CachedItems = new Dictionary<string, ItemsAll>();
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

            CachedItems.Add(id, new ItemsAll(id, name));
        }

        await ItemsAll.InsertManyAsync(CachedItems.Select(e => e.Value));
    }
}