using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ramojusd", "ahhhh")]
    public async Task ramojusd()
    {
        string[] ids =
        [
            Settings.ID_CARLOS,
            Settings.ID_KELINIMO,
            Settings.ID_RAMOJUSD,
            Settings.ID_VOLATILE,
        ];

        int chosenId = Utility.NextRange(0, ids.Length - 1);
        await RespondAsync($"<@{ids[chosenId]}> is this true?");
    }
}