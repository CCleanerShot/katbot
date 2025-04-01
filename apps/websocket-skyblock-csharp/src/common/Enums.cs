namespace Enums
{
    public enum LogLevel
    {
        NONE,
        WARN,
        ERROR,
    }

    public enum OrderType
    {
        INSTA,
        ORDER,
    }

    public enum Color
    {
        BLACK,
        RED,
        GREEN,
        YELLOW,
        BLUE,
        MAGENTA,
        CYAN,
        WHITE,
    }

    public static class Dictionaries
    {

        public static Dictionary<Color, string> Colors = new Dictionary<Color, string>()
        {
            {Color.BLACK, "[0;30;40m"},
            {Color.RED, "[0;31;40m"},
            {Color.GREEN, "[0;32;40m"},
            {Color.YELLOW, "[0;33;40m"},
            {Color.BLUE, "[0;34;40m"},
            {Color.MAGENTA, "[0;35;40m"},
            {Color.CYAN, "[0;36;40m"},
            {Color.WHITE, "[0;37;40m"},
        };
    }
}