package discordbot.mongodb.collections;

import kotlin.ULong;
import java.util.Date;
import org.bson.types.ObjectId;
import org.bson.codecs.pojo.annotations.BsonId;

// TODO: Date probably doesnt work since its old

public class Session {
    @BsonId
    public ObjectId _id;
    /**
     * The date at which the session key expires.
     */
    public Date ExpiresAt;
    /**
     * The primary identifier of the session.
     */
    public String ID;
    /**
     * The Discord ID the session is for.
     */
    public ULong UserId;
    /**
     * The username the session is for.
     */
    public String Username;

    public Session(Date _ExpiresAt, String _ID, ULong _UserId, String _Username) {
        ExpiresAt = _ExpiresAt;
        ID = _ID;
        UserId = _UserId;
        Username = _Username;
    }
}