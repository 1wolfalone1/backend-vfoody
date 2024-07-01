namespace VFoody.Application.UseCases.Dashboard.Models;

public class OverviewResponse
{
    public double TotalTrading { get; set; }
    public double TotalTradingRate { get; set; }
    public double TotalRevenue { get; set; }
    public double TotalRevenueRate { get; set; }
    public int TotalOrder { get; set; }
    public double TotalOrderRate { get; set; }
    public int TotalUser { get; set; }
    public double TotalUserRate { get; set; }
    public int DayCompareRate { get; set; }

    public void CalTotalTradingRate(double previousRevenue)
    {
        if(previousRevenue != 0)
            this.TotalTradingRate = (this.TotalTrading - previousRevenue) / previousRevenue * 100;
        else 
            this.TotalTradingRate = 0;
    }
    
    public void CalTotalRevenueRate(double previousProfit)
    {
        if(previousProfit != 0)
            this.TotalRevenueRate = (this.TotalRevenue - previousProfit) / previousProfit * 100;
        else 
            this.TotalRevenueRate = 0;
    }
    
    public void CalTotalOrderRate(double previousOrder)
    {
        if(previousOrder != 0)
            this.TotalOrderRate = (this.TotalOrder - previousOrder) / previousOrder * 100;
        else 
            this.TotalOrderRate = 0;
    }
    
    public void CalTotalUserRate(double previousUser)
    {
        if(previousUser != 0)
            this.TotalUserRate = (this.TotalUser - previousUser) / previousUser * 100;
        else 
            this.TotalUserRate = 0;
    }
}