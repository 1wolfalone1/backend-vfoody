using VFoody.Application.UseCases.Accounts.Models;

namespace VFoody.Application.UseCases.Shop.Models;

public class ShopProfileResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Description { get; set; }
    public float Balance { get; set; }
    public string PhoneNumber { get; set; }
    public int ActiveFrom { get; set; }
    public int ActiveTo { get; set; }
    public ulong Active { get; set; }
    public int TotalProduct { get; set; }
    public int TotalOrder { get; set; }
    public int TotalRating { get; set; }
    public int TotalStar { get; set; }
    public double Rating
    {
        get
        {
            return Math.Round((double)this.TotalStar / TotalRating, 1);
        }
    }

    public float MinimumValueOrderFreeship { get; set; }
    public float ShippingFee { get; set; }
    public BuildingResponse Building { get; set; }
}