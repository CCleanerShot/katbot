package discordbot.mongodb.collections;

import java.util.ArrayList;

import org.bson.BsonType;
import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.codecs.pojo.annotations.BsonRepresentation;
import org.bson.types.ObjectId;

import net.querz.nbt.tag.Tag;

public class AuctionTags {
    @BsonId
    public ObjectId _id;
    /**
     * The name of the tag.
     */
    public String Name;
    /**
     * The type for the tag.
     */
    @BsonRepresentation(BsonType.STRING)
    public Tag<?> Type;
    /**
     * A list of values this tag has been seen with.
     */
    public ArrayList<String> Values = new ArrayList<String>();

    public AuctionTags(String _Name, Tag<?> _Type) {
        Name = _Name;
        Type = _Type;
    }
}
