using Autenticador.Application.Features.Users.CreateUser;
using Autenticador.Domain.Interfaces;
using FluentValidation;

namespace Autenticador.Application.Features.Users.Create
{
    public sealed class CreateUserAdminValidator : AbstractValidator<CreateUserAdminCommand>
    {
        public CreateUserAdminValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.")
                .MustAsync(async (username, cancellationToken) =>
                {
                    return !await userRepository.UsernameExistsAsync(username);
                })
                .WithMessage("Este nome já está a ser utilizado.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("As senhas não coincidem.");

            RuleFor(x => x.RoleId)
                .IsInEnum().WithMessage("O ID da role é inválido.");
        }
    }
}
