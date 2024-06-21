using FluentValidation;

namespace VFoody.Application.UseCases.FavouriteShop.Commands.FavouriteShop;

public class FavouriteShopValidation : AbstractValidator<FavouriteShopCommand>
{
    public FavouriteShopValidation()
    {
        RuleFor(p => p.ShopId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Shop id not null and greater than 0.");
    }
}