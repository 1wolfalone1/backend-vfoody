namespace VFoody.Application.UseCases.Orders.Models;

public class ManageOrderDto
{
    public int Id { get; set; }
    public string ShopName { get; set; }

    public string CustomerName { get; set; }
    public int Status { get; set; }
    public float Price { get; set; }
    public string OrderDate { get; set; }
}