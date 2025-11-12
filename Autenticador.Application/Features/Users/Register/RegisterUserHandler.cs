using Autenticador.Application.Common.Interfaces;
using Autenticador.Domain.Entities;
using Autenticador.Domain.Enums;
using Autenticador.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Autenticador.Application.Features.Users.Create
{
    public sealed class RegisterUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, int>
    {

        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<int> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(command);

            user.PasswordHash = _passwordHasher.HashPassword(command.Password);

            user.UserRoles.Add(new UserRole
            {
                RoleId = (int)Roles.User,
            });

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}