/// <summary>
/// Holds data regarding constant runtime-specifics of the app.
/// </summary>
public class Settings
{
    #region  PRIVATE should not be exposed
    public static string CERTIFICATE_LOCATION_LINUX = "";
    public static string CERTIFICATE_LOCATION_WINDOWS = "";
    public static string CERTIFICATE_PASSWORD = ""; // security issue but i dont really care lol
    public static string DOCKER = "";
    public static string ENVIRONMENT = "";
    public static string HYPIXEL_API_BASE_URL = "";
    public static string HYPIXEL_BOT_KEY = "";
    public static string MONGODB_BASE_URI = "";
    public static string MONGODB_C_AUCTION_BUY = "";
    public static string MONGODB_C_AUCTION_ITEMS = "";
    public static string MONGODB_C_AUCTION_TAGS = "";
    public static string MONGODB_C_BAZAAR_BUY = "";
    public static string MONGODB_C_BAZAAR_SELL = "";
    public static string MONGODB_C_BAZAAR_ITEMS = "";
    public static string MONGODB_C_SESSIONS = "";
    public static string MONGODB_C_STARBOARDS = "";
    public static string MONGODB_C_STATE_DISCORD = "";
    public static string MONGODB_C_STATE_GENERAL = "";
    public static string MONGODB_C_STATE_HYPIXEL = "";
    public static string MONGODB_C_ROLL_STATS = "";
    public static string MONGODB_C_USERS = "";
    public static string MONGODB_D_DISCORD = "";
    public static string MONGODB_D_GENERAL = "";
    public static string MONGODB_D_HYPIXEL = "";
    public static string MONGODB_D_STATE = "";
    public static string MONGODB_OPTIONS = "";
    public static string MONGODB_SESSION_DAY_LENGTH = "";
    public static string PORT_WEBSOCKET = "";
    #endregion

    #region PUBLIC idc if these are exposed
    public static int PUBLIC_HYPIXEL_AUCTION_TIMER_MINUTES = 2;
    public static int PUBLIC_HYPIXEL_BAZAAR_TIMER_MINUTES = 1;
    #endregion

    /// <summary>
    /// Loads the settings.
    /// </summary>
    public static void Load()
    {
        LoadENV();
    }

