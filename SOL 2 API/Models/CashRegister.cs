using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SOL_2_API.Models
{
    public class CashRegister
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("bills")]
        public Dictionary<string, int> Bills { get; set; } = new Dictionary<string, int>
        {
            { "50", 0 }, { "20", 0 }, { "10", 0 }, { "5", 0 }
        };

        [BsonElement("coins")]
        public Dictionary<string, int> Coins { get; set; } = new Dictionary<string, int>
        {
            { "2", 0 }, { "1", 0 }, { "0.5", 0 }, { "0.2", 0 }, { "0.1", 0 }
        };
    }
}
