using MongoDB.Driver;

public class MongoBot
{
    private static MongoClient _Client = default!;
    private static IMongoDatabase _DiscordDB = default!;
    private static IMongoDatabase _HypixelDB = default!;

    private static string _Uri = default!;

    public static IMongoCollection<BazaarTracked> BazaarTracked { get; protected set; } = default!;
    public static IMongoCollection<StarboardMessage> Starboards { get; protected set; } = default!;

    public static async Task Load()
    {
        try
        {
            _Uri = Settings.MONGODB_URI;
            _Client = new MongoClient(_Uri);
            _DiscordDB = _Client.GetDatabase(Settings.MONGODB_DATABASE_DISCORD);
            _HypixelDB = _Client.GetDatabase(Settings.MONGODB_DATABASE_HYPIXEL);
            BazaarTracked = _HypixelDB.GetCollection<BazaarTracked>(Settings.MONGODB_COLLECTION_BAZAAR_TRACKED);
            Starboards = _DiscordDB.GetCollection<StarboardMessage>(Settings.MONGODB_COLLECTION_DISCORD_STARBOARDS);

            // Test the connection:
            await BazaarTracked.FindAsync(e => e.Name != "");
            await Starboards.FindAsync(e => e.MessageId != 0);

            Utility.Log(Enums.LogLevel.NONE, "MongoDB has connected!");
        }

        catch (Exception)
        {
            Utility.Log(Enums.LogLevel.ERROR, "MongoDB failed to connect! Throwing...");
            throw;
        }
    }
}