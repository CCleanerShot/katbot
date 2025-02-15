using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;

public partial class DiscordEvents
{
    [DiscordEvents]
    public void easter_egg_ping()
    {
        _Client.MessageReceived += (_1) => EE_Ping_MessageReceived(_1);
    }

    async Task EE_Ping_MessageReceived(SocketMessage message)
    {
        int chance = Program.Utility.NextRange(1, 100);

        if (chance != 1)
            return;

        if (message.MentionedUsers.Any(e => e.Id.ToString() == Settings.ID_BOT))
            return;

        await message.Channel.SendMessageAsync("Who ping me?");
    }
}