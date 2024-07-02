namespace VFoody.Application.UseCases.Shop.Models;

public class ManageShopDetailResponse
{
    public int Id { get; set; }
    public string ShopName { get; set; }
    public string? Description { get; set; }
    public string ShopOwnerName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string PhoneNumber { get; set; }
    public string Active { get; set; }
    public string Status { get; set; }
    public int TotalOrder { get; set; }
    public int TotalProduct { get; set; }
    public double TotalRating { get; set; }
    public double AvgRating { get; set; }
    public DateTime CreatedDate { get; set; }
    public float ShopRevenue { get; set; }
    public int ActiveFrom { get; set; }
    public int ActiveTo { get; set; }
}