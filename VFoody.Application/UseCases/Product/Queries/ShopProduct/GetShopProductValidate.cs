using FluentValidation;

namespace VFoody.Application.UseCases.Product.Queries.ShopProduct;

public class GetShopProductValidate : AbstractValidator<GetShopProductQuery>
{
    public GetShopProductValidate()
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