using FluentValidation;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
    }
}