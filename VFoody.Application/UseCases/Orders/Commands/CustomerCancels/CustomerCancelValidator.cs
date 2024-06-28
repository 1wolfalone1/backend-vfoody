using FluentValidation;

namespace VFoody.Application.UseCases.Orders.Commands.CustomerCancels;

public class CustomerCancelValidator : AbstractValidator<CustomerCancelCommand>
{
    public CustomerCancelValidator()
    {
        RuleFor(x => x.Reason)
            .NotNull()
            .WithMessage("Cần cung cấp lí do hủy đơn hàng");
    }
}