// looks weird, but there are currently 2 ways I have setup to catch program exits
// 1. unhandled exceptions
// 2. abrupt program exit
// the purpose of this is to log the session into /logs

#if WINDOWS
    using System.Runtime.InteropServices;
#endif

public class Program
{
    public static HttpClient Client = default!;
    public static Utility Utility = default!;

#if WINDOWS
    // related to catching program exit method #2
    // P/Invoke and handler (Windows-only)
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    private delegate bool ConsoleEventDelegate(int eventType);
    static ConsoleEventDelegate? handler; // Nullable for conditional use
#endif

    static async Task Main()
    {
        Console.CancelKeyPress += (sender, e) => SaveSession();

#if WINDOWS
    // Windows-only setup
    handler = ConsoleEventCallback;
    SetConsoleCtrlHandler(handler, true);
#endif

        await Run();
    }

    static async Task Run()
    {
        try
        {
            Utility = new Utility();
            Client = new HttpClient();

            Settings.Load();
            await DiscordBot.Initialize();
            await MongoBot.Load();

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Add("API-Key", Settings.HYPIXEL_BOT_KEY);

            // keeps program running
            await Task.Delay(-1);
        }

        catch (Exception)
        {
            Console.WriteLine("catching...");
            SaveSession();
        }
    }

    static void SaveSession()
    {
        Utility.Log(Enums.LogLevel.NONE, "Closing...");
        using StreamWriter writer = new StreamWriter("./logs/" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".txt");
        writer.Write(Utility.LogLine);
    }


#if WINDOWS
    // Windows-specific callback
    private static bool ConsoleEventCallback(int eventType)
    {
        if (eventType == 2)
        {
            Console.WriteLine(eventType);
            SaveSession();
        }

        return false; // Allow default exit behavior
    }
#endif
}

