using Autenticador.Application.Common.Interfaces;
using MediatR;

namespace Autenticador.Application.Features.Auth.LogoutAll
{
    public class LogoutAllHandler(IRefreshTokenRedisService refreshTokenRedisService) : IRequestHandler<LogoutAllCommand>
    {
        private readonly IRefreshTokenRedisService _refreshTokenRedisService = refreshTokenRedisService;
        public async Task Handle(LogoutAllCommand command, CancellationToken cancellationToken)
        {
            var refreshTokens = await _refreshTokenRedisService.GetAllTokensFromUserAsync(command.UserId);
            var revokedAt = DateTime.UtcNow;
            var reasonRevoked = "User logged out from all devices";

            var tokensToRevoke = new List<RefreshToken>();

            foreach (var refreshToken in refreshTokens)
            {
                if (refreshToken is not { IsActive: true })
                    continue;

                refreshToken.RevokedAt = revokedAt;
                refreshToken.ReasonRevoked = reasonRevoked;

                tokensToRevoke.Add(refreshToken);
            }

            await _refreshTokenRedisService.RevokeAllRefreshTokensAsync(tokensToRevoke, revokedAt, reasonRevoked);
        }
    }
}
