using Discord;
using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_add_buy", "adds an auction item to your list for buying. NOTE: you can specify properties after!")]
    public async Task ah_add_buy(
    [Summary("item", "the item to track (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(AuctionItemAutocomplete))] string itemID,
    [Summary("buy_price", "the maximum buy price (alerts if lower)")] ulong buyPrice,
    [Summary("remove_after", "whether or not to remove this item after it alerts the user")] bool removeAfter = true
    )
    {
        try
        {
            ButtonBuilder addButton = new ButtonBuilder()
                .WithCustomId(Settings.PUBLIC_AH_PROPERTY_ADD_PROPERTY_BUTTON)
                .WithStyle(ButtonStyle.Primary)
                .WithLabel("ADD PROPERTY");

            SelectMenuBuilder addSelect = new SelectMenuBuilder()
                .WithCustomId(Settings.PUBLIC_AH_PROPERTY_ADD_PROPERTY_MENU)
                .WithPlaceholder("Choose a property...");

            AuctionItemsAll item = MongoBot.CachedAuctionItems[itemID];
            List<AuctionTags> tags = item.ExtraAttributes.Select(e => MongoBot.CachedAuctionTags[e]).ToList();

            foreach (AuctionTags tag in tags)
                addSelect.AddOption(new SelectMenuOptionBuilder()
                    .WithLabel(tag.Name)
                    .WithValue(tag.Name));

            MessageComponent response = new ComponentBuilder()
                .WithRows([new ActionRowBuilder().WithButton(addButton)])
                .WithSelectMenu(addSelect)
                .Build();

            await RespondAsync("Done. Here, you can add properties for your item:", null, false, false, null, null, response);
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}