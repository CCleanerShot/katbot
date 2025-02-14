using Discord;
using Discord.Rest;
using Discord.WebSocket;
using MongoDB.Driver;

struct CommandTag
{
    string Name;
    string Value;
}

public partial class DiscordEvents
{
    string QUICK_RESPONSE = "cool";

    Dictionary<string, List<CommandTag>> UserCommands = new Dictionary<string, List<CommandTag>>();

    /// <summary>
    /// NOTE: keep in mind that the values (except from the select options are not granted to you
    /// cross-component). you will need to store them locally here.
    /// </summary>
    [DiscordEvents]
    public void ah_attributes()
    {
        _Client.ButtonExecuted += (_1) => ButtonExecuted(_1);
        _Client.SelectMenuExecuted += (_1) => SelectMenuExecuted(_1);
        _Client.MessageReceived += (_1) => MessageReceived(_1);
    }

    async Task _PropertyButtonAdd(SocketMessageComponent context)
    {
        // string value = context.Data.Values.First();

        // IReadOnlyCollection<ActionRowComponent> components = context.Message.Components;

        // ActionRowComponent newRow = new ActionRowBuilder()
        // .WithSelectMenu(new SelectMenuBuilder()
        //     .WithCustomId($"property-{value}")
        //     .WithPlaceholder("test")
        //     .AddOption(new SelectMenuOptionBuilder()
        //         .WithLabel(value)
        //         .WithValue(value)))
        //         .Build();

        await context.ModifyOriginalResponseAsync((e) =>
        {
            e.Components = new Discord.Optional<MessageComponent>();
            // e.Components.Value.Components.Append(newRow);
        });

        Console.WriteLine("okay");
    }

    async Task _PropertyButtonRemove(SocketMessageComponent context)
    {

    }

    async Task _MenuAdd(SocketMessageComponent context)
    {
        RestInteractionMessage message = await context.GetOriginalResponseAsync();

        await message.ModifyAsync(m =>
        {
            m.Components = new ComponentBuilder()
                .WithButton("New Button", "btn-id", ButtonStyle.Primary)
                .Build();
        });
    }

    async Task ButtonExecuted(SocketMessageComponent context)
    {
        if (context.Data.CustomId == Settings.PUBLIC_AH_PROPERTY_ADD_PROPERTY_BUTTON)
            await _PropertyButtonAdd(context);

        else if (context.Data.CustomId == Settings.PUBLIC_AH_PROPERTY_REMOVE_PROPERTY_BUTTON)
            await _PropertyButtonRemove(context);

        await context.DeferAsync();
    }


    async Task SelectMenuExecuted(SocketMessageComponent context)
    {
        if (context.Data.CustomId == Settings.PUBLIC_AH_PROPERTY_ADD_PROPERTY_MENU)
            await _MenuAdd(context);

        await context.DeferAsync();
    }

    async Task MessageReceived(SocketMessage message)
    {
        if (message.Author.Id.ToString() != Settings.ID_BOT)
            return;

        if (message.Content != QUICK_RESPONSE)
            return;

        Console.WriteLine(message.Content, message.Id);
        await message.DeleteAsync();
    }
}