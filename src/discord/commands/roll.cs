using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("roll", "first person to roll 1000 wins")]
    public async Task roll(
        [Summary("target", "player to fight with")] IUser targetUser
    )
    {
        try
        {
            SocketTextChannel? channel = Context.Channel as SocketTextChannel;
            SocketGuildUser? user1 = Context.User as SocketGuildUser;
            SocketGuildUser? user2 = targetUser as SocketGuildUser;

            if (channel == null || user1 == null || user2 == null)
                throw new Exception("One of the values unexpectedly returned null!");

            if (
                RollMatch.RollMatches.Find(e => e.Users.Select(e => e.Id).Contains(user1.Id)) != null &&
                RollMatch.RollMatches.Find(e => e.Users.Select(e => e.Id).Contains(user2.Id)) != null
            )
            {
                await RespondAsync($"You guys already have a match >:(");
            }

            else
            {
                await RespondAsync($"You've successfully challenged <@{user2.Id}> to a duel!", null, false, true);
                RestUserMessage message = (await Context.Channel.SendMessageAsync($"<@{user2.Id}>, you have been challenged to a roll battle! Type to '!roll' to continue. First to 1000 wins.") as RestUserMessage)!;
                new RollMatch(channel, user1, user2, message);
            }
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}

class RollMatch
{
    static int MaxRoll = 1000;
    public static List<RollMatch> RollMatches = new List<RollMatch>();

    SocketTextChannel Channel;
    int CurrentRoll = 0;
    RestUserMessage FullMessage;
    SocketGuildUser PlayerTurn;
    RestUserMessage? PreviousMessage;
    SocketGuildUser User1;
    SocketGuildUser User2;

    public readonly List<SocketGuildUser> Users; // to not re-construct an array everything, but has little practical use outside of optimization

    /// <summary>
    /// The constructor here itself causes it's own event to the discord bot.
    /// </summary>
    /// <param name="_Channel"></param>
    /// <param name="_User1"></param>
    /// <param name="_User2"></param>
    public RollMatch(SocketTextChannel _Channel, SocketGuildUser _User1, SocketGuildUser _User2, RestUserMessage _FullMessage)
    {
        Channel = _Channel;
        FullMessage = _FullMessage;
        User1 = _User1;
        User2 = _User2;
        PlayerTurn = User1;
        RollMatches.Add(this);
        Users = new List<SocketGuildUser>() { User1, User2 };
        DiscordBot._Client.MessageReceived += _MessageReceived;
    }

    async Task ConcludeMatch()
    {
        // even if the next operations fail, it should just remove these
        RollMatches.Remove(this);
        DiscordBot._Client.MessageReceived -= _MessageReceived;

        SocketGuildUser winnerDiscord = Users.Find(e => e.Id == PlayerTurn.Id)!;
        SocketGuildUser loserDiscord = Users.Find(e => e.Id != PlayerTurn.Id)!;
        List<RollStats> winner = (await MongoBot.RollStats.FindAsync(e => e.UserId == winnerDiscord.Id)).ToList();
        List<RollStats> loser = (await MongoBot.RollStats.FindAsync(e => e.UserId == loserDiscord.Id)).ToList();

        if (winner.Count == 0)
        {
            RollStats newPlayer = new RollStats(winnerDiscord.Id) { Wins = 1 };
            await MongoBot.RollStats.InsertOneAsync(newPlayer);
        }

        if (loser.Count == 0)
        {
            RollStats newPlayer = new RollStats(loserDiscord.Id) { Loses = 1 };
            await MongoBot.RollStats.InsertOneAsync(newPlayer);
        }

        if (winner.Count != 0)
        {
            RollStats replacement = winner.First();
            replacement.Wins++;
            await MongoBot.RollStats.FindOneAndReplaceAsync(e => e.UserId == winnerDiscord.Id, replacement);
        }

        if (loser.Count != 0)
        {
            RollStats replacement = loser.First();
            replacement.Loses++;
            await MongoBot.RollStats.FindOneAndReplaceAsync(e => e.UserId == loserDiscord.Id, replacement);
        }

        string finalMessage = $":confetti_ball: <@{winnerDiscord.Id}> has rolled 1000 and won! :confetti_ball:";
        await FullMessage.ModifyAsync((e) => { e.Content = $"{e.Content}\n{finalMessage}"; });
        await Channel.SendMessageAsync(finalMessage);
    }

    async Task _MessageReceived(SocketMessage message)
    {
        if (message.Channel.Id != Channel.Id)
            return;

        if (PlayerTurn.Id != message.Author.Id)
            return;

        if (message.Content != "!roll")
            return;

        if (PreviousMessage != null)
            await PreviousMessage.DeleteAsync();

        await message.DeleteAsync();
        CurrentRoll = Program.Utility.NextRange(CurrentRoll, MaxRoll);

        if (CurrentRoll != MaxRoll)
        {
            SocketGuildUser cV = Users.Find(e => e.Id == PlayerTurn.Id)!;
            SocketGuildUser nV = Users.Find(e => e.Id != PlayerTurn.Id)!;
            PlayerTurn = nV;
            await FullMessage.ModifyAsync((e) => { e.Content = $"{e.Content}\n<@{cV.Id}> rolled a {CurrentRoll}!"; });
            PreviousMessage = await message.Channel.SendMessageAsync($"<@{cV.Id}>, !roll.");
        }

        else
            await ConcludeMatch();
    }
}