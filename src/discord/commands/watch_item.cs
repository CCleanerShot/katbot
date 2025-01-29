using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("watch_item", "echo echo")]
    public async Task watch_item(string input)
    {
        await ReplyAsync("true");
    }
}