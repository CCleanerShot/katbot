package discordbot.discord.events;

import net.dv8tion.jda.api.hooks.ListenerAdapter;
import net.dv8tion.jda.api.requests.RestAction;
import net.dv8tion.jda.api.requests.restaction.pagination.ReactionPaginationAction;
import discordbot.BotSettings;
import discordbot.common.Enums.Settings;
import net.dv8tion.jda.api.entities.Message;
import net.dv8tion.jda.api.entities.channel.middleman.MessageChannel;
import net.dv8tion.jda.api.events.message.react.GenericMessageReactionEvent;
import net.dv8tion.jda.api.events.message.react.MessageReactionAddEvent;
import net.dv8tion.jda.api.events.message.react.MessageReactionRemoveEvent;

enum ReactionCase {
    ADD,
    REMOVE,
}

public class Starboards extends ListenerAdapter {
    /**
     * The emote that the starboard checks for.
     */
    String Emote = "⭐";
    /**
     * The threshold at which a new starboard is added for a message.
     */
    int Threshold = 3;

    @Override
    public void onMessageReactionAdd(MessageReactionAddEvent event) {
        onMessageReactionChanged(event, ReactionCase.ADD);
    }

    @Override
    public void onMessageReactionRemove(MessageReactionRemoveEvent event) {
        onMessageReactionChanged(event, ReactionCase.REMOVE);
    }

    void onMessageReactionChanged(GenericMessageReactionEvent event, ReactionCase reactionCase) {
        if (event.getGuild().getId().toString() == BotSettings.Get(Settings.TEST_DISCORD_GUILD_ID))
            return;

        if (event.getReaction().getEmoji().getName() != Emote)
            return;

        // TODO: verify it works
        ReactionPaginationAction users = event.getReaction().retrieveUsers();

        switch (reactionCase) {
            case ADD:
                if (users.cacheSize() < Threshold)
                    return;
                break;
            case REMOVE:
                break;
            default:
                break;
        }
    }
}
