using FluentValidation;

namespace VFoody.Application.UseCases.Shops.Commands.GetBan;

public class ShopGetBanValidator: AbstractValidator<ShopGetBanCommand>
{
    public ShopGetBanValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Cần cung cấp lý do");
    }
}