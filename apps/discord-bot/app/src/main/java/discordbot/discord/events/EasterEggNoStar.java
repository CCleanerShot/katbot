package discordbot.discord.events;

import discordbot.Main;
import discordbot.BotSettings;
import discordbot.common.Enums.LogLevel;
import discordbot.common.Enums.Settings;
import net.dv8tion.jda.api.hooks.ListenerAdapter;
import net.dv8tion.jda.api.events.message.MessageReceivedEvent;
import net.dv8tion.jda.api.events.message.react.MessageReactionAddEvent;

public class EasterEggNoStar extends ListenerAdapter {
    public String NOSTAR_MESSAGE = "im rigging the election";

    @Override
    public void onMessageReceived(MessageReceivedEvent event) {
        if (event.getAuthor().getId().toString() != BotSettings.Get(Settings.ID_BOT))
            return;

        if (!event.getMessage().getContentRaw().equals(NOSTAR_MESSAGE))
            return;

        try {
            Thread.sleep(1000);
        } catch (InterruptedException e) {
            Main.Utility.Log(LogLevel.ERROR, "Error while attempting to sleep!");
        }

        event.getMessage().delete();
    }

    @Override
    public void onMessageReactionAdd(MessageReactionAddEvent event) {

        if (event.getMessageAuthorId().toString() != BotSettings.Get(Settings.ADMIN_1))
            return;

        if (event.getReaction().getEmoji().getName() != "nostar")
            return;

        event.getChannel().sendMessage(NOSTAR_MESSAGE);

        // TODO: verify that this works
        event.getEmoji().asApplication().delete();
        // event.getChannel().removeReactionById(event.getChannel().getId(), event.getEmoji());
    }
}
