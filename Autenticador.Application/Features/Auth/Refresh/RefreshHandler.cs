using MediatR;

namespace Autenticador.Application.Features.Auth.Refresh
{
    public class RefreshHandler() : IRequestHandler<RefreshCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(RefreshCommand command, CancellationToken cancellationToken)
        {
            return new AuthResponse("", "");
        }
    }
}
