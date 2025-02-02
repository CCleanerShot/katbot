// looks weird, but there are currently 2 ways I have setup to catch program exits
// 1. unhandled exceptions
// 2. abrupt program exit
// the purpose of this is to log the session into /logs

using System.Runtime.InteropServices;

public class Program
{
    public static HttpClient Client = default!;
    public static Utility Utility = default!;


    // related to catching program exit method #2
    private delegate bool ConsoleEventDelegate(int eventType);
    static ConsoleEventDelegate handler = default!;
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    static async Task Main()
    {
        Console.CancelKeyPress += (sender, e) => SaveSession();

        handler = new ConsoleEventDelegate(ConsoleEventCallback);
        SetConsoleCtrlHandler(handler, true);
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


    static bool ConsoleEventCallback(int eventType)
    {
        if (eventType == 2)
        {
            Console.WriteLine(eventType);
            SaveSession();
        }

        return false;
    }
}

