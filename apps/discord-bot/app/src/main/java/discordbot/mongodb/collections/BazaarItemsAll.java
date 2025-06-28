package discordbot.mongodb.collections;

import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

public class BazaarItemsAll {
    @BsonId
    public ObjectId _id;
    /**
     * The Hypixel ID of the item.
     */
    public String ID;
    /**
     * The name of the item.
     */
    public String Name;

    public BazaarItemsAll(String _ID, String _Name) {
        ID = _ID;
        Name = _Name;
    }
}
