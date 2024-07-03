using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.UnBanAction;

public class UnBanActionValidator : AbstractValidator<UnBanActionCommand>
{
    public UnBanActionValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Cần cung cấp lí do");
    }
}