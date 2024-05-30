using FluentValidation;

namespace VFoody.Application.UseCases.Product.Queries.TopProductShop;

public class GetTopProductShopValidate : AbstractValidator<GetTopProductShopQuery>
{
    public GetTopProductShopValidate()
    {
        RuleFor(p => p.ShopId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Shop id not null and greater than 0.");
        RuleFor(p => p.PageIndex)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Page number not null and greater than 0.");
        RuleFor(p => p.PageSize)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Page size not null and greater than 0.");
    }
}