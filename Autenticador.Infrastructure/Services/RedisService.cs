using Autenticador.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Autenticador.Infrastructure.Services
{
    public class RedisService(IConnectionMultiplexer connection) : IRedisService
    {
        public readonly IDatabase _database = connection.GetDatabase();
        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var value = await _database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(value.ToString());
            }
            catch (JsonException)
            {
                return default;
            }
        }
        public async Task<T?> GetAsync<T>(string key, IBatch batch) where T : class
        {
            Task<RedisValue> valueTask = batch.StringGetAsync(key);

            var value = await valueTask;

            if (value.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(value.ToString());
        }
        public async Task<string?> GetStringAsync(string key)
        {
            var value = await _database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return null;

            return value.ToString();
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration)
        {
            var jsonValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, jsonValue, expiration);
        }
        public async Task SetStringAsync(string key, string value, TimeSpan? expiration = null)
            => await _database.StringSetAsync(key, value, expiration);
        public async Task RemoveAsync(string key)
            => await _database.KeyDeleteAsync(key);
        public async Task SortedSetAddAsync(string key, string member, double score)
            => await _database.SortedSetAddAsync(key, member, score);
        public async Task<string[]> SortedSetRangeByRankAsync(string key, long minScore, long maxScore)
        {
            var redisValues = await _database.SortedSetRangeByRankAsync(key, minScore, maxScore);

            return Array.ConvertAll(redisValues, rv => rv.ToString());
        }
        public async Task<bool> SortedSetRemoveAsync(string key, string member)
            => await _database.SortedSetRemoveAsync(key, member);
        public async Task<long> SortedSetRemoveRangeByScoreAsync(string key, double minScore, double maxScore)
            => await _database.SortedSetRemoveRangeByScoreAsync(key, minScore, maxScore);
        public ITransaction CreateTransaction()
            => _database.CreateTransaction();
        public IBatch CreateBatch()
            => _database.CreateBatch();

    }
}
