package discordbot.discord.events;

import java.util.List;

import discordbot.Main;
import discordbot.S;
import discordbot.common.Enums.Settings;
import net.dv8tion.jda.api.entities.Member;
import net.dv8tion.jda.api.events.message.MessageReceivedEvent;
import net.dv8tion.jda.api.hooks.ListenerAdapter;

public class EasterEggPing extends ListenerAdapter {

    @Override
    public void onMessageReceived(MessageReceivedEvent event) {
        int chance = Main.Utility.NextRange(1, 20);
        List<Member> members = event.getMessage().getMentions().getMembers();

        if (chance != 1)
            return;

        if (members.stream().anyMatch(e -> e.getId().toString() == S.Get(Settings.ID_BOT)))
            return;

        event.getChannel().sendMessage("Who ping me?");
    }
}