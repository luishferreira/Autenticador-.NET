using Autenticador.Application.Common.Interfaces;
using Autenticador.Application.Features.Auth;
using StackExchange.Redis;
using System.Text.Json;

namespace Autenticador.Infrastructure.Services
{
    public class RefreshTokenRedisService(IRedisService redisService) : IRefreshTokenRedisService
    {
        private readonly IRedisService _redisService = redisService;
        private static string GetTokenKey(string token) => $"refreshToken:{token}";
        private static string GetUserKey(int userId) => $"user:{userId}";

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _redisService.GetAsync<RefreshToken>(GetTokenKey(token));
        }

        public async Task SetRefreshTokenAsync(RefreshToken refreshToken)
        {
            string refreshTokenJsonValue = JsonSerializer.Serialize(refreshToken);
            ITransaction _transaction = _redisService.CreateTransaction();

            _ = _transaction.StringSetAsync(GetTokenKey(refreshToken.Token), refreshTokenJsonValue, refreshToken.ExpiresAt - DateTime.UtcNow);

            double score = new DateTimeOffset(refreshToken.ExpiresAt).ToUnixTimeSeconds();
            _ = _transaction.SortedSetAddAsync(GetUserKey(refreshToken.UserId), refreshToken.Token, score);

            if (!await _transaction.ExecuteAsync())
                throw new RedisException("Falha ao salvar o refresh token.");
        }
        public async Task RotateTokenAsync(RefreshToken oldRefreshToken, RefreshToken newRefreshToken)
        {
            string oldRefreshTokenJsonValue = JsonSerializer.Serialize(oldRefreshToken);
            string newRefreshTokenJsonValue = JsonSerializer.Serialize(newRefreshToken);
            string oldTokenKey = GetTokenKey(oldRefreshToken.Token);
            string newTokenKey = GetTokenKey(newRefreshToken.Token);

            ITransaction _transaction = _redisService.CreateTransaction();
            _ = _transaction.StringSetAsync(oldTokenKey, oldRefreshTokenJsonValue, oldRefreshToken.ExpiresAt - DateTime.UtcNow);
            _ = _transaction.StringSetAsync(newTokenKey, newRefreshTokenJsonValue, newRefreshToken.ExpiresAt - DateTime.UtcNow);
            _ = _transaction.SortedSetRemoveAsync(GetUserKey(oldRefreshToken.UserId), oldRefreshToken.Token);
            _ = _transaction.SortedSetAddAsync(GetUserKey(newRefreshToken.UserId), newRefreshToken.Token, new DateTimeOffset(newRefreshToken.ExpiresAt).ToUnixTimeSeconds());

            if (!await _transaction.ExecuteAsync())
                throw new RedisException("Falha ao rotacionar o token.");
        }

        public async Task CleanExpiredRefreshTokensAsync(int userId)
        {
            double nowScore = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var userKey = GetUserKey(userId);
            var removedCount = await _redisService.SortedSetRemoveRangeByScoreAsync(userKey, double.NegativeInfinity, nowScore);
        }
    }
}
