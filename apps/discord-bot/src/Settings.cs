/// <summary>
/// Holds data regarding constant runtime-specifics of the app.
/// </summary>
public class Settings
{
    #region  PRIVATE should not be exposed
    public static string ADMIN_1 = "";
    public static string ADMIN_2 = "";
    public static ulong DISCORD_APPLICATION_ID = 0;
    public static ulong DISCORD_HYPIXEL_ALERTS_CHANNEL_ID = 0;
    public static ulong DISCORD_MAIN_CHANNEL_ID = 0;
    public static string DISCORD_PUBLIC_KEY = "";
    public static ulong DISCORD_SEND_CHANNEL_ID = 0;
    public static ulong DISCORD_STARBOARDS_CHANNEL_ID = 0;
    public static string DISCORD_TOKEN = "";
    public static string ENVIRONMENT = "";
    public static string HYPIXEL_API_BASE_URL = "";
    public static string HYPIXEL_BOT_KEY = "";
    public static string ID_BOT = "";
    public static string ID_CARLOS = "";
    public static string ID_KELINIMO = "";
    public static string ID_RAMOJUSD = "";
    public static string ID_VOLATILE = "";
    public static string MONGODB_BASE_URI = "";
    public static string MONGODB_BASE_URI_TEST = "";
    public static string MONGODB_C_AUCTION_BUY = "";
    public static string MONGODB_C_AUCTION_ITEMS = "";
    public static string MONGODB_C_AUCTION_TAGS = "";
    public static string MONGODB_C_BAZAAR_BUY = "";
    public static string MONGODB_C_BAZAAR_SELL = "";
    public static string MONGODB_C_BAZAAR_ITEMS = "";
    public static string MONGODB_C_SESSIONS = "";
    public static string MONGODB_C_STARBOARDS = "";
    public static string MONGODB_C_ROLL_STATS = "";
    public static string MONGODB_C_USERS = "";
    public static string MONGODB_D_DISCORD = "";
    public static string MONGODB_D_GENERAL = "";
    public static string MONGODB_D_HYPIXEL = "";
    public static string MONGODB_OPTIONS = "";
    public static string PATH_OBAMA = "";
    public static ulong TEST_DISCORD_GUILD_ID = 0;
    #endregion

    #region PUBLIC idc if these are exposed
    public static string PUBLIC_PATH_MONDAY_GIF_URL = "https://cdn.imgchest.com/files/4z9cvdj6op7.gif";
    public static string PUBLIC_PATH_MONDAY_GIF_URL_2 = "https://media1.tenor.com/m/Jnqs70oki_AAAAAd/tuesday.gif";
    public static string PUBLIC_PATH_RAMOJUSD_GIF_URL = "https://cdn.discordapp.com/attachments/1210132113358065695/1236680414597222411/ramojusd.gif?ex=679c3023&is=679adea3&hm=5f03113120db858cb71fcd7715f4ca75ffeda1a5de2a284639034d90fc97e07c&";
    public static int PUBLIC_HYPIXEL_AUCTION_TIMER_MINUTES = 6;
    public static int PUBLIC_HYPIXEL_BAZAAR_TIMER_MINUTES = 4;
    #endregion

    /// <summary>
    /// Loads the settings.
    /// </summary>
    public static void Load()
    {
        LoadAssets();
        LoadENV();
    }

    /// <summary>
    /// Loads the assets from /assets, if any.
    /// </summary>
    static void LoadAssets()
    {
        string assetsFolder = Path.GetFullPath("assets");
        PATH_OBAMA = $"{assetsFolder}/obama.jpg";
    }

