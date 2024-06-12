namespace VFoody.Application.UseCases.Dashboard.Models;

public class WeekChartOrderResponse
{
    public List<DayChartOrderResponse> Days { get; set; } = new List<DayChartOrderResponse>();
}