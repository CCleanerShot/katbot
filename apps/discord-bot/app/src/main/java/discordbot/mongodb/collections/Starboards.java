package discordbot.mongodb.collections;

import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

import kotlin.ULong;

public class Starboards {
    @BsonId
    public ObjectId _id;
    /**
     * The ID of the current message. Used for fetching the embed message on Discord.
     */
    public ULong MessageId;
    /**
     * The ID of the message that was starred.
     */
    public ULong MessageStarredId;
}
