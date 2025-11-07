using Autenticador.Application.Common.Interfaces;
using Autenticador.Domain.Entities;
using Autenticador.Domain.Interfaces;
using MediatR;
using System.Security.Authentication;

namespace Autenticador.Application.Features.Auth.Login
{
    public class LoginHandler(ITokenGenerator tokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher, IRefreshTokenRedisService refreshTokenRedisService) : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IRefreshTokenRedisService _refreshTokenRedisService = refreshTokenRedisService;

        public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetByUsernameAsync(command.Username) ?? throw new KeyNotFoundException("Credenciais inválidas.");

            bool isValid = _passwordHasher.VerifyPassword(command.Password, user.PasswordHash);

            if (!isValid)
                throw new AuthenticationException("Credenciais inválidas.");

            var accessToken = _tokenGenerator.GenerateAccessToken(user.Id);
            var refreshToken = _tokenGenerator.GenerateRefreshToken(user.Id);
            
            await _refreshTokenRedisService.SetRefreshTokenAsync(refreshToken);

            await _refreshTokenRedisService.CleanExpiredRefreshTokensAsync(user.Id);

            return new AuthResponse(accessToken, refreshToken.Token);
        }
    }
}