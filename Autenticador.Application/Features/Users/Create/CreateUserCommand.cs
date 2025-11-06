using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Users.Create
{
    /// <summary>
    /// O "Command" (mensagem MediatR) para registar um novo utilizador.
    /// Implementa ICommand<int> para indicar que retorna o ID do novo utilizador.
    /// </summary>
    public sealed record CreateUserCommand(
        string Username,
        string Password,
        string ConfirmPassword) : ICommand<int>;
}
