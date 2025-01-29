using System.Reflection;
using Discord;
using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("obama", "obama")]
    public async Task obama()
    {
        FileAttachment file = new FileAttachment(Settings.PATH_OBAMA);
        file.Description = "Obama plays osu!";

        await RespondWithFileAsync(file);
    }
}