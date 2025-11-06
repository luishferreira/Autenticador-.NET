using Autenticador.Application.Common.Interfaces;
using Autenticador.Application.Features.Auth;
using StackExchange.Redis;

namespace Autenticador.Infrastructure.Services
{
    public class RefreshTokenRedisService(IRedisService redisService) : IRefreshTokenRedisService
    {
        private readonly IRedisService _redisService = redisService;
        private static string GetTokenKey(string token) => $"refreshToken:{token}";
        private static string GetUserKey(int userId) => $"user:{userId}";

        public async Task SetRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _redisService.SetAsync(GetTokenKey(refreshToken.Token), refreshToken, refreshToken.ExpiresAt - DateTime.UtcNow);

            double score = new DateTimeOffset(refreshToken.ExpiresAt).ToUnixTimeSeconds();
            await _redisService.SortedSetAddAsync(GetUserKey(refreshToken.UserId), refreshToken.Token, score);
        }
    }
}
