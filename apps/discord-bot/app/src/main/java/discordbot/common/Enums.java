package discordbot.common;

import java.util.HashMap;

public class Enums {
    public static enum LogLevel {
        NONE,
        WARN,
        ERROR,
    }

    public static enum OrderType {
        INSTA,
        ORDER,
    }

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

    public static class Dictionaries {

        public static HashMap<Color, String> Colors = new HashMap<Color, String>() {
            {
                put(Enums.Color.BLACK, "[0;30;40m");
                put(Color.RED, "[0;31;40m");
                put(Color.GREEN, "[0;32;40m");
                put(Color.YELLOW, "[0;33;40m");
                put(Color.BLUE, "[0;34;40m");
                put(Color.MAGENTA, "[0;35;40m");
                put(Color.CYAN, "[0;36;40m");
                put(Color.WHITE, "[0;37;40m");
            }
        };
    }
}
