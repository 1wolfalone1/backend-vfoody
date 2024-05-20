using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands;

public class CustomerRegisterRequestValidator : AbstractValidator<CustomerRegisterRequest>
{
    public CustomerRegisterRequestValidator()
    {
        RuleFor(a => a.FirstName).NotEmpty();
        RuleFor(a => a.LastName).NotEmpty();
        RuleFor(a => a.Email)
            .NotEmpty()
            .Matches(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$")
            .WithMessage("Email address provided is not in a valid format.");
        RuleFor(a => a.Password)
            .NotEmpty()
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$")
            .WithMessage("Password should be 6 to 20 characters with a numeric, 1 lowercase and 1 uppercase letters.");
    }
}