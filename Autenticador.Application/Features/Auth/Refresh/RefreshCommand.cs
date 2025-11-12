using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Auth.Refresh
{
    public sealed record RefreshCommand(
        string RefreshToken) : ICommand<AuthResponse>;
}
