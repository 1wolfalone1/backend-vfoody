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
        When(x => x.Status.HasValue, () =>
        {
            RuleFor(x => x.Status)
                .Must(status => status == (int)PromotionStatus.Active || status == (int)PromotionStatus.UnActive)
                .WithMessage("Trạng thái mã giảm giá chỉ có thể là 1 (Active) hoặc 2 (InActive).");
        });
    }
}