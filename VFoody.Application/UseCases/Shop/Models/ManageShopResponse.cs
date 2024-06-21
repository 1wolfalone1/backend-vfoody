namespace VFoody.Application.UseCases.Shop.Models;

public class ManageShopResponse
{
    public int Id { get; set; }
    public string ShopName { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Description { get; set; }
    public float Balance { get; set; }
    public string PhoneNumber { get; set; }
    public int ActiveFrom { get; set; }
    public int ActiveTo { get; set; }
    public string Active { get; set; }
    public string Status { get; set; }
    public int TotalOrder { get; set; }
    public int TotalProduct { get; set; }
    public double TotalRating { get; set; }
    public int TotalStar { get; set; }
    public double RatingPercent { get; set; }
    public float MinimumValueOrderFreeship { get; set; }
    public float ShippingFee { get; set; }
    public DateTime CreatedDate { get; set; }
    public BuildingResponse Building { get; set; }

    public class BuildingResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}