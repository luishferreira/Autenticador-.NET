using Autenticador.Application.Common.Interfaces;
using MediatR;

namespace Autenticador.Application.Features.Auth.LogoutAll
{
    public class LogoutAllHandler(ITokenRevocationService tokenRevocationService) : IRequestHandler<LogoutAllCommand>
    {
        private readonly ITokenRevocationService _tokenRevocationService = tokenRevocationService;
        public async Task Handle(LogoutAllCommand command, CancellationToken cancellationToken)
        {
            await _tokenRevocationService.RevokeAllUsersRefreshTokensAsync(command.UserId, DateTime.UtcNow, "User logged out from all devices");
        }
    }
}
