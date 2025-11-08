using Autenticador.Application.Common.Interfaces;
using Autenticador.Domain.Entities;
using MediatR;
using System.Security.Authentication;

namespace Autenticador.Application.Features.Auth.Refresh
{
    public class RefreshHandler(IRefreshTokenRedisService refreshTokenRedisService, ITokenGenerator tokenGenerator) : IRequestHandler<RefreshCommand, AuthResponse>
    {
        private readonly IRefreshTokenRedisService _refreshTokenRedisService = refreshTokenRedisService;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        public async Task<AuthResponse> Handle(RefreshCommand command, CancellationToken cancellationToken)
        {
            RefreshToken oldRefreshToken = await _refreshTokenRedisService.GetRefreshTokenAsync(command.RefreshToken) ?? throw new KeyNotFoundException("Refresh token not found.");
            var userId = oldRefreshToken.UserId;

            if (oldRefreshToken.IsRevoked)
            {
                //revogar todos os refresh tokens do usuário
            }

            if (!oldRefreshToken.IsActive)
                throw new AuthenticationException("Refresh token is not active.");

            RefreshToken newRefreshToken = _tokenGenerator.GenerateRefreshToken(userId);

            oldRefreshToken.RevokedAt = DateTime.UtcNow;
            oldRefreshToken.ReplacedByToken = newRefreshToken.Token;
            oldRefreshToken.ReasonRevoked = "Replaced by new token";

            await _refreshTokenRedisService.RotateRefreshTokenAsync(oldRefreshToken, newRefreshToken);

            await _refreshTokenRedisService.CleanExpiredRefreshTokensAsync(userId);

            var accessToken = _tokenGenerator.GenerateAccessToken(userId);
            return new AuthResponse(accessToken, newRefreshToken.Token);
        }
    }
}
