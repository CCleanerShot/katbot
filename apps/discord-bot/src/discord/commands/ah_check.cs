using Discord.Interactions;
using MongoDB.Driver;

enum WantedAuctionTable
{
    NAME,
    WANTED_PRICE,
    WANTED_PROPERTIES,
}

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_check", "checks your current auction watchlist")]
    public async Task ah_check()
    {
        try
        {
            List<AuctionBuy> buys = MongoBot.CachedAuctionBuys.Where(e => e.UserId == Context.User.Id).ToList();
            DiscordTable<WantedAuctionTable> discordTable = new DiscordTable<WantedAuctionTable>("WANTED ITEMS");

            foreach (AuctionBuy auctionBuy in buys)
            {
                string wantedProperties = "";

                for (int i = 0; i < auctionBuy.AuctionTags.Count; i++)
                {
                    AuctionTag attribute = auctionBuy.AuctionTags[i];

                    if (MongoBot.CachedAuctionTags[attribute.Name].Type == Cyotek.Data.Nbt.TagType.Compound)
                        wantedProperties += $"{attribute.Name} [{attribute.Value}]";
                    else
                        wantedProperties += $"{attribute.Name} {attribute.Value}";

                    if (i < auctionBuy.AuctionTags.Count - 1)
                        wantedProperties += ", ";
                }

                discordTable.Table[WantedAuctionTable.NAME].Add(auctionBuy.Name);
                discordTable.Table[WantedAuctionTable.WANTED_PRICE].Add(auctionBuy.Price.ToString());
                discordTable.Table[WantedAuctionTable.WANTED_PROPERTIES].Add(wantedProperties);
            }

            string result = discordTable.Construct();
            await RespondAsync(result);
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}