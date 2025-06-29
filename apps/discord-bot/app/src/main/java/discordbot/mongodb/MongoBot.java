package discordbot.mongodb;

import discordbot.Main;
import discordbot.BotSettings;

import java.util.HashMap;
import java.text.MessageFormat;
import java.util.ArrayList;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoClients;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;

import discordbot.common.Enums;
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
    public static MongoCollection<BazaarItem> BazaarBuy;
    public static MongoCollection<BazaarItemsAll> BazaarItemsAll;
    public static MongoCollection<BazaarItem> BazaarSell;
    public static MongoCollection<MongoUser> MongoUser;
    public static MongoCollection<RollStats> RollStats;
    public static MongoCollection<Session> Session;
    public static MongoCollection<Starboards> Starboards;

    public static void Load() {
        try {
            if (BotSettings.Get(Settings.ENVIRONMENT) == "development")
                _Uri = BotSettings.Get(Settings.MONGODB_BASE_URI_TEST) + BotSettings.Get(Settings.MONGODB_OPTIONS);
            else
                _Uri = BotSettings.Get(Settings.MONGODB_BASE_URI) + BotSettings.Get(Settings.MONGODB_OPTIONS);

            _Client = MongoClients.create(_Uri);
            _DiscordDB = _Client.getDatabase(BotSettings.Get(Settings.MONGODB_D_DISCORD));
            _GeneralDB = _Client.getDatabase(BotSettings.Get(Settings.MONGODB_D_GENERAL));
            _HypixelDB = _Client.getDatabase(BotSettings.Get(Settings.MONGODB_D_HYPIXEL));

            AuctionBuy = _HypixelDB.getCollection(BotSettings.Get(Settings.MONGODB_C_AUCTION_BUY), AuctionBuy.class);
            AuctionItemsAll = _HypixelDB.getCollection(BotSettings.Get(Settings.MONGODB_C_AUCTION_ITEMS), AuctionItemsAll.class);
            AuctionTags = _HypixelDB.getCollection(BotSettings.Get(Settings.MONGODB_C_AUCTION_TAGS), AuctionTags.class);
            BazaarBuy = _HypixelDB.getCollection(BotSettings.Get(Settings.MONGODB_C_BAZAAR_BUY), BazaarItem.class);
            BazaarItemsAll = _HypixelDB.getCollection(BotSettings.Get(Settings.MONGODB_C_BAZAAR_ITEMS), BazaarItemsAll.class);
            BazaarSell = _HypixelDB.getCollection(BotSettings.Get(Settings.MONGODB_C_BAZAAR_SELL), BazaarItem.class);
            MongoUser = _GeneralDB.getCollection(BotSettings.Get(Settings.MONGODB_C_USERS), MongoUser.class);
            RollStats = _DiscordDB.getCollection(BotSettings.Get(Settings.MONGODB_C_ROLL_STATS), RollStats.class);
            Session = _DiscordDB.getCollection(BotSettings.Get(Settings.MONGODB_C_SESSIONS), Session.class);
            Starboards = _DiscordDB.getCollection(BotSettings.Get(Settings.MONGODB_C_STARBOARDS), Starboards.class);

            CachedAuctionBuys = Main.Utility.ToArrayList(AuctionBuy.find());
            CachedAuctionItems = Main.Utility.ToHashMap(AuctionItemsAll.find(), (item) -> item.ID);
            CachedAuctionTags = Main.Utility.ToHashMap(AuctionTags.find(), (item) -> item.Name);
            CachedBazaarBuys = Main.Utility.ToArrayList(BazaarBuy.find());
            CachedBazaarItems = Main.Utility.ToHashMap(BazaarItemsAll.find(), (item) -> item.ID);
            CachedBazaarSells = Main.Utility.ToArrayList(BazaarSell.find());

            Main.Utility.Log(Enums.LogLevel.NONE, "MongoDB has connected!");
        } catch (Exception e) {
            Main.Utility.Log(Enums.LogLevel.ERROR, MessageFormat.format("MongoDB failed to connect! {0}", e));
            throw e;
        }
    }
}
