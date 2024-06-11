using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.CheckAccount;

public class CheckAccountValidator : AbstractValidator<CheckAccountCommand>
{
    public CheckAccountValidator()
    {
        RuleFor(a => a.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.");
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
    }
}