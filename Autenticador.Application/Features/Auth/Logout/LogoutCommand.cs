using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Auth.Logout
{
    public sealed record LogoutCommand(string refreshToken) : ICommand;
}
