using FluentValidation;

namespace VFoody.Application.UseCases.Promotion.Commands.UpdateShopPromotionStatus;

public class UpdateShopPromotionStatusValidate : AbstractValidator<UpdateShopPromotionStatusCommand>
{
    public UpdateShopPromotionStatusValidate()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id phải lớn hơn 0");

        RuleFor(x => x.Status)
            .IsInEnum()
            .NotEmpty()
            .WithMessage("Status (1: Active, 2: UnActive, 3: Delete)");
    }
}