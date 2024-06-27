namespace VFoody.Application.UseCases.Accounts.Models;

public class UpdateAccountToShopOwnerResponse
{
    public int Id { get; set; }
    public string ShopName { get; set; } = null!;
    public string BannerUrl { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int ActiveFrom { get; set; }
    public int ActiveTo { get; set; }
    public float MinimumValueOrderFreeship { get; set; }
    public float ShippingFee { get; set; }
    public AccessTokenResponse AccessTokenResponse { get; set; }
    public BuildingResponse Building { get; set; }
}