using Autenticador.Application.Features.Auth;

namespace Autenticador.Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(int userId, List<string> roles);
        RefreshToken GenerateRefreshToken(int userId);
    }
}