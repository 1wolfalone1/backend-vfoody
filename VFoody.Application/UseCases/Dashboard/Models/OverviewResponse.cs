namespace VFoody.Application.UseCases.Dashboard.Models;

public class OverviewResponse
{
    public double TotalTrading { get; set; }
    public double TotalTradingRateRate { get; set; }
    public double TotalRevenue { get; set; }
    public double TotalRevenueRate { get; set; }
    public int TotalOrder { get; set; }
    public double TotalOrderRate { get; set; }
    public int TotalUser { get; set; }
    public double TotalUserRate { get; set; }
    public int DayCompareRate { get; set; }

    public void CalTotalTradingRate(double previousRevenue)
    {
        if(this.TotalTrading != 0)
            this.TotalTradingRateRate = (this.TotalTrading - previousRevenue) / this.TotalTrading * 100;
        else 
            this.TotalTradingRateRate = 0;
    }
    
    public void CalTotalRevenueRate(double previousProfit)
    {
        if(this.TotalRevenue != 0)
            this.TotalRevenueRate = (this.TotalRevenue - previousProfit) / this.TotalRevenue * 100;
        else 
            this.TotalRevenueRate = 0;
    }
    
    public void CalTotalOrderRate(double previousOrder)
    {
        if(TotalOrder != 0)
            this.TotalOrderRate = (this.TotalOrder - previousOrder) / this.TotalOrder * 100;
        else 
            this.TotalOrderRate = 0;
    }
    
    public void CalTotalUserRate(double previousUser)
    {
        if(this.TotalUser != 0)
            this.TotalUserRate = (this.TotalUser - previousUser) / this.TotalUser * 100;
        else 
            this.TotalUserRate = 0;
    }
}