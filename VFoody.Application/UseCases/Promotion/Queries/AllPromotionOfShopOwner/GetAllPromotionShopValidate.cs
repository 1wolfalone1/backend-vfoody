using FluentValidation;

namespace VFoody.Application.UseCases.Promotion.Queries.AllPromotionOfShopOwner;

public class GetAllPromotionShopValidate : AbstractValidator<GetAllPromotionShopQuery>
{
    public GetAllPromotionShopValidate()
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