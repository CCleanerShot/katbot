using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class MongoUser
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    /// <summary>
    /// User ID of the discord member.
    /// </summary>
    public ulong UserId;
    /// <summary>
    /// Username of the database user.
    /// </summary>
    public string Username;
    /// <summary>
    /// Password of the database user, given by me.
    /// </summary>
    public string Password;

    public MongoUser(ulong _UserId, string _Username, string _Password)
    {
        UserId = _UserId;
        Username = _Username;
        Password = _Password;
    }
}