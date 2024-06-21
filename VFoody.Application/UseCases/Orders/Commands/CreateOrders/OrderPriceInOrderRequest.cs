namespace VFoody.Application.UseCases.Orders.Commands.CreateOrders;

public class OrderPriceInOrderRequest
{
    public double Total { get; set; }
    public double TotalProduct { get; set; }
    public double Voucher { get; set; }
    public double ShippingFee { get; set; }
}