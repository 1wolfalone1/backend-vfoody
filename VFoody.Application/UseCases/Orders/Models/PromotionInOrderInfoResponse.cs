namespace VFoody.Application.UseCases.Orders.Models;

public class PromotionInOrderInfoResponse
{
    public int PromotionId { get; set; }
    public string Title { get; set; }
    public double AmountRate { get; set; }
    public double AmountValue { get; set; }
    public double MinimumOrderValue { get; set; }
    public double MaximumApplyValue { get; set; }
    public int ApplyType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}