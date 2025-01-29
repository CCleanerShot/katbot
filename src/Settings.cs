using System.Reflection;

/// <summary>
/// Holds data regarding constant runtime-specifics of the app.
/// </summary>
public class Settings
{
    public static string ADMIN_1 = "";
    public static string ADMIN_2 = "";
    public static ulong DISCORD_APPLICATION_ID = 0;
    public static ulong DISCORD_MAIN_CHANNEL_ID = 0;
    public static string DISCORD_PUBLIC_KEY = "";
    public static ulong DISCORD_SEND_CHANNEL_ID = 0;
    public static ulong DISCORD_STARBOARDS_CHANNEL_ID = 0;
    public static string DISCORD_TOKEN = "";
    public static string HYPIXEL_BOT_KEY = "";
    public static string ID_CARLOS = "";
    public static string ID_KELINIMO = "";
    public static string ID_RAMOJUSD = "";
    public static string ID_VOLATILE = "";
    public static string PATH_OBAMA = "";
    public static string PATH_RAMOJUSD_GIF_URL = "https://cdn.discordapp.com/attachments/1218190610247323751/1334044487223087155/ramojusd.gif?ex=679b193f&is=6799c7bf&hm=338b1002ce5e2367c193d4f5500bbbf9ec8ed7b452f571b4174a1fb54cce6452&";

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

            string[] lines = line.Split("=");

            if (lines.Length != 2)
                throw new Exception("Illegal structure in the .env.");

            if (lines[0] == "ADMIN_1")
                ADMIN_1 = lines[1];
            else if (lines[0] == "ADMIN_2")
                ADMIN_2 = lines[1];
            else if (lines[0] == "DISCORD_APPLICATION_ID")
                DISCORD_APPLICATION_ID = UInt64.Parse(lines[1]);
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
            else if (lines[0] == "HYPIXEL_BOT_KEY")
                HYPIXEL_BOT_KEY = lines[1];
            else if (lines[0] == "ID_CARLOS")
                ID_CARLOS = lines[1];
            else if (lines[0] == "ID_KELINIMO")
                ID_KELINIMO = lines[1];
            else if (lines[0] == "ID_RAMOJUSD")
                ID_RAMOJUSD = lines[1];
            else if (lines[0] == "ID_VOLATILE")
                ID_VOLATILE = lines[1];
            else
                Utility.Log(Enums.LogLevel.WARN, "The key was not implemented yet. Intentional?");

        } while (line != null);
    }
}

