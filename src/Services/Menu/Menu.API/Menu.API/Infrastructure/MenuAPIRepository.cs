using MongoDB.Driver;
using MongoDbGenericRepository;

namespace Menu.API.Infrastructure
{
    public class MenuAPIRepository : BaseMongoRepository
    {
        public MenuAPIRepository(string connectionString, string databaseName = null) : base(connectionString, databaseName)
        {
        }

        public MenuAPIRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }

        public MenuAPIRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

    }
}
