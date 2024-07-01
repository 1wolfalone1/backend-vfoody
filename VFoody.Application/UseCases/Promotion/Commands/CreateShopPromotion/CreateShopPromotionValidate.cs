using FluentValidation;

namespace VFoody.Application.UseCases.Promotion.Commands.CreateShopPromotion;

public class CreateShopPromotionValidate : AbstractValidator<CreateShopPromotionCommand>
{
    public CreateShopPromotionValidate()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Tiêu đề không thể trống");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Mô tả không thể trống");

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0)
            .WithMessage("Số lượng sử dụng giới hạn không thể nhỏ hơn 0");

        RuleFor(x => x.AmountRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số phần trăm giảm giá không thể âm");

        RuleFor(x => x.MinimumOrderValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số tiền đơn hàng tối thiểu không thể âm");

        RuleFor(x => x.MaximumApplyValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số tiền giảm giá tối đa không thể âm");

        RuleFor(x => x.AmountValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số tiền giảm không thể âm");

        RuleFor(x => x.ApplyType)
            .IsInEnum()
            .NotEmpty()
            .WithMessage("Loại khuyến mãi phải là 1 (phần trăm) hoặc 2 (tiền)");

        RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage("Ngày bắt đầu bắt buộc");

        RuleFor(x => x.EndDate)
            .NotNull()
            .WithMessage("Ngày kết thúc bắt buộc")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu");
    }
}