using Autenticador.Application.Features.Auth;

namespace Autenticador.Application.Common.Interfaces
{
    public interface IRefreshTokenRedisService
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task SetRefreshTokenAsync(RefreshToken refreshToken);
        Task CleanExpiredRefreshTokensAsync(int userId);
        Task RotateTokenAsync(RefreshToken oldRefreshToken, RefreshToken newRefreshToken);
    }
}
