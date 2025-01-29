using Discord;

public partial class DiscordEvents
{
    string Emote = "⭐";
    int Threshold = 1;

    [DiscordEvents]
    public void starboards()
    {
        _Client.ReactionAdded += ReactionAdded;
        _Client.ReactionRemoved += ReactionRemoved;

    }

    async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, Discord.WebSocket.SocketReaction reaction)
    {
        if (reaction.Emote.Name != Emote)
            return;

        var emotes = await message.GetOrDownloadAsync().Result.GetReactionUsersAsync(new Emoji(Emote), 1000).FlattenAsync();

        if (emotes.Count() < Threshold)
            return;

        // Check if the message already exists in the starboards channel
        var starboardsChannel = _Client.GetChannelAsync((ulong)Settings.DISCORD_STARBOARDS_CHANNEL_ID);
    }

    async Task ReactionRemoved(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, Discord.WebSocket.SocketReaction reaction)
    {
        if (reaction.Emote.Name != Emote)
            return;

        var emotes = await message.GetOrDownloadAsync().Result.GetReactionUsersAsync(new Emoji(Emote), 1000).FlattenAsync();

        if (emotes.Count() > Threshold)
            return;
    }
}