namespace VFoody.Application.UseCases.Orders.Commands.CreateOrders;

public class ShipInOrderRequest
{
    public double Distance { get; set; }
    public double Duration { get; set; }
}