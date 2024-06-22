using Newtonsoft.Json;

namespace VFoody.Application.UseCases.Shop.Models;

public class ManageShopDto
{
    public int Id { get; set; }
    public string ShopName { get; set; } = null!;
    public string ShopOwnerName { get; set; } = null!;
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public float ShopRevenue { get; set; }
    public string? PhoneNumber { get; set; }
    public int Active { get; set; }
    public int Status { get; set; }
    public int TotalOrder { get; set; }
    public int TotalProduct { get; set; }
    public DateTime CreatedDate { get; set; }
    [JsonIgnore]
    public int TotalCount { get; set; }
}