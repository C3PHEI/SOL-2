using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SOL_2_API.Models
{
    public class Message
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("message")]
        public string MessageContent { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
