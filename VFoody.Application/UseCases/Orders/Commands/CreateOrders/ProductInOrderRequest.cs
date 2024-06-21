using VFoody.Application.Common.Constants;

namespace VFoody.Application.UseCases.Orders.Models;

public class ProductInOrderRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public ToppingInProductOrderRequest Topping { get; set; }
    public string Note { get; set; }

    public int Id
    {
        get
        {
            var productId = ProductId.Split(StringPatterConstants.SEPARATE_ORDERID);
            return int.Parse(productId[0]);
        }
    }

}

public class ToppingInProductOrderRequest
{
    public List<OptionCheckBoxInProductOrderRequest> CheckBox { get; set; } =
        new List<OptionCheckBoxInProductOrderRequest>();

    public List<OptionRadioInProductOrderRequest> Radio { get; set; } = 
        new List<OptionRadioInProductOrderRequest>();
}

public class OptionCheckBoxInProductOrderRequest
{
    public int Id { get; set; }
    public int[] OptionIds { get; set; }
}

public class OptionRadioInProductOrderRequest
{
    public int Id { get; set; }
    public int OptionId { get; set; }
}