using FluentValidation;

namespace VFoody.Application.UseCases.Shop.Queries.ShopDetail;

public class GetShopDetailValidator : AbstractValidator<GetShopDetailQuery>
{
    public GetShopDetailValidator()
    {
        RuleFor(p => p.ShopId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Shop id not null and greater than 0.");
    }
}