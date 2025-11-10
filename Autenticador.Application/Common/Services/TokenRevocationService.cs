using Autenticador.Application.Common.Interfaces;
using Autenticador.Application.Features.Auth;

namespace Autenticador.Application.Common.Services
{
    public class TokenRevocationService(IRefreshTokenRedisService refreshTokenRedisService) : ITokenRevocationService
    {
        private readonly IRefreshTokenRedisService _refreshTokenRedisService = refreshTokenRedisService;
        public async Task RevokeAllUsersRefreshTokensAsync(int userId, DateTime revokedAt, string reason)
        {
            var tokensToRevoke = new List<RefreshToken>();

            var refreshTokens = await _refreshTokenRedisService.GetAllTokensFromUserAsync(userId);

            foreach (var refreshToken in refreshTokens)
            {
                if (refreshToken is not { IsActive: true })
                    continue;

                refreshToken.RevokedAt = revokedAt;
                refreshToken.ReasonRevoked = reason;

                tokensToRevoke.Add(refreshToken);
            }

            await _refreshTokenRedisService.RevokeAllRefreshTokensAsync(tokensToRevoke, revokedAt, reason);
        }
    }
}
