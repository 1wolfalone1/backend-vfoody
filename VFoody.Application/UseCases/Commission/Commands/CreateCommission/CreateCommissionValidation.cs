using FluentValidation;

namespace VFoody.Application.UseCases.Commission.Commands.CreateCommission;

public class CreateCommissionValidation : AbstractValidator<CreateCommissionCommand>
{
    public CreateCommissionValidation()
    {
        RuleFor(p => p.CommissionRate)
            .GreaterThan(0)
            .WithMessage("Commission rate must greater than 0");
    }
}