package discordbot.mongodb.collections;

import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

import kotlin.ULong;

public class MongoUser {
    @BsonId
    public ObjectId _id;
    /**
     * The ID of the discord member.
     */
    public ULong DiscordID;
    /**
     * The username of the database user.
     */
    public String Username;
    /**
     * The password of the database user, given by me.
     */
    public String Password;

    public MongoUser(ULong _DiscordID, String _Username, String _Password) {
        DiscordID = _DiscordID;
        Username = _Username;
        Password = _Password;
    }
}