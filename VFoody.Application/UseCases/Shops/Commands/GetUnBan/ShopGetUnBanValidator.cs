using FluentValidation;

namespace VFoody.Application.UseCases.Shops.Commands.GetUnBan;

public class ShopGetUnBanValidator : AbstractValidator<ShopGetUnBanCommand>
{
    public ShopGetUnBanValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Cần cung cấp lý do");
    }
}