using FluentValidation;
using VFoody.Domain.Enums;

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
        RuleFor(p => p.IsAvailable)
            .Must(IsValidBoolean)
            .WithMessage("IsAvailable must be true or false.");
    }

    private bool IsValidBoolean(bool isAvailable)
    {
        return isAvailable || isAvailable == false;
    }
}