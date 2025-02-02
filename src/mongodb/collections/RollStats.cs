using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class RollStats
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    /// <summary>
    /// Number of losses during a /roll battle
    /// </summary>
    public int Loses = 0;
    /// <summary>
    /// User ID of the discord member.
    /// </summary>
    public ulong UserId;
    /// <summary>
    /// Number of wins during a /roll battle
    /// </summary>
    public int Wins = 0;

    public RollStats(ulong _UserId)
    {
        UserId = _UserId;
    }
}