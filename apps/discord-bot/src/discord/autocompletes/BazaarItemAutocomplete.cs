#pragma warning disable CS1998 // misleading warning

using Discord;
using Discord.Interactions;

public class BazaarItemAutocomplete : AutocompleteHandler
{
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
    {
        string value = autocompleteInteraction.Data.Current.Value.ToString()!;

        IEnumerable<AutocompleteResult> results = MongoBot.CachedBazaarItems
            .Where(e => e.Value.Name.ToLower().Contains(value.ToLower()))
            .OrderBy(e => e.Value.Name)
            .Select(e => new AutocompleteResult(e.Value.Name, e.Value.ID));

        return AutocompletionResult.FromSuccess(results.Take(25));
    }
}