using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Models;

namespace Menu.API.Infrastructure.Entities
{
    public class Pizza : IDocument
    {
        public int Version { get; set; }

        [BsonId]
        public Guid Id { get; set; }

        [BsonRequired] public string Name { get; set; } = null!;

        [BsonRequired] public IEnumerable<string> Ingredients { get; set; } = null!;

        [BsonRequired] public bool InStock { get; set; }

        [BsonRequired] public double Cost { get; set; }
    }
}
