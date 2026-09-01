using FluentValidation;

namespace SnapEats.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(v => v.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);

        RuleFor(v => v.Role)
            .NotEmpty()
            .Must(r => r is "Customer" or "Admin")
            .WithMessage("Role must be either 'Customer' or 'Admin'.");
    }
}

