using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Auth.LogoutAll
{
    public sealed record LogoutAllCommand(int UserId) : ICommand;
}
