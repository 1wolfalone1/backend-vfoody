using FluentValidation;

namespace VFoody.Application.UseCases.Product.Queries.ProductDetailShopOwner;

public class GetProductDetailValidator : AbstractValidator<GetProductDetailQuery>
{
    public GetProductDetailValidator()
    {
        RuleFor(p => p.productId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Product id not null and greater than 0.");
    }
}