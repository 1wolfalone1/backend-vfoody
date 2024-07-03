using FluentValidation;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductStatusOfShopOwner;

public class UpdateProductStatusValidate : AbstractValidator<UpdateProductStatusCommand>
{
    public UpdateProductStatusValidate()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id phải lớn hơn 0");
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Trạng thái sản phẩm: 1 (Active), 2 (InActive), 3 (Delete)");
    }
}