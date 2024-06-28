using FluentValidation;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopRejectOrder;

public class ShopRejectOrderValidator : AbstractValidator<ShopRejectOrderCommand>
{
    public ShopRejectOrderValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .NotNull()
            .WithMessage("Cần cung cấp lý do");
    }
}