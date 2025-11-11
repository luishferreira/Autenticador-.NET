using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Users.GetAllUsers
{
    public sealed record GetAllUsersQuery() : IQuery<IEnumerable<UserWithRolesResponse>>;
}
