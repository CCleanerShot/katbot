package discordbot.mongodb.collections;

import java.util.ArrayList;
import org.bson.types.ObjectId;
import org.bson.codecs.pojo.annotations.BsonId;

public class AuctionBuy {
    @BsonId
    public ObjectId _id;

    /**
     * List of AuctionTags that the tracked item should have.
     */
    public ArrayList<AuctionBuy> AuctionTags = new ArrayList<AuctionBuy>();
    /**
     * The Hypixel ID of the item.
     */
    public String ID = "";

}
