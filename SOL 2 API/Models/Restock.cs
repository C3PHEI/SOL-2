using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SOL_2_API.Models
{
    public class Restock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("nachfullung")]
        public bool Nachfullung { get; set; }
    }
}
