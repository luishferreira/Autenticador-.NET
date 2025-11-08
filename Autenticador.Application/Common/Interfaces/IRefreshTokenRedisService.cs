using Autenticador.Application.Features.Auth;

namespace Autenticador.Application.Common.Interfaces
{
    public interface IRefreshTokenRedisService
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task SaveNewRefreshTokenAsync(RefreshToken refreshToken);
        Task RotateRefreshTokenAsync(RefreshToken oldRefreshToken, RefreshToken newRefreshToken);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken);
        Task RevokeAllRefreshTokensAsync(List<RefreshToken> refreshTokens, DateTime revokedAt, string reasonRevoked);
        Task CleanExpiredRefreshTokensAsync(int userId);
        Task<List<RefreshToken>> GetAllTokensFromUserAsync(int userId);
    }
}
