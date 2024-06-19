namespace VFoody.Application.UseCases.Dashboard.Models;

public class DayChartOrderResponse
{
    public int TotalOfOrder { get; set; }
    public int Pending { get; set; }
    public int Confirmed { get; set; }
    public int Delivering  { get; set; }
    public int Successful { get; set; }
    public int Cancelled { get; set; }
    public int Fail  { get; set; }
    public int Reject  { get; set; }
    public double TotalTradingAmount { get; set; }
    public double Revenue { get; set; }
    public DateTime Day { get; set; }
}