    /// <summary>
    /// Loads the .env file.
    /// </summary>
    static void LoadENV()
    {
#if DEBUG
        using StreamReader streamReader = new StreamReader(".env");
        string? line = null;

        do
        {
            line = streamReader.ReadLine();

            if (line == null)
                continue;

            string[] lines = line.Split("=", 2);

            if (lines.Length != 2)
                throw new Exception("Illegal structure in the .env.");

            if (lines[0] == "ADMIN_1")
                ADMIN_1 = lines[1];
            else if (lines[0] == "ADMIN_2")
                ADMIN_2 = lines[1];
            else if (lines[0] == "DISCORD_APPLICATION_ID")
                DISCORD_APPLICATION_ID = UInt64.Parse(lines[1]);
            else if (lines[0] == "DISCORD_HYPIXEL_ALERTS_CHANNEL_ID")
                DISCORD_HYPIXEL_ALERTS_CHANNEL_ID = UInt64.Parse(lines[1]);
            else if (lines[0] == "DISCORD_MAIN_CHANNEL_ID")
                DISCORD_MAIN_CHANNEL_ID = UInt64.Parse(lines[1]);
            else if (lines[0] == "DISCORD_PUBLIC_KEY")
                DISCORD_PUBLIC_KEY = lines[1];
            else if (lines[0] == "DISCORD_SEND_CHANNEL_ID")
                DISCORD_SEND_CHANNEL_ID = UInt64.Parse(lines[1]);
            else if (lines[0] == "DISCORD_STARBOARDS_CHANNEL_ID")
                DISCORD_STARBOARDS_CHANNEL_ID = UInt64.Parse(lines[1]);
            else if (lines[0] == "DISCORD_TOKEN")
                DISCORD_TOKEN = lines[1];
            else if (lines[0] == "ENVIRONMENT")
                ENVIRONMENT = lines[1];
            else if (lines[0] == "HYPIXEL_API_BASE_URL")
                HYPIXEL_API_BASE_URL = lines[1];
            else if (lines[0] == "HYPIXEL_BOT_KEY")
                HYPIXEL_BOT_KEY = lines[1];
            else if (lines[0] == "ID_BOT")
                ID_BOT = lines[1];
            else if (lines[0] == "ID_CARLOS")
                ID_CARLOS = lines[1];
            else if (lines[0] == "ID_KELINIMO")
                ID_KELINIMO = lines[1];
            else if (lines[0] == "ID_RAMOJUSD")
                ID_RAMOJUSD = lines[1];
            else if (lines[0] == "ID_VOLATILE")
                ID_VOLATILE = lines[1];
            else if (lines[0] == "MONGODB_BASE_URI")
                MONGODB_BASE_URI = lines[1];
            else if (lines[0] == "MONGODB_BASE_URI_TEST")
                MONGODB_BASE_URI_TEST = lines[1];
            else if (lines[0] == "MONGODB_C_AUCTION_BUY")
                MONGODB_C_AUCTION_BUY = lines[1];
            else if (lines[0] == "MONGODB_C_AUCTION_ITEMS")
                MONGODB_C_AUCTION_ITEMS = lines[1];
            else if (lines[0] == "MONGODB_C_AUCTION_TAGS")
                MONGODB_C_AUCTION_TAGS = lines[1];
            else if (lines[0] == "MONGODB_C_BAZAAR_BUY")
                MONGODB_C_BAZAAR_BUY = lines[1];
            else if (lines[0] == "MONGODB_C_BAZAAR_ITEMS")
                MONGODB_C_BAZAAR_ITEMS = lines[1];
            else if (lines[0] == "MONGODB_C_BAZAAR_SELL")
                MONGODB_C_BAZAAR_SELL = lines[1];
            else if (lines[0] == "MONGODB_C_ROLL_STATS")
                MONGODB_C_ROLL_STATS = lines[1];
            else if (lines[0] == "MONGODB_C_SESSIONS")
                MONGODB_C_SESSIONS = lines[1];
            else if (lines[0] == "MONGODB_C_STARBOARDS")
                MONGODB_C_STARBOARDS = lines[1];
            else if (lines[0] == "MONGODB_C_USERS")
                MONGODB_C_USERS = lines[1];
            else if (lines[0] == "MONGODB_D_DISCORD")
                MONGODB_D_DISCORD = lines[1];
            else if (lines[0] == "MONGODB_D_HYPIXEL")
                MONGODB_D_HYPIXEL = lines[1];
            else if (lines[0] == "MONGODB_D_GENERAL")
                MONGODB_D_GENERAL = lines[1];
            else if (lines[0] == "MONGODB_OPTIONS")
                MONGODB_OPTIONS = lines[1];
            else if (lines[0] == "TEST_DISCORD_GUILD_ID")
                TEST_DISCORD_GUILD_ID = UInt64.Parse(lines[1]);
            else
                Program.Utility.Log(Enums.LogLevel.WARN, $"The key was not implemented yet. Intentional? (at {lines[0]})");
        } while (line != null);
#else
        ADMIN_1 = Environment.GetEnvironmentVariable("ADMIN_1");
        ADMIN_2 = Environment.GetEnvironmentVariable("ADMIN_2");
        DISCORD_APPLICATION_ID = UInt64.Parse(Environment.GetEnvironmentVariable("DISCORD_APPLICATION_ID"));
        DISCORD_HYPIXEL_ALERTS_CHANNEL_ID = UInt64.Parse(Environment.GetEnvironmentVariable("DISCORD_HYPIXEL_ALERTS_CHANNEL_ID"));
        DISCORD_MAIN_CHANNEL_ID = UInt64.Parse(Environment.GetEnvironmentVariable("DISCORD_MAIN_CHANNEL_ID"));
        DISCORD_PUBLIC_KEY = Environment.GetEnvironmentVariable("DISCORD_PUBLIC_KEY");
        DISCORD_SEND_CHANNEL_ID = UInt64.Parse(Environment.GetEnvironmentVariable("DISCORD_SEND_CHANNEL_ID"));
        DISCORD_STARBOARDS_CHANNEL_ID = UInt64.Parse(Environment.GetEnvironmentVariable("DISCORD_STARBOARDS_CHANNEL_ID"));
        DISCORD_TOKEN = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        ENVIRONMENT = Environment.GetEnvironmentVariable("ENVIRONMENT");
        HYPIXEL_API_BASE_URL = Environment.GetEnvironmentVariable("HYPIXEL_API_BASE_URL");
        HYPIXEL_BOT_KEY = Environment.GetEnvironmentVariable("HYPIXEL_BOT_KEY");
        ID_BOT = Environment.GetEnvironmentVariable("ID_BOT");
        ID_CARLOS = Environment.GetEnvironmentVariable("ID_CARLOS");
        ID_KELINIMO = Environment.GetEnvironmentVariable("ID_KELINIMO");
        ID_RAMOJUSD = Environment.GetEnvironmentVariable("ID_RAMOJUSD");
        ID_VOLATILE = Environment.GetEnvironmentVariable("ID_VOLATILE");
        MONGODB_BASE_URI = Environment.GetEnvironmentVariable("MONGODB_BASE_URI");
        MONGODB_BASE_URI_TEST = Environment.GetEnvironmentVariable("MONGODB_BASE_URI_TEST");
        MONGODB_C_AUCTION_BUY = Environment.GetEnvironmentVariable("MONGODB_C_AUCTION_BUY");
        MONGODB_C_AUCTION_ITEMS = Environment.GetEnvironmentVariable("MONGODB_C_AUCTION_ITEMS");
        MONGODB_C_AUCTION_TAGS = Environment.GetEnvironmentVariable("MONGODB_C_AUCTION_TAGS");
        MONGODB_C_BAZAAR_BUY = Environment.GetEnvironmentVariable("MONGODB_C_BAZAAR_BUY");
        MONGODB_C_BAZAAR_ITEMS = Environment.GetEnvironmentVariable("MONGODB_C_BAZAAR_ITEMS");
        MONGODB_C_BAZAAR_SELL = Environment.GetEnvironmentVariable("MONGODB_C_BAZAAR_SELL");
        MONGODB_C_ROLL_STATS = Environment.GetEnvironmentVariable("MONGODB_C_ROLL_STATS");
        MONGODB_C_SESSIONS = Environment.GetEnvironmentVariable("MONGODB_C_SESSIONS");
        MONGODB_C_STARBOARDS = Environment.GetEnvironmentVariable("MONGODB_C_STARBOARDS");
        MONGODB_C_USERS = Environment.GetEnvironmentVariable("MONGODB_C_USERS");
        MONGODB_D_DISCORD = Environment.GetEnvironmentVariable("MONGODB_D_DISCORD");
        MONGODB_D_HYPIXEL = Environment.GetEnvironmentVariable("MONGODB_D_HYPIXEL");
        MONGODB_D_GENERAL = Environment.GetEnvironmentVariable("MONGODB_D_GENERAL");
        TEST_DISCORD_GUILD_ID = UInt64.Parse(Environment.GetEnvironmentVariable("TEST_DISCORD_GUILD_ID"));
#endif
    }
}

