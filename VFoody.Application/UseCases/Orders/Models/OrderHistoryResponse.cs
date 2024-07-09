using System.Text.Json.Serialization;

namespace VFoody.Application.UseCases.Orders.Models;

public class OrderHistoryResponse
{
    public int OrderId { get; set; }
    public int Status { get; set; }
    public double TotalOrderValue { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DurationShipping { get; set; }
    public DateTime EndDate { get; set; }
    public int ShopId { get; set; }
    public string ShopName { get; set; }
    public string LogoUrl { get; set; }
    public int ProductOrderQuantity { get; set; }
    [JsonIgnore]
    public int TotalItems { get; set; }
}