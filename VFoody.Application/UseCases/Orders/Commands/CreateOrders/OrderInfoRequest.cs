namespace VFoody.Application.UseCases.Orders.Commands.CreateOrders;

public class OrderInfoRequest
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public BuildingInOrderRequest Building { get; set; }
}

public class BuildingInOrderRequest
{
    public string Address { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}