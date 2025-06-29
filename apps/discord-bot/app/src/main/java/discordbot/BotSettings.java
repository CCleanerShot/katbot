package discordbot;

import discordbot.common.Enums;
import discordbot.common.Enums.Dictionaries;
import io.github.cdimascio.dotenv.Dotenv;

/**
 * Short for Settings
 */
public class BotSettings {
    public static Dotenv dotenv;

    public static void Load() {
        dotenv = Dotenv.load();

    }

    public static String Get(Enums.Settings setting) {
        return dotenv.get(Dictionaries.Settings.get(setting));
    }
}
