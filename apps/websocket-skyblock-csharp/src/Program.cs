// looks weird, but there are currently 2 ways I have setup to catch program exits
// 1. unhandled exceptions
// 2. abrupt program exit
// the purpose of this is to log the session into /logs

using System.Net;
using System.Net.Sockets;
using oslo.binary;

public class Program
{
    public static BigEndian BigEndian = new BigEndian();
    public static HttpClient Client = new HttpClient();

    static async Task Main()
    {
        Console.CancelKeyPress += (sender, e) => SaveSession();
        await Run();
    }

    static async Task Run()
    {
        try
        {
            Settings.Load();
            TimerBot.Load();
            await MongoBot.Load();
            WebSocketBot.Load();
#pragma warning disable CS4014
            HTTPBot.Load();
#pragma warning restore CS4014

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Add("API-Key", Settings.HYPIXEL_BOT_KEY);

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
        using StreamWriter writer = new StreamWriter("./logs/" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".txt", true);
        writer.Write(Utility.LogLine);
    }
}

