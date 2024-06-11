namespace VFoody.Application.UseCases.Dashboard.Models;

public class OverviewResponse
{
    public double TotalRevenue { get; set; }
    public double TotalRevenueRate { get; set; }
    public double TotalProfit { get; set; }
    public double TotalProfitRate { get; set; }
    public int TotalOrder { get; set; }
    public double TotalOrderRate { get; set; }
    public int TotalUser { get; set; }
    public double TotalUserRate { get; set; }

    public void CalTotalRevenueRate(double previousRevenue)
    {
        if(this.TotalRevenue != 0)
            this.TotalRevenueRate = (this.TotalRevenue - previousRevenue) / this.TotalRevenue * 100;
        else 
            this.TotalRevenueRate = 0;
    }
    
    public void CalTotalProfitRate(double previousProfit)
    {
        if(this.TotalProfit != 0)
            this.TotalProfitRate = (this.TotalProfit - previousProfit) / this.TotalProfit * 100;
        else 
            this.TotalProfitRate = 0;
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