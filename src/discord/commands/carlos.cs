using System.Reflection;
using Discord;
using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("carlos", "carlos")]
    public async Task carlos()
    {
        FileAttachment file = new FileAttachment(Settings.OBAMA_PATH);
        file.Description = "Obama plays osu!";

        // await RespondWithModalAsync();
        // TODO: implement
    }
}