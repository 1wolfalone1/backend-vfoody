using FluentValidation;

namespace VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopBannerImage;

public class UploadShopBannerImageValidator : AbstractValidator<UploadShopBannerImageCommand>
{
    public UploadShopBannerImageValidator()
    {
        RuleFor(x => x.BannerImage)
            .NotNull()
            .WithMessage("Ảnh nền của shop không thể trống");
    }
}