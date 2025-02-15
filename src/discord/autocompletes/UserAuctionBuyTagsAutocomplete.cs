#pragma warning disable CS1998 // misleading warning

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class UserAuctionBuyTagsAutocomplete : AutocompleteHandler
{
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
    {
        string itemID = (context.Interaction.Data as SocketAutocompleteInteractionData)!.Options.ElementAt(0).Value.ToString()!;
        string value = autocompleteInteraction.Data.Current.Value.ToString()!;
        Console.WriteLine($"remove {itemID}");

        IEnumerable<AutocompleteResult> results = MongoBot.CachedAuctionBuys
            .Find(e => e.ID == itemID && e.UserId == context.User.Id)!.ExtraAttributes
                .DistinctBy(e => e.Name)
                .Where(e => e.Name.ToLower().Contains(value.ToLower()))
                .OrderBy(e => e)
                .Select(e => new AutocompleteResult(e.Name, e.Name));

        return AutocompletionResult.FromSuccess(results.Take(25));
    }
}