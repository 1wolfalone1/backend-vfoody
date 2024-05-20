using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.ReVerify;

public class AccountReVerifyRequestValidator : AbstractValidator<AccountReVerifyRequest>
{
    public AccountReVerifyRequestValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty()
            .Matches(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$")
            .WithMessage("Email address provided is not in a valid format.");
    }
}