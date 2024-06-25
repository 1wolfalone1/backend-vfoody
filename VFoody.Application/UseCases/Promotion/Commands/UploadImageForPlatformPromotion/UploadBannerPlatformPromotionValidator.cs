using FluentValidation;

namespace VFoody.Application.UseCases.Promotion.Commands.UploadImageForPlatformPromotion;

public class UploadBannerPlatformPromotionValidator : AbstractValidator<UploadBannerPlatformPromotionCommand>
{
    public UploadBannerPlatformPromotionValidator()
    {
        RuleFor(x => x.BannerImage)
            .NotEmpty()
            .WithMessage("Vui lòng cung cấp file hình ảnh");
    }
}