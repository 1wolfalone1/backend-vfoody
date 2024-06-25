using System.Text.Json.Serialization;

namespace VFoody.Application.UseCases.Orders.Models;

public class OrderOfShopResponse
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime CreatedDate { get; set; }
    [JsonIgnore]
    public int TotalItems { get; set; }
}