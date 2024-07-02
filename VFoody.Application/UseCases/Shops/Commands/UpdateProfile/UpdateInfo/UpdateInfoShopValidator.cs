using System.Data;
using FluentValidation;

namespace VFoody.Application.UseCases.Shop.Commands.UpdateProfile.UpdateInfo;

public class UpdateInfoShopValidator : AbstractValidator<UpdateInfoShopCommand>
{
    public UpdateInfoShopValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tên không thể để trống");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("SDT không thể để trống");
        
        RuleFor(x => x.ActiveFrom)
            .LessThan(x => x.ActiveTo)
            .WithMessage("Giờ mở cửa phải nhỏ hơn giờ mở cửa");

        RuleFor(x => x.MinimumValueOrderFreeShip)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Giá trị đơn hàng tối thiểu để miễn phí ship khi quá 5km không thể trống");

        RuleFor(x => x.ShippingFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Phí ship khi quá không thể để trống hoặc âm");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Địa chỉ không thể thiếu");

        RuleFor(x => x.Latitude)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vĩ độ không thể trống");

        RuleFor(x => x.Longitude)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Kinh độ không thể trống");
    }
}