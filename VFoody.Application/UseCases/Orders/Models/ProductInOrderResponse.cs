namespace VFoody.Application.UseCases.Orders.Models;

public class ProductInOrderResponse
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public ToppingInProductOrder Topping { get; set; }

}

public class ToppingInProductOrder
{
    public List<OptionInProductOrder> CheckBox { get; set; }
    public List<OptionInProductOrder> Radio { get; set; }
}

public class OptionInProductOrder
{
    public int Id { get; set; }
    public int[] Options { get; set; }
}