    /// <summary>
    /// Loads the .env file.
    /// </summary>
    static void LoadENV()
    {
#if DEBUG
        string? line = null;
        using StreamReader streamReader = new StreamReader(".env");

        do
        {
            line = streamReader.ReadLine();

            if (line == null)
                continue;

            string[] lines = line.Split("=", 2);

            if (lines.Length != 2)
                throw new Exception("Illegal structure in the .env.");

            else if (lines[0] == "CERTIFICATE_LOCATION_LINUX")
                CERTIFICATE_LOCATION_LINUX = lines[1];
            else if (lines[0] == "CERTIFICATE_LOCATION_WINDOWS")
                CERTIFICATE_LOCATION_WINDOWS = lines[1];
            else if (lines[0] == "CERTIFICATE_PASSWORD")
                CERTIFICATE_PASSWORD = lines[1];
            else if (lines[0] == "DOCKER")
                DOCKER = lines[1];
            else if (lines[0] == "ENVIRONMENT")
                ENVIRONMENT = lines[1];
            else if (lines[0] == "HYPIXEL_API_BASE_URL")
                HYPIXEL_API_BASE_URL = lines[1];
            else if (lines[0] == "HYPIXEL_BOT_KEY")
                HYPIXEL_BOT_KEY = lines[1];
            else if (lines[0] == "MONGODB_BASE_URI")
                MONGODB_BASE_URI = lines[1];
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
            else if (lines[0] == "MONGODB_C_STATE_DISCORD")
                MONGODB_C_STATE_DISCORD = lines[1];
            else if (lines[0] == "MONGODB_C_STATE_GENERAL")
                MONGODB_C_STATE_GENERAL = lines[1];
            else if (lines[0] == "MONGODB_C_STATE_HYPIXEL")
                MONGODB_C_STATE_HYPIXEL = lines[1];
            else if (lines[0] == "MONGODB_C_USERS")
                MONGODB_C_USERS = lines[1];
            else if (lines[0] == "MONGODB_D_DISCORD")
                MONGODB_D_DISCORD = lines[1];
            else if (lines[0] == "MONGODB_D_HYPIXEL")
                MONGODB_D_HYPIXEL = lines[1];
            else if (lines[0] == "MONGODB_D_GENERAL")
                MONGODB_D_GENERAL = lines[1];
            else if (lines[0] == "MONGODB_D_STATE")
                MONGODB_D_STATE = lines[1];
            else if (lines[0] == "MONGODB_OPTIONS")
                MONGODB_OPTIONS = lines[1];
            else if (lines[0] == "MONGODB_SESSION_DAY_LENGTH")
                MONGODB_SESSION_DAY_LENGTH = lines[1];
            else if (lines[0] == "PORT_WEBSOCKET")
                PORT_WEBSOCKET = lines[1];
            else
                Utility.Log(Enums.LogLevel.WARN, $"The key was not implemented yet. Intentional? (at {lines[0]})");
        } while (line != null);

        streamReader.Close();
#else
    CERTIFICATE_LOCATION_LINUX=Environment.GetEnvironmentVariable("CERTIFICATE_LOCATION_LINUX");
    CERTIFICATE_LOCATION_WINDOWS=Environment.GetEnvironmentVariable("CERTIFICATE_LOCATION_WINDOWS");
    CERTIFICATE_PASSWORD=Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");
    DOCKER=Environment.GetEnvironmentVariable("DOCKER");
    ENVIRONMENT=Environment.GetEnvironmentVariable("ENVIRONMENT");
    HYPIXEL_API_BASE_URL=Environment.GetEnvironmentVariable("HYPIXEL_API_BASE_URL");
    HYPIXEL_BOT_KEY=Environment.GetEnvironmentVariable("HYPIXEL_BOT_KEY");
    MONGODB_BASE_URI=Environment.GetEnvironmentVariable("MONGODB_BASE_URI");
    MONGODB_C_AUCTION_BUY=Environment.GetEnvironmentVariable("MONGODB_C_AUCTION_BUY");
    MONGODB_C_AUCTION_ITEMS=Environment.GetEnvironmentVariable("MONGODB_C_AUCTION_ITEMS");
    MONGODB_C_AUCTION_TAGS=Environment.GetEnvironmentVariable("MONGODB_C_AUCTION_TAGS");
    MONGODB_C_BAZAAR_BUY=Environment.GetEnvironmentVariable("MONGODB_C_BAZAAR_BUY");
    MONGODB_C_BAZAAR_SELL=Environment.GetEnvironmentVariable("MONGODB_C_BAZAAR_SELL");
    MONGODB_C_BAZAAR_ITEMS=Environment.GetEnvironmentVariable("MONGODB_C_BAZAAR_ITEMS");
    MONGODB_C_SESSIONS=Environment.GetEnvironmentVariable("MONGODB_C_SESSIONS");
    MONGODB_C_STARBOARDS=Environment.GetEnvironmentVariable("MONGODB_C_STARBOARDS");
    MONGODB_C_ROLL_STATS=Environment.GetEnvironmentVariable("MONGODB_C_ROLL_STATS");
    MONGODB_C_USERS=Environment.GetEnvironmentVariable("MONGODB_C_USERS");
    MONGODB_D_DISCORD=Environment.GetEnvironmentVariable("MONGODB_D_DISCORD");
    MONGODB_D_GENERAL=Environment.GetEnvironmentVariable("MONGODB_D_GENERAL");
    MONGODB_D_HYPIXEL=Environment.GetEnvironmentVariable("MONGODB_D_HYPIXEL");
    MONGODB_OPTIONS=Environment.GetEnvironmentVariable("MONGODB_OPTIONS");
    MONGODB_SESSION_DAY_LENGTH=Environment.GetEnvironmentVariable("MONGODB_SESSION_DAY_LENGTH");
    PORT_WEBSOCKET=Environment.GetEnvironmentVariable("PORT_WEBSOCKET");
#endif
    }
}

