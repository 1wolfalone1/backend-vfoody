using VFoody.Application.UseCases.Shop.Models;

namespace VFoody.Application.UseCases.Orders.Models;

public class OrderDetailResponse
{
    public OrderInfoResponse OrderInfo { get; set; } = new OrderInfoResponse();
    public ShopInfoResponse ShopInfo { get; set; }
    public List<ProductInOrderInfoResponse> Products { get; set; }

}