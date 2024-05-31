using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.SendCode;

public class AccountSendCodeRequestValidator : AbstractValidator<AccountSendCodeRequest>
{
    public AccountSendCodeRequestValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
        RuleFor(a => a.VerifyType)
            .NotEmpty()
            .Must(verifyType => verifyType is >= 1 and <= 2)
            .WithMessage("Verify that the type is 1 for registration or 2 for forgot password.");
    }
}