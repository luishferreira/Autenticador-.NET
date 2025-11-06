namespace Autenticador.Application.Features.Auth
{
    public sealed record AuthResponse
    (
        string AccessToken,
        string RefreshToken
    );
}
