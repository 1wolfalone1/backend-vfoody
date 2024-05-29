using FluentValidation;

namespace VFoody.Application.UseCases.Shop.Queries.ShopInfo;

public class GetShopInfoValidate :AbstractValidator<GetShopInfoQuery>
{
    public GetShopInfoValidate()
    {
        RuleFor(p => p.shopId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Shop id not null and greater than 0.");
    }
}