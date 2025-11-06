using Autenticador.Application.Common.Interfaces;

namespace Autenticador.Application.Features.Users.GetUserByUsername
{
    public sealed record GetUserByUsernameQuery(string Username) : IQuery<UserResponse>;

}
