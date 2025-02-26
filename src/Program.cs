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
    delegate bool ConsoleEventDelegate(int eventType);

    static ConsoleEventDelegate Handler;
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

    private static bool ConsoleEventCallback(int eventType)
    {
        if (eventType == 2)
        {
            Console.WriteLine(eventType);
            SaveSession();
        }

        return false;
    }
#endif

    static async Task Main()
    {
        Console.CancelKeyPress += (sender, e) => SaveSession();

#if WINDOWS
    Handler = ConsoleEventCallback;
    SetConsoleCtrlHandler(Handler, true);
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
            AspnetBot.Start();

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Add("API-Key", Settings.HYPIXEL_BOT_KEY);

            // keeps program running
            await Task.Delay(-1);
        }

        catch (Exception e)
        {
            Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            SaveSession();
        }
    }

    static void SaveSession()
    {
        Utility.Log(Enums.LogLevel.NONE, "Saving log to file...");
        using StreamWriter writer = new StreamWriter("./logs/" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".txt");
        writer.Write(Utility.LogLine);
    }




}

