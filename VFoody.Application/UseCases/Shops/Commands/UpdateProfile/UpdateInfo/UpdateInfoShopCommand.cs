using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Commands.UpdateProfile.UpdateInfo;

public class UpdateInfoShopCommand : ICommand<Result>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhoneNumber { get; set; }
    public int ActiveFrom { get; set; }
    public int ActiveTo { get; set; }
    public float MinimumValueOrderFreeShip { get; set; }
    public float ShippingFee { get; set; }
    public int BuildingId { get; set; }
    public string Address { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}