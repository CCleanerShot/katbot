using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class Session
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    /// <summary>
    /// The date at which the session key expires.
    /// </summary>
    public DateTime ExpiresAt;
    /// <summary>
    /// The primary identifier of the session.
    /// </summary>
    public string ID = "";
    /// <summary>
    /// The discord id the session is for.
    /// </summary>
    public ulong UserId = 0;
    /// <summary>
    /// The username the session is for.
    /// </summary>
    public string Username = "";

    public Session(DateTime _ExpiresAt, string _ID, ulong _UserId, string _Username)
    {
        ExpiresAt = _ExpiresAt;
        ID = _ID;
        UserId = _UserId;
        Username = _Username;
    }
}