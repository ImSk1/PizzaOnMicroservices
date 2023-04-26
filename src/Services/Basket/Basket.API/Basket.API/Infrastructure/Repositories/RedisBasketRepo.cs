using System.Text.Json;
using Basket.API.Infrastructure.Contracts;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Basket.API.Models;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace Basket.API.Infrastructure.Repositories
{
    public class RedisBasketRepo : IBasketRepository
    {
        private readonly ILogger<RedisBasketRepo> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        public RedisBasketRepo(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
        {
            _logger = loggerFactory.CreateLogger<RedisBasketRepo>();
            _redis = redis;
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public IEnumerable<string> GetBuyerIds()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
        public async Task<BuyerBasket> GetBasketAsync(string customerId)
        {
            var data = await _database.StringGetAsync(customerId);

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<BuyerBasket>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<BuyerBasket> UpdateBasketAsync(BuyerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.BuyerId, JsonSerializer.Serialize(basket));

            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item.");
                return null;
            }

            _logger.LogInformation("Basket item persisted succesfully.");

            return await GetBasketAsync(basket.BuyerId);
        }
    }
}
