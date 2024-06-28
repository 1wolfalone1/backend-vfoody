using FluentValidation;
using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.Promotion.Commands.CreatePromotion;

public class CreatePromotionValidate : AbstractValidator<CreatePromotionRequest>
{
    public CreatePromotionValidate()
    {
        RuleFor(x => x.ApplyType)
            .IsInEnum()
            .WithMessage("Loại khuyến mãi phải là 1 (phần trăm) hoặc 2 (tiền)");

        RuleFor(x => x.PromotionType)
            .IsInEnum()
            .WithMessage("Loại khuyến mãi phải là 1 (PlatformPromotion), 2 (PersonPromotion), 3 (ShopPromotion)");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Tiêu đề không thể trống");

        RuleFor(x => x.PromotionType)
            .NotNull()
            .WithMessage("Loại khuyến mãi không tể để trống");

        RuleFor(x => x.AmountRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số phần trăm giảm không thể âm");

        RuleFor(x => x.AmountValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số tiền giảm không thể âm");

        RuleFor(x => x.MinimumOrderValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Giá trị đơn hàng tối thiểu không thể âm");

        RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage("Ngày bắt đầu không thể âm");

        RuleFor(x => x.EndDate)
            .NotNull()
            .WithMessage("Ngày kết thúc không thể trống")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu");

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0)
            .WithMessage("Số lượng sử dụng giới hạn không thể nhỏ hơn 0");

        RuleFor(x => x.ApplyType)
            .NotEmpty()
            .WithMessage("Loại giảm giá không thể bỏ trống");

        RuleFor(x => x.ShopId)
            .NotNull()
            .WithMessage("Mã id shop không thể trống")
            .When(x => x.PromotionType == PromotionTypes.ShopPromotion);

        RuleFor(x => x.AccountId)
            .NotNull()
            .WithMessage("Mã id không thể để trống")
            .When(x => x.PromotionType == PromotionTypes.PersonPromotion);
    }
}