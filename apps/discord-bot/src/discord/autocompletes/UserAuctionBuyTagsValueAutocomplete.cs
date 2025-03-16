#pragma warning disable CS1998 // misleading warning

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class UserAuctionBuyTagsValueAutocomplete : AutocompleteHandler
{
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
    {
        string itemID = (context.Interaction.Data as SocketAutocompleteInteractionData)!.Options.ElementAt(0).Value.ToString()!;
        string tagName = (context.Interaction.Data as SocketAutocompleteInteractionData)!.Options.ElementAt(1).Value.ToString()!;
        string value = autocompleteInteraction.Data.Current.Value.ToString()!;

        IEnumerable<AutocompleteResult> results = MongoBot.CachedAuctionBuys
            .Find(e => e.ID == itemID && e.UserId == context.User.Id)!.AuctionTags
                .Where(e => e.Value.ToLower().Contains(value.ToLower()))
                .OrderBy(e => e.Value)
                .Select(e => new AutocompleteResult(e.Value, e.Value));

        return AutocompletionResult.FromSuccess(results.Take(25));
    }
}