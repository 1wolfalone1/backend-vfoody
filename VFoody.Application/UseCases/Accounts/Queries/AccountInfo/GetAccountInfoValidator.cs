using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Queries.AccountInfo;

public class GetAccountInfoValidator : AbstractValidator<GetAccountInfoQuery>
{
    public GetAccountInfoValidator()
    {
        RuleFor(p => p.accountId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Account id not null and greater than 0.");
    }
}