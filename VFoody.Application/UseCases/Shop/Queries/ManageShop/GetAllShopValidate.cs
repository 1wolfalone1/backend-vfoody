using FluentValidation;

namespace VFoody.Application.UseCases.Shop.Queries.ManageShop;

public class GetAllShopValidate : AbstractValidator<GetAllShopQuery>
{
    public GetAllShopValidate()
    {
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