public class Program
{
    public static HttpClient Client = default!;

    static async Task Main()
    {
        Settings.Load();
        await DiscordBot.Initialize();
        await MongoBot.Load();

        Client = new HttpClient();
        Client.DefaultRequestHeaders.Accept.Clear();

        if (Settings.HYPIXEL_BOT_KEY == "")
            Utility.Log(Enums.LogLevel.WARN, "NOTE: no Hypixel API key found!");
        else
            Client.DefaultRequestHeaders.Add("API-Key", Settings.HYPIXEL_BOT_KEY);

        // keeps program running
        await Task.Delay(-1);
    }
}

