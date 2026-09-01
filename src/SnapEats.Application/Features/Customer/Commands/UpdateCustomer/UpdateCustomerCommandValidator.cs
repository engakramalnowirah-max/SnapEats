using FluentValidation;

namespace SnapEats.Application.Features.Customer.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0);

        RuleFor(v => v.FullName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(v => v.Phone)
            .NotEmpty()
            .MaximumLength(20);
    }
}
