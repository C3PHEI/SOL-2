using MongoDB.Driver;
using SOL_2_API.Models;
using Microsoft.Extensions.Configuration;

namespace SOL_2_API.Services
{
    public class DatabaseService
    {
        private readonly IMongoDatabase _database;

        public DatabaseService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var databaseName = configuration["DatabaseSettings:DatabaseName"];
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<CashRegister> CashRegisters => _database.GetCollection<CashRegister>("cashregisters");
        public IMongoCollection<Message> Messages => _database.GetCollection<Message>("messages");
    }
}
