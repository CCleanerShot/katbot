package discordbot.common;

import java.util.HashMap;

public class Enums {
    public static enum Color {
        BLACK,
        RED,
        GREEN,
        YELLOW,
        BLUE,
        MAGENTA,
        CYAN,
        WHITE,
    }

    public static enum LogLevel {
        NONE,
        WARN,
        ERROR,
    }

    public static enum OrderType {
        INSTA,
        ORDER,
    }

    public static enum Settings {
        ADMIN_1,
        ADMIN_2,
        DISCORD_APPLICATION_ID,
        DISCORD_HYPIXEL_ALERTS_CHANNEL_ID,
        DISCORD_MAIN_CHANNEL_ID,
        DISCORD_PUBLIC_KEY,
        DISCORD_SEND_CHANNEL_ID,
        DISCORD_STARBOARDS_CHANNEL_ID,
        DISCORD_TOKEN,
        ENVIRONMENT,
        HYPIXEL_API_BASE_URL,
        HYPIXEL_BOT_KEY,
        ID_BOT,
        ID_CARLOS,
        ID_KELINIMO,
        ID_RAMOJUSD,
        ID_VOLATILE,
        MONGODB_BASE_URI,
        MONGODB_BASE_URI_TEST,
        MONGODB_D_DISCORD,
        MONGODB_D_GENERAL,
        MONGODB_D_HYPIXEL,
        MONGODB_C_AUCTION_BUY,
        MONGODB_C_AUCTION_ITEMS,
        MONGODB_C_AUCTION_TAGS,
        MONGODB_C_BAZAAR_BUY,
        MONGODB_C_BAZAAR_ITEMS,
        MONGODB_C_BAZAAR_SELL,
        MONGODB_C_ROLL_STATS,
        MONGODB_C_SESSIONS,
        MONGODB_C_STARBOARDS,
        MONGODB_C_USERS,
        MONGODB_OPTIONS,
        TEST_DISCORD_GUILD_ID
    }

    public static class Dictionaries {
        public static HashMap<Color, String> Colors = new HashMap<Color, String>() {
            {
                put(Color.BLACK, "[0;30;40m");
                put(Color.RED, "[0;31;40m");
                put(Color.GREEN, "[0;32;40m");
                put(Color.YELLOW, "[0;33;40m");
                put(Color.BLUE, "[0;34;40m");
                put(Color.MAGENTA, "[0;35;40m");
                put(Color.CYAN, "[0;36;40m");
                put(Color.WHITE, "[0;37;40m");
            }
        };

        public static HashMap<Settings, String> Settings = new HashMap<Settings, String>() {
            {
                put(Enums.Settings.ADMIN_1, "ADMIN_1");
                put(Enums.Settings.ADMIN_2, "ADMIN_2");
                put(Enums.Settings.DISCORD_APPLICATION_ID, "DISCORD_APPLICATION_ID");
                put(Enums.Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID, "DISCORD_HYPIXEL_ALERTS_CHANNEL_ID");
                put(Enums.Settings.DISCORD_MAIN_CHANNEL_ID, "DISCORD_MAIN_CHANNEL_ID");
                put(Enums.Settings.DISCORD_PUBLIC_KEY, "DISCORD_PUBLIC_KEY");
                put(Enums.Settings.DISCORD_SEND_CHANNEL_ID, "DISCORD_SEND_CHANNEL_ID");
                put(Enums.Settings.DISCORD_STARBOARDS_CHANNEL_ID, "DISCORD_STARBOARDS_CHANNEL_ID");
                put(Enums.Settings.DISCORD_TOKEN, "DISCORD_TOKEN");
                put(Enums.Settings.ENVIRONMENT, "ENVIRONMENT");
                put(Enums.Settings.HYPIXEL_API_BASE_URL, "HYPIXEL_API_BASE_URL");
                put(Enums.Settings.HYPIXEL_BOT_KEY, "HYPIXEL_BOT_KEY");
                put(Enums.Settings.ID_BOT, "ID_BOT");
                put(Enums.Settings.ID_CARLOS, "ID_CARLOS");
                put(Enums.Settings.ID_KELINIMO, "ID_KELINIMO");
                put(Enums.Settings.ID_RAMOJUSD, "ID_RAMOJUSD");
                put(Enums.Settings.ID_VOLATILE, "ID_VOLATILE");
                put(Enums.Settings.MONGODB_BASE_URI, "MONGODB_BASE_URI");
                put(Enums.Settings.MONGODB_BASE_URI_TEST, "MONGODB_BASE_URI_TEST");
                put(Enums.Settings.MONGODB_D_DISCORD, "MONGODB_D_DISCORD");
                put(Enums.Settings.MONGODB_D_GENERAL, "MONGODB_D_GENERAL");
                put(Enums.Settings.MONGODB_D_HYPIXEL, "MONGODB_D_HYPIXEL");
                put(Enums.Settings.MONGODB_C_AUCTION_BUY, "MONGODB_C_AUCTION_BUY");
                put(Enums.Settings.MONGODB_C_AUCTION_ITEMS, "MONGODB_C_AUCTION_ITEMS");
                put(Enums.Settings.MONGODB_C_AUCTION_TAGS, "MONGODB_C_AUCTION_TAGS");
                put(Enums.Settings.MONGODB_C_BAZAAR_BUY, "MONGODB_C_BAZAAR_BUY");
                put(Enums.Settings.MONGODB_C_BAZAAR_ITEMS, "MONGODB_C_BAZAAR_ITEMS");
                put(Enums.Settings.MONGODB_C_BAZAAR_SELL, "MONGODB_C_BAZAAR_SELL");
                put(Enums.Settings.MONGODB_C_ROLL_STATS, "MONGODB_C_ROLL_STATS");
                put(Enums.Settings.MONGODB_C_SESSIONS, "MONGODB_C_SESSIONS");
                put(Enums.Settings.MONGODB_C_STARBOARDS, "MONGODB_C_STARBOARDS");
                put(Enums.Settings.MONGODB_C_USERS, "MONGODB_C_USERS");
                put(Enums.Settings.MONGODB_OPTIONS, "MONGODB_OPTIONS");
                put(Enums.Settings.TEST_DISCORD_GUILD_ID, "TEST_DISCORD_GUILD_ID");
            }
        };

    }
}
