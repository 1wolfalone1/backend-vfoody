namespace VFoody.Application.UseCases.Dashboard.Models;

public class DayChartOrderResponse
{
    public int TotalOfOrder { get; set; }
    public int Pending { get; set; }
    public int Cancelled { get; set; }
    public int TotalSuccessfullAmount { get; set; }
    public double Revenue { get; set; }
    public DateTime Day { get; set; }
}