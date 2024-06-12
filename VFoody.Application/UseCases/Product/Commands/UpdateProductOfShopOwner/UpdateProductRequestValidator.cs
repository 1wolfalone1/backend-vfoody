using FluentValidation;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Product id must greater than 0");
    }
}