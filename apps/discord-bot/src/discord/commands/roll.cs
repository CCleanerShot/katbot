using System.Reflection;
using System.Text.RegularExpressions;
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

            bool found = false;
            var matchesWithIds = RollMatch.RollMatches.Select(e => e.Users.Select(ee => ee.Id).ToList()).ToList();

            foreach (List<ulong> match in matchesWithIds)
            {
                if (match.Contains(user1.Id) && match.Contains(user2.Id))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                await RespondAsync($"You guys already have a match >:(");
            }

            else
            {
                RollMatch match = new RollMatch(channel, user1, user2);
                await RespondAsync($"<@{user2.Id}>, you have been challenged to a roll battle! Type to '!roll' to continue. First to 1000 wins.");
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
    SocketGuildUser PlayerTurn;
    SocketGuildUser User1;
    SocketGuildUser User2;

    public string FullMessage = "";
    public readonly List<RestUserMessage> RestMessages = new List<RestUserMessage>();
    public readonly List<SocketMessage> SocketMessages = new List<SocketMessage>();
    public readonly List<SocketGuildUser> Users; // to not re-construct an array everything, but has little practical use outside of optimization

    /// <summary>
    /// The constructor here itself causes it's own event to the discord bot.
    /// </summary>
    /// <param name="_Channel"></param>
    /// <param name="_User1"></param>
    /// <param name="_User2"></param>
    public RollMatch(SocketTextChannel _Channel, SocketGuildUser _User1, SocketGuildUser _User2)
    {
        Channel = _Channel;
        User1 = _User1;
        User2 = _User2;
        PlayerTurn = User2;
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
        List<RollStats> winner = await MongoBot.RollStats.FindList(e => e.UserId == winnerDiscord.Id);
        List<RollStats> loser = await MongoBot.RollStats.FindList(e => e.UserId == loserDiscord.Id);

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

        foreach (SocketMessage message in SocketMessages)
            await message.DeleteAsync();

        foreach (RestMessage message in RestMessages)
            await message.DeleteAsync();

        string lastMessage = $":confetti_ball: <@{winnerDiscord.Id}> has rolled 1000 and won! :confetti_ball: ";
        FullMessage += $"\n{lastMessage}";
        await Channel.SendMessageAsync(FullMessage);
    }

    async Task _MessageReceived(SocketMessage message)
    {
        if (message.Channel.Id != Channel.Id)
            return;

        if (PlayerTurn.Id != message.Author.Id)
            return;

        if (message.Content != "!roll")
            return;

        SocketMessages.Add(message);
        CurrentRoll = Program.Utility.NextRange(CurrentRoll, MaxRoll);

        if (CurrentRoll != MaxRoll)
        {
            SocketGuildUser cV = Users.Find(e => e.Id == PlayerTurn.Id)!;
            SocketGuildUser nV = Users.Find(e => e.Id != PlayerTurn.Id)!;
            PlayerTurn = nV;
            string stringMessage = $"<@{cV.Id}> rolled a {CurrentRoll}! <@{nV.Id}>, !roll.";
            RestMessages.Add(await message.Channel.SendMessageAsync(stringMessage));
            FullMessage += $"\n{stringMessage}";
        }

        else
            await ConcludeMatch();
    }
}