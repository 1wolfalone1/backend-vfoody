namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Models;

public class ShopWithdrawalHistoryResponse
{
    public int Id { get; set; }
    public int ShopId { get; set; }
    public float RequestedAmount { get; set; }
    public int Status { get; set; }
    public int BankCode { get; set; }
    public string BankShortName { get; set; }
    public string BankAccountNumber { get; set; }
    public string Note { get; set; }
    public DateTime RequestedDate { get; set; }
    public DateTime ProcessedDate { get; set; }
}