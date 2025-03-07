using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using MongoDB.Driver;

public partial class DiscordEvents
{
    public string NOSTAR_MESSAGE = "im rigging the election";

    [DiscordEvents]
    public void easter_egg_nostar()
    {
        _Client.MessageReceived += (_1) => EE_NoStar_MessageReceived(_1);
        _Client.ReactionAdded += (_1, _2, _3) => EE_NoStar_ReactionAdded(_1, _2, _3);
    }

    async Task EE_NoStar_MessageReceived(SocketMessage message)
    {
        if (message.Author.Id.ToString() != Settings.ID_BOT)
            return;

        if (message.Content != NOSTAR_MESSAGE)
            return;

        await Task.Delay(1000);
        await message.DeleteAsync();
    }

    async Task EE_NoStar_ReactionAdded(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        RestUserMessage restMessage = ((await message.GetOrDownloadAsync()) as RestUserMessage)!;

        if (restMessage.Author.Id.ToString() != Settings.ADMIN_1)
            return;

        if (reaction.Emote.Name != "nostar")
            return;

        await restMessage.Channel.SendMessageAsync(NOSTAR_MESSAGE);
        await restMessage.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
    }
}