using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UpdateProfile;

public class UpdateProfileCommand : ICommand<Result>
{
    public UpdateProfileRequest UpdateProfileRequest { get; set; }
    public int Id { get; set; }
}