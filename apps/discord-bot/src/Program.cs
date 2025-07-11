﻿// looks weird, but there are currently 2 ways I have setup to catch program exits
// 1. unhandled exceptions
// 2. abrupt program exit
// the purpose of this is to log the session into /logs

public class Program
{
    public static HttpClient Client = default!;
    public static Utility Utility = default!;

    static async Task Main()
    {
        Console.CancelKeyPress += (sender, e) => SaveSession();
        await Run();
    }

    static async Task Run()
    {
        try
        {
            Utility = new Utility();
            Client = new HttpClient();

            Utility.Log(Enums.LogLevel.NONE, "Initializing...");
            Settings.Load();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Add("API-Key", Settings.HYPIXEL_BOT_KEY);
            await MongoBot.Load();
            await DiscordBot.Initialize();

            // // keeps program running
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

        if (!Directory.Exists("logs"))
            Directory.CreateDirectory("logs");

        using StreamWriter writer = new StreamWriter("./logs/" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm") + ".txt", true);
        writer.Write(Utility.LogLine);
    }
}

