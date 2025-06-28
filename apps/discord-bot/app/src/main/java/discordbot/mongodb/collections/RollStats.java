package discordbot.mongodb.collections;

import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

import kotlin.ULong;

public class RollStats {
    @BsonId
    public ObjectId _id;
    /**
     * The number of losses during a /roll battle.
     */
    public Integer Lose;
    /**
     * User ID of the discord member.
     */
    public ULong UserId;
    /**
     * The number of wins during a /roll battle.
     */
    public Integer Wins;

    public RollStats(ULong _UserId) {
        UserId = _UserId;
    }
}