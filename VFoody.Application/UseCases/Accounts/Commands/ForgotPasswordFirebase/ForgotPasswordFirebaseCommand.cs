using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.ForgotPasswordFirebase;

public class ForgotPasswordFirebaseCommand : ICommand<Result>
{
    public ForgotPasswordFirebaseRequest ForgotPasswordFirebaseRequest { get; set; }
}