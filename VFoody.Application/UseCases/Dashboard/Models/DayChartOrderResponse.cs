namespace VFoody.Application.UseCases.Dashboard.Models;

public class DayChartOrderResponse
{
    public int TotalOfOrder { get; set; }
    public int OrderPLaced { get; set; }
    public int OrderConfirmed { get; set; }
    public int Preparing  { get; set; }
    public int OutForDelivery { get; set; }
    public int Delivered { get; set; }
    public int Cancel  { get; set; }
    public int Refund  { get; set; }
    public double TotalTradingAmount { get; set; }
    public double Revenue { get; set; }
    public DateTime Day { get; set; }
}