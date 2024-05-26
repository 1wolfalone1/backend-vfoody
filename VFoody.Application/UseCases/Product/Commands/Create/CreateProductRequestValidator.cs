using FluentValidation;

namespace VFoody.Application.UseCases.Product.Commands;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(p => p.File).NotNull().WithMessage("File required.");
    }
}