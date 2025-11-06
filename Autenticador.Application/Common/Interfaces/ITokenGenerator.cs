using Autenticador.Application.Features.Auth;

namespace Autenticador.Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(int userId);
        RefreshToken GenerateRefreshToken(int userId);
    }
}