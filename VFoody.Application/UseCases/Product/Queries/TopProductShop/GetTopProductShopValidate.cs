using FluentValidation;

namespace VFoody.Application.UseCases.Product.Queries.TopProductShop;

public class GetTopProductShopValidate : AbstractValidator<GetTopProductShopQuery>
{
    public GetTopProductShopValidate()
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