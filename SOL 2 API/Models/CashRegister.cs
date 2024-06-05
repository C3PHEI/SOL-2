using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SOL_2_API.Models
{
    public class CashRegister
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId UserId { get; set; }

        [BsonElement("bills")]
        public Dictionary<string, int> Bills { get; set; }

        [BsonElement("coins")]
        public Dictionary<string, int> Coins { get; set; }
    }
}