#pragma warning disable CS1998 // misleading warning

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class AuctionTagsAutocomplete : AutocompleteHandler
{
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
    {
        string itemID = (context.Interaction.Data as SocketAutocompleteInteractionData)!.Options.ElementAt(0).Value.ToString()!;
        string value = autocompleteInteraction.Data.Current.Value.ToString()!;

        IEnumerable<AutocompleteResult> results = MongoBot.CachedAuctionItems[itemID].ExtraAttributes
            .Where(e => e.ToLower().Contains(value.ToLower()))
            .OrderBy(e => e)
            .Select(e => new AutocompleteResult(e, e));

        return AutocompletionResult.FromSuccess(results.Take(25));
    }
}