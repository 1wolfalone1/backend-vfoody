namespace VFoody.Application.UseCases.Dashboard.Models;

public class ShopOverviewResponse
{
    public int TotalRevenue { get; set; }
    public double TotalRevenueRate { get; set; }
    public int TotalSuccessOrder { get; set; }
    public double TotalSuccessOrderRate { get; set; }
    public int TotalOrderCancel { get; set; }
    public double TotalOrderCancelRate { get; set; }
    public int TotalCustomerOrder { get; set; }
    public double TotalCustomerOrderRate { get; set; }
    
    public void CalTotalRevenueRate(double previousRevenue)
    {
        if(previousRevenue != 0)
            this.TotalRevenueRate = (this.TotalRevenue - previousRevenue) / previousRevenue * 100;
        else 
            this.TotalRevenueRate = 0;
    }
    
    public void CalTotalSuccessOrderRate(double previousSuccessOrder)
    {
        if(previousSuccessOrder != 0)
            this.TotalSuccessOrderRate = (this.TotalSuccessOrder - previousSuccessOrder) / previousSuccessOrder * 100;
        else 
            this.TotalSuccessOrderRate = 0;
    }
    
    public void CalTotalOrderCancelRate(double previousCancelOrderRevenue)
    {
        if(previousCancelOrderRevenue != 0)
            this.TotalOrderCancelRate = (this.TotalOrderCancel - previousCancelOrderRevenue) / previousCancelOrderRevenue * 100;
        else 
            this.TotalOrderCancelRate = 0;
    }
    
    public void CalTotalCustomerRate(double previousCustomer)
    {
        if(previousCustomer != 0)
            this.TotalCustomerOrderRate = (this.TotalCustomerOrder - previousCustomer) / previousCustomer * 100;
        else 
            this.TotalCustomerOrderRate = 0;
    }
}