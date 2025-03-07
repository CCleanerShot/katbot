using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;

public partial class DiscordEvents
{
    [DiscordEvents]
    public void easter_egg_typing()
    {
        _Client.UserIsTyping += (_1, _2) => EE_Typing_UserIsTyping(_1, _2);
    }

    async Task EE_Typing_UserIsTyping(Cacheable<IUser, ulong> user, Cacheable<IMessageChannel, ulong> channel)
    {
        int chance = Program.Utility.NextRange(1, 1000);

        if (chance != 1)
            return;

        if (user.Id.ToString() == Settings.ADMIN_1)
            return;

        SocketTextChannel? textChannel = await channel.GetOrDownloadAsync() as SocketTextChannel;

        if (textChannel == null)
            return;

        await textChannel.SendMessageAsync($"<@{user.Id}> i see you typing");
    }
}