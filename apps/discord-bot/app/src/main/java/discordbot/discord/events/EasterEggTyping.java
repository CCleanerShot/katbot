package discordbot.discord.events;

import java.text.MessageFormat;

import discordbot.Main;
import discordbot.BotSettings;
import discordbot.common.Enums.Settings;
import net.dv8tion.jda.api.events.user.UserTypingEvent;
import net.dv8tion.jda.api.hooks.ListenerAdapter;

public class EasterEggTyping extends ListenerAdapter {

    @Override
    public void onUserTyping(UserTypingEvent event) {
        int chance = Main.Utility.NextRange(1, 1000);

        if (chance != 1)
            return;

        if (event.getUser().getId().toString() == BotSettings.Get(Settings.ADMIN_1))
            return;

        String message = MessageFormat.format("<@{0}> i see you typing", event.getUser().getId().toString());
        event.getChannel().sendMessage(message);
    }
}
