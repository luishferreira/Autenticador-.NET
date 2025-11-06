using Autenticador.Application.Features.Auth;

namespace Autenticador.Application.Common.Interfaces
{
    public interface IRefreshTokenRedisService
    {
        Task SetRefreshTokenAsync(RefreshToken refreshToken);
    }
}
