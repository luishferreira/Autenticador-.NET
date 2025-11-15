using Autenticador.Application.Common.Interfaces;
using Autenticador.Application.Features.Users.Create;
using Autenticador.Application.Mappings;
using Autenticador.Domain.Entities;
using Autenticador.Domain.Interfaces;
using AutoMapper;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using System.Security.Cryptography;

namespace Autenticador.Application.Tests.Unit.Features.Users.Register
{
    public class RegisterUserHandlerTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly RegisterUserHandler _handler;
        private readonly Faker<RegisterUserCommand> _faker;

        public RegisterUserHandlerTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _passwordHasher = Substitute.For<IPasswordHasher>();

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            }, NullLoggerFactory.Instance);

            _mapper = mappingConfig.CreateMapper();

            _handler = new RegisterUserHandler(_userRepositoryMock, _mapper, _unitOfWork, _passwordHasher);

            _faker = new Faker<RegisterUserCommand>().CustomInstantiator(faker =>
            {
                var password = faker.Internet.Password(8);
                return new RegisterUserCommand
                (
                    faker.Internet.UserName(),
                    password,
                    password
                );
            });

            _passwordHasher.HashPassword(Arg.Any<string>())
                .Returns(callInfo => $"$argon2id${callInfo.Arg<string>()}");
        }

        public static void SetEntityId<T>(T entity, int id) where T : BaseEntity
        {
            var propertyInfo = typeof(BaseEntity).GetProperty("Id");
            propertyInfo?.SetValue(entity, id);
        }

        public async Task Handle_ShouldRegisterUser_Success()
        {
            RegisterUserCommand user = _faker.Generate();
            var userIdExpected = RandomNumberGenerator.GetInt32(1, 1000);
            User? capturedUser = null;

            await _userRepositoryMock.AddAsync(Arg.Do<User>(user => capturedUser = user));

            // Tem que simular o EF Core setando o Id do usuário após o SaveChangesAsync.
            _unitOfWork.SaveChangesAsync().Returns(Task.FromResult(1)).AndDoes(_ =>
            {
                SetEntityId(capturedUser!, userIdExpected);
            });

            var returnedId = await _handler.Handle(_faker.Generate(), CancellationToken.None);

            await _unitOfWork.Received(1).SaveChangesAsync();
            await _userRepositoryMock.Received(1).AddAsync(Arg.Is<User>(
                u => u.Username == user.Username 
                && u.PasswordHash.StartsWith("$argon2id$")
                && u.UserRoles.First().Role.Name == "User"
            ));
        }
    }
}
