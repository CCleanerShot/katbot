using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class MongoUser
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    /// <summary>
    /// The ID of the discord member.
    /// </summary>
    public ulong DiscordId;
    /// <summary>
    /// The username of the database user.
    /// </summary>
    public string Username;
    /// <summary>
    /// The password of the database user, given by me.
    /// </summary>
    public string Password;

    public MongoUser(ulong _DiscordId, string _Username, string _Password)
    {
        DiscordId = _DiscordId;
        Username = _Username;
        Password = _Password;
    }
}