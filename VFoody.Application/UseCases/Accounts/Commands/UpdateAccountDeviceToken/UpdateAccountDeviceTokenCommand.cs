using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateAccountDeviceToken;

public class UpdateAccountDeviceTokenCommand : ICommand<Result>
{
    public string DeviceToken { get; set; }
}