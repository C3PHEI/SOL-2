using MongoDB.Driver;
using SOL_2_API.Models;

namespace SOL_2_API.Services
{
    public class DatabaseService
    {
        private readonly IMongoDatabase _database;

        public DatabaseService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<CashRegister> CashRegisters => _database.GetCollection<CashRegister>("CashRegisters");
        public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");
    }
}
