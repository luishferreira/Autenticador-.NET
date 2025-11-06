using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Users.GetUserById
{
    public sealed record GetUserByIdQuery(int Id) : IQuery<UserResponse>;
}
