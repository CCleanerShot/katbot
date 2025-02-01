using Discord;
using Discord.Interactions;
using Discord.WebSocket;

/// <summary>
/// Create commands inside the /commands folder.
/// </summary>
public partial class DiscordCommands : InteractionModuleBase
{
    [AutocompleteCommand("watch_buy_item", "watch_buy_item")]
    public async Task AutocompleteFunction()
    {
        string userInput = (Context.Interaction as SocketAutocompleteInteraction)!.Data.Current.Value.ToString()!;

        IEnumerable<AutocompleteResult> results = new[]
        {
        new AutocompleteResult("foo", "foo_value"),
        new AutocompleteResult("bar", "bar_value"),
        new AutocompleteResult("baz", "baz_value"),
    }.Where(x => x.Name.StartsWith(userInput, StringComparison.InvariantCultureIgnoreCase)); // only send suggestions that starts with user's input; use case insensitive matching

        // max - 25 suggestions at a time
        await (Context.Interaction as SocketAutocompleteInteraction)!.RespondAsync(results.Take(25));
    }
}