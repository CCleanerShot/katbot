using Discord;
using Discord.Rest;
using Discord.WebSocket;
using MongoDB.Driver;

enum ReactionCase
{
    ADD,
    REMOVE,
}

public partial class DiscordEvents
{
    /// <summary>
    /// The emote that the starboard checks for.
    /// </summary>
    string Starboards_Emote = "⭐";
    /// <summary>
    /// The threshold at which a new starboard is added for a message.
    /// </summary>
    int Starboards_Threshold = 3;

    [DiscordEvents]
    public void starboards()
    {
        _Client.ReactionAdded += (_1, _2, _3) => Starboards_ReactionChanged(_1, _2, _3, ReactionCase.ADD);
        _Client.ReactionRemoved += (_1, _2, _3) => Starboards_ReactionChanged(_1, _2, _3, ReactionCase.REMOVE);
    }

    async Task Starboards_ReactionChanged(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, Discord.WebSocket.SocketReaction reaction, ReactionCase reactionCase)
    {
        RestUserMessage restMessage = ((await message.GetOrDownloadAsync()) as RestUserMessage)!;

        if (restMessage.Channel is not SocketGuildChannel guildChannel)
            return;

        if (guildChannel.Guild.Id == Settings.TEST_DISCORD_GUILD_ID)
            return;

        if (reaction.Emote.Name != Starboards_Emote)
            return;

        SocketTextChannel? starboardsChannel = await _Client.GetChannelAsync(Settings.DISCORD_STARBOARDS_CHANNEL_ID) as SocketTextChannel;

        if (starboardsChannel == null)
        {
            Program.Utility.Log(Enums.LogLevel.WARN, "Could not found the starboards channel! Was it deleted?");
            return;
        }

        IEnumerable<IUser> users = await message.GetOrDownloadAsync().Result.GetReactionUsersAsync(new Emoji(Starboards_Emote), 1000).FlattenAsync();

        // additional conditionals
        switch (reactionCase)
        {
            case ReactionCase.ADD:
                if (users.Count() < Starboards_Threshold)
                    return;
                break;
            case ReactionCase.REMOVE:
                break;
            default:
                break;
        }

        IUserMessage userMessage = await message.GetOrDownloadAsync();
        List<Starboards> response = await MongoBot.Starboards.FindList(e => e.MessageStarredId == message.Id);
        EmbedFooterBuilder footer = new EmbedFooterBuilder().WithText($"{Starboards_Emote}{users!.Count()} • {userMessage.CreatedAt.UtcDateTime.ToString("M/d/yyyy HH:mm:ss")}");
        RestUserMessage existingMessage;

        if (response.Count != 0)
            existingMessage = (await starboardsChannel!.GetMessageAsync(response.First().MessageId) as RestUserMessage)!;
        else
        {
            // a new starboard should never be below
            if (users.Count() < Starboards_Threshold)
                return;

            EmbedFieldBuilder authorField = new EmbedFieldBuilder()
                .WithName("Author")
                .WithIsInline(true)
                .WithValue($"<@{userMessage.Author.Id}>");
            EmbedFieldBuilder sourceField = new EmbedFieldBuilder()
                .WithName("Channel")
                .WithIsInline(true)
                .WithValue($"{userMessage.GetJumpUrl()}");
            EmbedFieldBuilder messageField = new EmbedFieldBuilder()
                .WithName("Message")
                .WithValue("N/A");
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithFields([authorField, sourceField, messageField])
                .WithFooter(footer);

            if (userMessage.Content != "" && userMessage.Content != null)
                messageField.WithValue($"{userMessage.Content}");

            if (userMessage.Attachments.Count > 0)
                embedBuilder.WithImageUrl(userMessage.Attachments.First().Url);

            if (userMessage.Embeds.Count > 0)
                if (userMessage.Embeds.First().Url != null && userMessage.Embeds.First().Url != "")
                    embedBuilder.WithImageUrl(userMessage.Embeds.First().Url);

            existingMessage = await starboardsChannel.SendMessageAsync("", false, embedBuilder.Build());
            await MongoBot.Starboards.InsertOneAsync(new Starboards() { MessageId = existingMessage.Id, MessageStarredId = message.Id });
        }

        switch (reactionCase)
        {
            case ReactionCase.ADD:
                Program.Utility.Log(Enums.LogLevel.NONE, $"{existingMessage.Author.GlobalName} added a star (message: {existingMessage.Content}, id: {existingMessage.Id}).");
                break;
            case ReactionCase.REMOVE:
                Program.Utility.Log(Enums.LogLevel.NONE, $"{existingMessage.Author.GlobalName} removed a star (message: {existingMessage.Content}, id: {existingMessage.Id}).");
                break;
            default:
                break;
        }

        Embed newEmbed = existingMessage.Embeds.First().ToEmbedBuilder().WithFooter(footer).Build();
        await existingMessage.ModifyAsync((e) =>
        {
            e.Embed = newEmbed;
        });
    }
}