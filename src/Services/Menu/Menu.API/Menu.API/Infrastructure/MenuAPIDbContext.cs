using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace Menu.API.Infrastructure
{
    public class MenuAPIDbContext : MongoDbContext
    {
        public MenuAPIDbContext(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public MenuAPIDbContext(string connectionString, string databaseName) : base(connectionString, databaseName)
        {
        }

        public MenuAPIDbContext(string connectionString) : base(connectionString)
        {
        }

        public MenuAPIDbContext(MongoClient client, string databaseName) : base(client, databaseName)
        {
        }

    }
}
