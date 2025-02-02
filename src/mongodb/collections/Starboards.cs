using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class Starboards
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    /// <summary>
    /// The ID of the current message. Used for fetching the embed message on Discord.
    /// </summary>
    public ulong MessageId;
    /// <summary>
    /// The ID of the message that was starred.
    /// </summary>
    public ulong MessageStarredId;
}