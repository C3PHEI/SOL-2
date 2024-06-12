using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SOL_2.Models
{
    public class CashRegister
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("bills")]
        public Dictionary<string, int> Bills { get; set; }

        [BsonElement("coins")]
        public Dictionary<string, int> Coins { get; set; }
    }
}