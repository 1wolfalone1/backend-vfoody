using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;

public class ForgotPasswordRequestValidation : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidation()
    {
        RuleFor(a => a.Code)
            .NotEmpty()
            .Must(code => code is >= 1000 and < 10000)
            .WithMessage("Code must be a 4-digit number.");
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
        RuleFor(a => a.NewPassword)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}