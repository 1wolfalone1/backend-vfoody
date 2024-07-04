using FluentValidation;

namespace VFoody.Application.UseCases.Accounts.Commands.LoginGoogleByFirebase;

public class LoginGoogleByFirebaseValidator : AbstractValidator<LoginGoogleByFirebaseCommand>
{
    public LoginGoogleByFirebaseValidator()
    {
        RuleFor(x => x.IdToken)
            .NotEmpty()
            .WithMessage("Cần cung cấp IdToken");
    }
}