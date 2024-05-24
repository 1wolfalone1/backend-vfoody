using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands;

public class CustomerLoginGoogleCommand : ICommand<Result>
{
    public string AccessToken { get; set; }
}