#pragma warning disable CS1998 // misleading warning

using Discord;
using Discord.Interactions;

public class UserBazaarBuysAutocomplete : AutocompleteHandler
{
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
    {
        string value = autocompleteInteraction.Data.Current.Value.ToString()!;

        // there are two ".Where" that increases compute time but are negligible. for readability.
        IEnumerable<AutocompleteResult> results = MongoBot.CachedBazaarBuys
            .Where(e => e.UserId == context.User.Id)
            .Where(e => e.Name.ToLower().Contains(value.ToLower()))
            .OrderBy(e => e.Name)
            .Select(e => new AutocompleteResult(e.Name, e.ID));

        return AutocompletionResult.FromSuccess(results.Take(25));
    }
}