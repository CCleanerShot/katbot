package discordbot.common;

import java.text.MessageFormat;
import java.util.regex.Pattern;
import net.querz.nbt.tag.IntTag;
import net.querz.nbt.tag.Tag;

public class AuctionTag {
    /**
    * The name of the tag.
    */
    public String Name;
    /**
     * The type of this tag.
     */
    public Tag<?> Type;
    /**
     * The value for this tag.
     */
    public String Value;

    public AuctionTag(String _Name, Tag<?> _Type, String _Value) {
        Name = _Name;
        Type = _Type;
        Value = _Value;
    }

    @Override
    public final String toString() {
        return MessageFormat.format("({0}, {1}) ({2})", Name, Value, Type);
    }

    @Override
    public final boolean equals(Object obj) {
        if (!(obj instanceof AuctionTag)) {
            return false;
        }

        AuctionTag c1 = this;
        AuctionTag c2 = (AuctionTag) obj;

        if (c1.Type != c2.Type)
            return false;

        if (c1.Name != c2.Name)
            return false;

        switch (c1.Type) {
            case IntTag tag:
                Pattern pattern = Pattern.compile(MessageFormat.format(".+ [1-{0}]", c1.Value));
                return pattern.matcher(c2.Value).hasMatch();
            default:
                return c1.Value == c2.Value;
        }
    }

    @Override
    public int hashCode() {
        return Name.hashCode() + Type.hashCode() + Value.hashCode();
    }
}
