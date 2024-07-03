using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.BanAction;

public class BanActionValidator : AbstractValidator<BanActionCommand>
{
    public BanActionValidator()
    {
        RuleFor(x => x.Reason)
            .NotNull()
            .WithMessage("Cần cung cấp lí do");
    }
}