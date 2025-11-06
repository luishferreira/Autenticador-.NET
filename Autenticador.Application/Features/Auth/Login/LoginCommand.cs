using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Auth.Login
{
    public sealed record LoginCommand(
        string Username,
        string Password) : ICommand<AuthResponse>;
}