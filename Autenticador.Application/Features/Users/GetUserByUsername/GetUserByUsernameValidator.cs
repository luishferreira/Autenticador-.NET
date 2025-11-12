using FluentValidation;

namespace Autenticador.Application.Features.Users.GetUserByUsername
{
    public sealed class GetUserByUsernameValidator : AbstractValidator<GetUserByUsernameQuery>
    {
        public GetUserByUsernameValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O Username deve ser informado.")
                .MaximumLength(255).WithMessage("O Username excedeu o tamanho máximo.");
        }
    }
}
