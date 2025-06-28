package discordbot.mongodb.collections;

import java.util.ArrayList;

import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

public class AuctionItemsAll {
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
    /**
     * List of "Name" of the extra attributes that this item has been seen with.
     */
    public ArrayList<String> AuctionTags = new ArrayList<String>();

    public AuctionItemsAll(String _ID, String _Name) {
        this(_ID, _Name, new ArrayList<String>());
    }

    public AuctionItemsAll(String _ID, String _Name, ArrayList<String> _AuctionTags) {
        ID = _ID;
        Name = _Name;
        AuctionTags = _AuctionTags;
    }
}
