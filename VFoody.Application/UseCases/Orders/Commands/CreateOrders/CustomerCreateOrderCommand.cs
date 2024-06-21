using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.CreateOrders;

public class CustomerCreateOrderCommand : ICommand<Result>
{
    public List<ProductInOrderRequest> Products { get; set; }
    public int ShopId { get; set; }
    public OrderPriceInOrderRequest OrderPrice { get; set; }
    public VoucherInOrderRequest Voucher { get; set; }
    public OrderInfoRequest OrderInfo { get; set; }
    public ShipInOrderRequest Ship { get; set; }
}