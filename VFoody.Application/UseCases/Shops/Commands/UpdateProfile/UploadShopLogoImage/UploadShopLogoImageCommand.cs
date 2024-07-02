using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopLogoImage;

public class UploadShopLogoImageCommand : ICommand<Result>
{
    public IFormFile LogoImage { get; set; }
}