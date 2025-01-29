using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ramojusd", "ahhhh")]
    public async Task Ramojusd()
    {
        await ReplyAsync("is this true?");
    }
}