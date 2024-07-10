namespace VFoody.Application.UseCases.Feedbacks.Models;

public class ShopFeedbackOverviewResponse
{
    public int ShopTotalFeedback { get; set; }
    public double ShopRatingAverage { get; set; }
    public int TotalExcellent { get; set; }
    public int  TotalGood { get; set; }
    public int TotalAverage { get; set; }
    public int TotalBellowAverage { get; set; }
    public int TotalPoor { get; set; }
}