using Autenticador.Domain.Entities;
using Autenticador.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Autenticador.Application.Features.Users.GetUserByUsername
{
    public sealed class GetUserByUsernameHandler(
        IUserRepository userRepository,
        IMapper mapper) : IRequestHandler<GetUserByUsernameQuery, UserResponse>
    {
        public async Task<UserResponse> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUsernameAsync(query.Username) ?? throw new KeyNotFoundException($"{nameof(User)} not found");

            var response = mapper.Map<UserResponse>(user);

            return response;
        }
    }
}
