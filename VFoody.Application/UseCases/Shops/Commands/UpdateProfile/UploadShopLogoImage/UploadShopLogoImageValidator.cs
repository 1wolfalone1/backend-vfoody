using FluentValidation;

namespace VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopLogoImage;

public class UploadShopLogoImageValidator : AbstractValidator<UploadShopLogoImageCommand>
{
    public UploadShopLogoImageValidator()
    {
        RuleFor(x => x.LogoImage)
            .NotNull()
            .WithMessage("Cần cung cấp logo của cửa hàng");
    }
}