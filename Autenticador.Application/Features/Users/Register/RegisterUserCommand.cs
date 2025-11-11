using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Users.Create
{
    /// <summary>
    /// Command para registar um novo usuário.
    /// Retorna o ID do usuário criado.
    /// </summary>
    public sealed record RegisterUserCommand(
        string Username,
        string Password,
        string ConfirmPassword) : ICommand<int>;
}
