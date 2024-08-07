namespace VFoody.Application.UseCases.Shop.Models;

public class ShopInfoResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsFavouriteShop { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Description { get; set; }
    public float Balance { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int ActiveFrom { get; set; }
    public int ActiveTo { get; set; }
    public ulong Active { get; set; }
    public int TotalProduct { get; set; }
    public int TotalOrder { get; set; }
    
    public double Rating { get; set; }
    public float MinimumValueOrderFreeship { get; set; }
    public float ShippingFee { get; set; }
    public BuildingResponse Building { get; set; }

    public class BuildingResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}