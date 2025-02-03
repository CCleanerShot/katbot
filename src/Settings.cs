using System.Reflection;

/// <summary>
/// Holds data regarding constant runtime-specifics of the app.
/// </summary>
public class Settings
{
    public static string ADMIN_1 = "";
    public static string ADMIN_2 = "";
    public static ulong DISCORD_APPLICATION_ID = 0;
    public static ulong DISCORD_HYPIXEL_ALERTS_CHANNEL_ID = 0;
    public static ulong DISCORD_MAIN_CHANNEL_ID = 0;
    public static string DISCORD_PUBLIC_KEY = "";
    public static ulong DISCORD_SEND_CHANNEL_ID = 0;
    public static ulong DISCORD_STARBOARDS_CHANNEL_ID = 0;
    public static string DISCORD_TOKEN = "";
    public static string HYPIXEL_API_BASE_URL = "";
    public static string HYPIXEL_BOT_KEY = "";
    public static int HYPIXEL_TIMER_MINUTES = 0;
    public static string ID_CARLOS = "";
    public static string ID_KELINIMO = "";
    public static string ID_RAMOJUSD = "";
    public static string ID_VOLATILE = "";
    public static string MONGODB_DATABASE_DISCORD = "";
    public static string MONGODB_DATABASE_HYPIXEL = "";
    public static string MONGODB_COLLECTION_BAZAAR_BUY = "";
    public static string MONGODB_COLLECTION_BAZAAR_SELL = "";
    public static string MONGODB_COLLECTION_BAZAAR_ITEMS = "";
    public static string MONGODB_COLLECTION_DISCORD_STARBOARDS = "";
    public static string MONGODB_COLLECTION_DISCORD_ROLL_STATS = "";
    public static string MONGODB_URI = "";
    public static string PATH_OBAMA = "";
    public static string PATH_MONDAY_GIF_URL = "https://media1.tenor.com/m/eB9Egjcaa_QAAAAd/evangelion.gif";
    public static string PATH_RAMOJUSD_GIF_URL = "https://cdn.discordapp.com/attachments/1210132113358065695/1236680414597222411/ramojusd.gif?ex=679c3023&is=679adea3&hm=5f03113120db858cb71fcd7715f4ca75ffeda1a5de2a284639034d90fc97e07c&";

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
            else if (lines[0] == "HYPIXEL_API_BASE_URL")
                HYPIXEL_API_BASE_URL = lines[1];
            else if (lines[0] == "HYPIXEL_BOT_KEY")
                HYPIXEL_BOT_KEY = lines[1];
            else if (lines[0] == "HYPIXEL_TIMER_MINUTES")
                HYPIXEL_TIMER_MINUTES = Int32.Parse(lines[1]);
            else if (lines[0] == "ID_CARLOS")
                ID_CARLOS = lines[1];
            else if (lines[0] == "ID_KELINIMO")
                ID_KELINIMO = lines[1];
            else if (lines[0] == "ID_RAMOJUSD")
                ID_RAMOJUSD = lines[1];
            else if (lines[0] == "ID_VOLATILE")
                ID_VOLATILE = lines[1];
            else if (lines[0] == "MONGODB_DATABASE_DISCORD")
                MONGODB_DATABASE_DISCORD = lines[1];
            else if (lines[0] == "MONGODB_DATABASE_HYPIXEL")
                MONGODB_DATABASE_HYPIXEL = lines[1];
            else if (lines[0] == "MONGODB_COLLECTION_BAZAAR_BUY")
                MONGODB_COLLECTION_BAZAAR_BUY = lines[1];
            else if (lines[0] == "MONGODB_COLLECTION_BAZAAR_SELL")
                MONGODB_COLLECTION_BAZAAR_SELL = lines[1];
            else if (lines[0] == "MONGODB_COLLECTION_BAZAAR_ITEMS")
                MONGODB_COLLECTION_BAZAAR_ITEMS = lines[1];
            else if (lines[0] == "MONGODB_COLLECTION_STARBOARDS")
                MONGODB_COLLECTION_DISCORD_STARBOARDS = lines[1];
            else if (lines[0] == "MONGODB_COLLECTION_DISCORD_ROLL_STATS")
                MONGODB_COLLECTION_DISCORD_ROLL_STATS = lines[1];
            else if (lines[0] == "MONGODB_URI")
                MONGODB_URI = lines[1];
            else
                Program.Utility.Log(Enums.LogLevel.WARN, "The key was not implemented yet. Intentional?");

        } while (line != null);
    }
}

