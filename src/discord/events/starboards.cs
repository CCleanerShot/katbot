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
    int Starboards_Threshold = 1;

    [DiscordEvents]
    public void starboards()
    {
        _Client.ReactionAdded += (_1, _2, _3) => ReactionChanged(_1, _2, _3, ReactionCase.ADD);
        _Client.ReactionRemoved += (_1, _2, _3) => ReactionChanged(_1, _2, _3, ReactionCase.REMOVE);
    }

    async Task ReactionChanged(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, Discord.WebSocket.SocketReaction reaction, ReactionCase reactionCase)
    {
        if (reaction.Emote.Name != Starboards_Emote)
            return;

        SocketTextChannel? starboardsChannel = await _Client.GetChannelAsync(Settings.DISCORD_STARBOARDS_CHANNEL_ID) as SocketTextChannel;

        if (starboardsChannel == null)
        {
            Utility.Log(Enums.LogLevel.WARN, "Could not found the starboards channel! Was it deleted?");
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


        List<Starboards>? response = (await MongoBot.Starboards.FindAsync(e => e.MessageStarredId == message.Id)).ToList();
        IUserMessage userMessage = await message.GetOrDownloadAsync();
        EmbedFooterBuilder footer = new EmbedFooterBuilder()
            .WithText($"{Starboards_Emote}{users!.Count()} • {userMessage.CreatedAt.UtcDateTime.ToString("M/d/yyyy HH:mm:ss")}");

        // helper function
        async Task _ModifyMessage()
        {
            RestUserMessage? existingMessage = await starboardsChannel!.GetMessageAsync(response[0].MessageId) as RestUserMessage;

            if (existingMessage == null)
            {
                Utility.Log(Enums.LogLevel.WARN, "Found the message in the database, but the message has disappeared.");
                return;
            }

            Embed newEmbed = existingMessage.Embeds.First().ToEmbedBuilder().WithFooter(footer).Build();
            await existingMessage.ModifyAsync((e) =>
            {
                e.Embed = newEmbed;
            });
        }

        switch (reactionCase)
        {
            case ReactionCase.ADD:
                if (response.Count != 0)
                    break;

                try
                {
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

                    RestUserMessage discordResponse = await starboardsChannel.SendMessageAsync("", false, embedBuilder.Build());

                    Starboards starboardMessage = new Starboards()
                    {
                        MessageId = discordResponse.Id,
                        MessageStarredId = message.Id,
                    };

                    await MongoBot.Starboards.InsertOneAsync(starboardMessage);
                }

                catch (Exception)
                {
                    Utility.Log(Enums.LogLevel.ERROR, $"An unexpected error has occured while creating the starboard message! (Message ID: {userMessage.Id})");
                    throw;
                }

                break;
            case ReactionCase.REMOVE:
            default:
                break;
        }

        await _ModifyMessage();
    }
}