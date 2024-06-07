using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UploadAvatar;

public class UpdateLoadAvatarCommand : ICommand<Result>
{
    public IFormFile AvatarImageFile { get; set; }
    public int Id { get; set; }
}