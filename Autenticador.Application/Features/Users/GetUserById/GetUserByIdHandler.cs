using Autenticador.Application.Common.Interfaces;
using Autenticador.Domain.Entities;
using Autenticador.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Autenticador.Application.Features.Users.GetUserById
{
    public sealed class GetUserByIdHandler(
        IUserRepository userRepository,
        IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        public async Task<UserResponse> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(query.Id) ?? throw new KeyNotFoundException($"{nameof(User)} not found");

            var response = mapper.Map<UserResponse>(user);

            return response;
        }
    }
}
