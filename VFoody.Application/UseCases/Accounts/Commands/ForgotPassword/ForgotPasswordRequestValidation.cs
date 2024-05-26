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
            .Matches(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$")
            .WithMessage("Email address provided is not in a valid format.");
        RuleFor(a => a.NewPassword)
            .NotEmpty()
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$")
            .WithMessage("Password should be 6 to 20 characters with a numeric, 1 lowercase and 1 uppercase letters.");
    }
}