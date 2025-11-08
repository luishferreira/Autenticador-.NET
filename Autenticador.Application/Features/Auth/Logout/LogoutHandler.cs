using Autenticador.Application.Common.Interfaces;
using MediatR;

namespace Autenticador.Application.Features.Auth.Logout
{
    public class LogoutHandler(IRefreshTokenRedisService refreshTokenRedisService) : IRequestHandler<LogoutCommand>
    {
        private readonly IRefreshTokenRedisService _refreshTokenRedisService = refreshTokenRedisService;
        public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _refreshTokenRedisService.GetRefreshTokenAsync(command.refreshToken);

            if (refreshToken is not { IsActive: true })
                return;

            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.ReasonRevoked = "User logged out";

            await _refreshTokenRedisService.RevokeRefreshTokenAsync(refreshToken);
        }
    }
}
