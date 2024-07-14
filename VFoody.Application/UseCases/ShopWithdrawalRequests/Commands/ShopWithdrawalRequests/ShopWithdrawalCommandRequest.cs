namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.ShopWithdrawalRequests;

public class ShopWithdrawalCommandRequest
{
    public int BankCode { get; set; }
    public string BankShortName { get; set; }
    public string BankAccountNumber { get; set; }
    public float RequestedAmount { get; set; }
}