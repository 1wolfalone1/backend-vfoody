using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopBannerImage;

public class UploadShopBannerImageCommand : ICommand<Result>
{
    public IFormFile BannerImage { get; set; }
}