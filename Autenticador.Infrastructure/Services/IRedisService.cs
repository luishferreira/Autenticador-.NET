using StackExchange.Redis;

namespace Autenticador.Infrastructure.Services
{
    public interface IRedisService
    {
        Task<T?> GetAsync<T>(string key) where T : class;
        Task<string?> GetStringAsync(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task SetStringAsync(string key, string value, TimeSpan? expiry = null);
        Task RemoveAsync(string key);
        Task SortedSetAddAsync(string key, string member, double score);
        Task<bool> SortedSetRemoveAsync(string key, string member);
        Task<long> SortedSetRemoveRangeByScoreAsync(string key, double minScore, double maxScore);
        ITransaction CreateTransaction();
    }
}
