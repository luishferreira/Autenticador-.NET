using FluentValidation;

namespace Autenticador.Application.Features.Users.GetUserById
{
    public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("O ID do utilizador deve ser um número positivo.");
        }
    }
}
