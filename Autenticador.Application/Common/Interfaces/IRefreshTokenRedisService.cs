using Autenticador.Application.Features.Auth;

namespace Autenticador.Application.Common.Interfaces
{
    public interface IRefreshTokenRedisService
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task SetRefreshTokenAsync(RefreshToken refreshToken);
        Task RotateTokenAsync(RefreshToken oldRefreshToken, RefreshToken newRefreshToken);
        Task CleanExpiredRefreshTokensAsync(int userId);
        Task LogoutAsync(RefreshToken refreshToken);
    }
}
