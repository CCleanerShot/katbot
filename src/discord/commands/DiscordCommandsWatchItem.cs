using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("test", "echo echo")]
    public async Task WatchItem(string input)
    {
        await ReplyAsync("true");
    }
}