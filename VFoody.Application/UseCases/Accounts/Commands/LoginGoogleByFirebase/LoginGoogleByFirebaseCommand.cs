using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.LoginGoogleByFirebase;

public class LoginGoogleByFirebaseCommand : ICommand<Result>
{
    public string IdToken { set; get; }
}