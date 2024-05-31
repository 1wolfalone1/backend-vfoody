using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.Register;

public class CustomerRegisterRequestValidator : AbstractValidator<CustomerRegisterRequest>
{
    public CustomerRegisterRequestValidator()
    {
        RuleFor(a => a.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.");
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
        RuleFor(a => a.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}