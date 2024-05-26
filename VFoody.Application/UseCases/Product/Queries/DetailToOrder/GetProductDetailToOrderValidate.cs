using FluentValidation;

namespace VFoody.Application.UseCases.Product.Queries.DetailToOrder;

public class GetProductDetailToOrderValidate : AbstractValidator<GetProductDetailToOrderQuery>
{
    public GetProductDetailToOrderValidate()
    {
        RuleFor(p => p.productId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Product id not null and greater than 0.");
    }
}