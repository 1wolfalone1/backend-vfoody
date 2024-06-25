using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.UploadImageForPlatformPromotion;

public class UploadBannerPlatformPromotionCommand : ICommand<Result>
{
    public IFormFile BannerImage { get; set; }
    public int Id { get; set; }
}