package discordbot.mongodb.collections;

import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

import discordbot.common.Enums;
import kotlin.ULong;

public class BazaarItem {
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
     * The type of order the watch is for.
     */
    public Enums.OrderType OrderType;
    /**
     * The price to start evaluating the threshold for alerts.
     */
    public ULong Price;
    /**
     * Whether or not to remove the tracked item after successfully found.
     */
    public boolean RemovedAfter;
    /**
     * The Discord ID of the user that submitted the tracking item.
     */
    public ULong UserId;
}