using FluentValidation;

namespace VFoody.Application.UseCases.Product.Queries.ShopProduct;

public class GetShopProductValidate : AbstractValidator<GetShopProductQuery>
{
    public GetShopProductValidate()
    {
        RuleFor(p => p.shopId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Shop id not null and greater than 0.");
        RuleFor(p => p.pageNum)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Page number not null and greater than 0.");
        RuleFor(p => p.pageSize)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Page size not null and greater than 0.");
    }
}