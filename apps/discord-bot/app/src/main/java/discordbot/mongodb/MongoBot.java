package discordbot.mongodb;

import discordbot.S;

import java.util.HashMap;
import java.util.ArrayList;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoClients;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;
import discordbot.common.Enums.Settings;
import discordbot.mongodb.collections.AuctionBuy;
import discordbot.mongodb.collections.AuctionItemsAll;
import discordbot.mongodb.collections.AuctionTags;
import discordbot.mongodb.collections.BazaarItem;
import discordbot.mongodb.collections.BazaarItemsAll;
import discordbot.mongodb.collections.MongoUser;
import discordbot.mongodb.collections.RollStats;
import discordbot.mongodb.collections.Session;
import discordbot.mongodb.collections.Starboards;

public class MongoBot {
    static MongoClient _Client;
    static MongoDatabase _DiscordDB;
    static MongoDatabase _GeneralDB;
    static MongoDatabase _HypixelDB;
    static String _Uri;

    public static ArrayList<AuctionBuy> CachedAuctionBuys = new ArrayList<AuctionBuy>();
    public static HashMap<String, AuctionItemsAll> CachedAuctionItems = new HashMap<String, AuctionItemsAll>();
    public static HashMap<String, AuctionTags> CachedAuctionTags = new HashMap<String, AuctionTags>();
    public static ArrayList<BazaarItem> CachedBazaarBuys = new ArrayList<BazaarItem>();
    public static HashMap<String, BazaarItemsAll> CachedBazaarItems = new HashMap<String, BazaarItemsAll>();
    public static ArrayList<BazaarItem> CachedBazaarSells = new ArrayList<BazaarItem>();

    public static MongoCollection<AuctionBuy> AuctionBuy;
    public static MongoCollection<AuctionItemsAll> AuctionItemsAll;
    public static MongoCollection<AuctionTags> AuctionTags;
    public static MongoCollection<BazaarItem> BazaarItem;
    public static MongoCollection<BazaarItemsAll> BazaarItemsAll;
    public static MongoCollection<MongoUser> MongoUser;
    public static MongoCollection<RollStats> RollStats;
    public static MongoCollection<Session> Session;
    public static MongoCollection<Starboards> Starboards;

    public static void Load() {
        try {
            if (S.Get(Settings.ENVIRONMENT) == "development")
                _Uri = S.Get(Settings.MONGODB_BASE_URI_TEST) + S.Get(Settings.MONGODB_OPTIONS);
            else
                _Uri = S.Get(Settings.MONGODB_BASE_URI) + S.Get(Settings.MONGODB_OPTIONS);

            _Client = MongoClients.create(_Uri);
            _DiscordDB = _Client.getDatabase(S.Get(Settings.MONGODB_D_DISCORD));
            _GeneralDB = _Client.getDatabase(S.Get(Settings.MONGODB_D_GENERAL));
            _HypixelDB = _Client.getDatabase(S.Get(Settings.MONGODB_D_HYPIXEL));

            AuctionBuy = _HypixelDB.getCollection(S.Get(Settings.MONGODB_C_AUCTION_BUY), AuctionBuy.class);
            AuctionItemsAll = _HypixelDB.getCollection(S.Get(Settings.MONGODB_C_AUCTION_ITEMS), AuctionItemsAll.class);
            AuctionTags = _HypixelDB.getCollection(S.Get(Settings.MONGODB_C_AUCTION_TAGS), AuctionTags.class);
            BazaarItemsAll = _HypixelDB.getCollection(S.Get(Settings.MONGODB_C_BAZAAR_ITEMS), BazaarItemsAll.class);
            MongoUser = _GeneralDB.getCollection(S.Get(Settings.MONGODB_C_USERS), MongoUser.class);
            RollStats = _DiscordDB.getCollection(S.Get(Settings.MONGODB_C_ROLL_STATS), RollStats.class);
            Session = _DiscordDB.getCollection(S.Get(Settings.MONGODB_C_SESSIONS), Session.class);
            Starboards = _DiscordDB.getCollection(S.Get(Settings.MONGODB_C_STARBOARDS), Starboards.class);

            // ArrayList<AuctionBuy> currentAuctionBuy = AuctionBuy.find(e -> true);
            // ArrayList<AuctionItemsAll> currentAuctionItems = AuctionItemsAll.find(e -> true);
            // ArrayList<AuctionTags> currentAuctionTags = AuctionTags.find(e -> true);
            // ArrayList<BazaarItem> currentBazaarBuys = BazaarBuy.find(e -> true);
            // ArrayList<BazaarItemsAll> currentBazaarItems = BazaarItemsAll.find(e -> true);
            // ArrayList<BazaarItem> currentSells = BazaarSell.find(e -> true);

            // // Reset the cache just in case
            // CachedAuctionBuys = currentAuctionBuy.Aggregate(new ArrayList<AuctionBuy>(), (pV, cV) -> { pV.Add(cV); return pV; });
            // CachedAuctionItems = currentAuctionItems.Aggregate(new HashMap<String, AuctionItemsAll>(), (pV, cV) -> { pV.Add(cV.ID, cV); return pV; });
            // CachedAuctionTags = currentAuctionTags.Aggregate(new HashMap<String, AuctionTags>(), (pV, cV) -> { pV.Add(cV.Name, cV); return pV; });
            // CachedBazaarBuys = currentBazaarBuys.Aggregate(new ArrayList<BazaarItem>(), (pV, cV) -> { pV.Add(cV); return pV; });
            // CachedBazaarItems = currentBazaarItems.Aggregate(new HashMap<String, BazaarItemsAll>(), (pV, cV) -> { pV.Add(cV.ID, cV); return pV; });
            // CachedBazaarSells = currentSells.Aggregate(new ArrayList<BazaarItem>(), (pV, cV) -> { pV.Add(cV); return pV; });

        } catch (Exception e) {

        }
    }
}
