using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UpdateProfile;

public class UpdateProfileValidate : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileValidate()
    {
        RuleFor(a => a.PhoneNumber)
            .NotEmpty()
            .WithMessage("Số điện thoại là bắt buộc");
        RuleFor(a => a.FullName)
            .NotEmpty()
            .WithMessage("Email là bắt buộc");
    }
}