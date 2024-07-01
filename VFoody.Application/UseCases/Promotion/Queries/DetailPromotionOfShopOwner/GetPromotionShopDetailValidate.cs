using FluentValidation;
using VFoody.Application.UseCases.Promotion.Queries.AllPromotionOfShopOwner;

namespace VFoody.Application.UseCases.Promotion.Queries.DetailPromotionOfShopOwner;

public class GetPromotionShopDetailValidate : AbstractValidator<GetPromotionShopDetailQuery>
{
    public GetPromotionShopDetailValidate()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id phải lớn hơn 0");
    }
}