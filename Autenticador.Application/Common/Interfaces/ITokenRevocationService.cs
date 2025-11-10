namespace Autenticador.Application.Common.Interfaces
{
    public interface ITokenRevocationService
    {
        Task RevokeAllUsersRefreshTokensAsync(int userId, DateTime revokedAt, string reason);
    }
}
