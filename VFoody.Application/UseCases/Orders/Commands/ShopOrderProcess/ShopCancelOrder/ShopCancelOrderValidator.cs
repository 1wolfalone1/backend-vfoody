using FluentValidation;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopCancelOrder;

public class ShopCancelOrderValidator : AbstractValidator<ShopCancelOrderCommand>
{
    public ShopCancelOrderValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .NotNull()
            .WithMessage("Cần cung cấp lý do");
    }
}