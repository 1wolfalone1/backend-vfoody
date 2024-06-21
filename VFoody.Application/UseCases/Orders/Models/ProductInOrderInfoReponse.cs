using System.Text.Json.Serialization;
using VFoody.Application.Common.Constants;

namespace VFoody.Application.UseCases.Orders.Models;

public class ProductInOrderInfoResponse
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int ProductQuantity { get; set; }
    public double ProductPrice { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public int ProductStatus { get; set; }

    public double TotalProductPrice
    {
        get
        {
            var totalProduct = this.ProductPrice * this.ProductQuantity;
            var totalTopping = this.Topping.Sum(x => x.CalTotalOptionPrice) * this.ProductQuantity;
            return totalProduct + totalTopping;
        }
    }
    public List<QuestionInOrderResponse> Topping { get; set; } = new List<QuestionInOrderResponse>();
}

public class QuestionInOrderResponse
{
    public int QuestionId { get; set; }
    public int QuestionType { get; set; }
    public string QueDescription { get; set; }

    public string TotalDescription
    {
        get
        {
            return QueDescription + StringPatterConstants.SEPARATE_ORDER_PRODUCT_NAME + " " +
                   string.Join(", ",Options.Select(x => x.OpDescription));
        }
    }
    public List<OptionInOrderResponse> Options { get; set; } = new List<OptionInOrderResponse>();

    [JsonIgnore]
    public double CalTotalOptionPrice
    {
        get
        {
            return Options.Sum(x => x.OptionPrice);
        }
    }
}

public class OptionInOrderResponse
{
    public int OptionId { get; set; }
    public string OpDescription { get; set; }
    public double OptionPrice { get; set; }
    public string OptionImageUrl { get; set; }
}