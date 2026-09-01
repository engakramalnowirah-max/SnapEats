using FluentValidation;

namespace SnapEats.Application.Features.Customer.Commands.DeleteCustomer;

public sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0);
    }
}
