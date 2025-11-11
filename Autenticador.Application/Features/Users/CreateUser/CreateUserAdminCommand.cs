using Autenticador.Application.Common.Interfaces;
using Autenticador.Domain.Enums;

namespace Autenticador.Application.Features.Users.CreateUser
{
    /// <summary>
    /// Command para criar um novo usuário.
    /// Retorna o ID do usuário criado.
    /// </summary>
    public sealed record CreateUserAdminCommand(
        string Username,
        string Password,
        string ConfirmPassword,
        Roles RoleId) : ICommand<int>;
}
