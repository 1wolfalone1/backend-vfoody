using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;

public class VerifyCodeForgotPasswordValidator : AbstractValidator<VerifyCodeForgotPasswordRequest>
{
    public VerifyCodeForgotPasswordValidator()
    {
        RuleFor(a => a.Code)
            .NotEmpty()
            .Must(code => code is >= 1000 and < 10000)
            .WithMessage("Code must be a 4-digit number.");
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
    }
}