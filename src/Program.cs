using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program
{
    static async Task Main()
    {
        Settings.Load();
        await DiscordBot.Initialize();
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();

        if (Settings.HYPIXEL_BOT_KEY == "")
            Utility.Log(Enums.LogLevel.WARN, "NOTE: no Hypixel API key found!");
        else
            client.DefaultRequestHeaders.Add("API-Key", Settings.HYPIXEL_BOT_KEY);

        await BazaarRoute.GetRoute(client);

        // keeps program running
        await Task.Delay(-1);
    }
}

