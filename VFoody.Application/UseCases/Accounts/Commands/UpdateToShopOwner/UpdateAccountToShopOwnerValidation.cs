using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateToShopOwner;

public class UpdateAccountToShopOwnerValidation : AbstractValidator<UpdateAccountToShopOwnerCommand>
{
    public UpdateAccountToShopOwnerValidation()
    {
        RuleFor(x => x.LogoFile)
            .NotNull()
            .WithMessage("Logo file must not null.");

        RuleFor(x => x.BannerFile)
            .NotNull()
            .WithMessage("Banner file must not null");

        RuleFor(x => x.ShopName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Shop name must not null");

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .WithMessage("Shop description must not null");

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("Shop phone number must not null");

        RuleFor(x => x.BuildingName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Building name must not null");
    }
}