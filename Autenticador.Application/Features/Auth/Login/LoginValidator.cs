using FluentValidation;

namespace Autenticador.Application.Features.Auth.Login
{
    public sealed class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username é obrigatório.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password é obrigatório.");
        }
    }
}
