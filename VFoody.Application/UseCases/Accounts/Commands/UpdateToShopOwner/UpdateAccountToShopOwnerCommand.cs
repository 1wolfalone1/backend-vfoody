using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateToShopOwner;

public class UpdateAccountToShopOwnerCommand : ICommand<Result>
{
    public string ShopName { get; set; } = null!;
    public IFormFile BannerFile { get; set; } = null!;
    public IFormFile LogoFile { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int ActiveFrom { get; set; }
    public int ActiveTo { get; set; }
    public float MinimumValueOrderFreeship { get; set; }
    public float ShippingFee { get; set; }
    public string BuildingName { get; set; } = null!;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}