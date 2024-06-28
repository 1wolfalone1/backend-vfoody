using VFoody.Application.Common.Models.Responses;

namespace VFoody.Application.UseCases.Orders.Models;

public class OrderInfoResponse
{
    public int OrderId { get; set; }
    public int OrderStatus { get; set; }
    public double ShippingFee { get; set; }
    public double TotalPrice { get; set; }
    public double TotalPromotion { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public double Distance { get; set; }
    public DateTime DurationShipping { get; set; }
    public DateTime OrderDate { get; set; }
    public int ShopId { get; set; }
    public string Note { get; set; }
    public BuildingResponse Building { get; set; } = new BuildingResponse();
    public PromotionInOrderInfoResponse Voucher { get; set; } = new PromotionInOrderInfoResponse();
}


