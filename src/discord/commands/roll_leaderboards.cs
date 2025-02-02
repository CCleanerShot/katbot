using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("roll_leaderboards", "see who's on top")]
    public async Task roll_leaderboards()
    {
        try
        {
            string result = "Fetching...";
            await RespondAsync(result);

            List<RollStats> allStats = (await MongoBot.RollStats.FindAsync(e => e.UserId != 1)).ToList();
            allStats.Sort((a, b) => b.Wins - a.Wins);

            await Context.Interaction.ModifyOriginalResponseAsync(e => e.Content = "");

            foreach (RollStats stats in allStats)
                result += $"<@{stats.UserId}>: {stats.Wins} Wins, {stats.Loses}";

        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}