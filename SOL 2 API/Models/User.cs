using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SOL_2_API.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }
    }
}