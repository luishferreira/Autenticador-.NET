using Autenticador.Application.Common.Interfaces;
using Autenticador.Domain.Entities;
using Autenticador.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Autenticador.Application.Features.Users.Create
{
    public sealed class CreateUserHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(command);

            user.PasswordHash = _passwordHasher.HashPassword(command.Password);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}


