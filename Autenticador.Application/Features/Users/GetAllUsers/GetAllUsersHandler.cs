using Autenticador.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Autenticador.Application.Features.Users.GetAllUsers
{
    public sealed class GetAllUsersHandler(
        IUserRepository userRepository,
        IMapper mapper) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserWithRolesResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<UserWithRolesResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();

            var response = _mapper.Map<IEnumerable<UserWithRolesResponse>>(users);

            return response;
        }
    }
}
