using System.Text.Json.Serialization;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Models;

public class ShopWithdrawalRequestResponse
{
    public int ShopId { get; set; }
    public string ShopName { get; set; }
    public string LogoUrl { get; set; }
    public string BannerUrl { get; set; }
    public float Balance { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public int TotalItems { get; set; }
    public int RequestId { get; set; }
    public float RequestedAmount { get; set; }
    public int Status { get; set; }
    public int BankCode { get; set; }
    public string BankShortName { get; set; }
    public string BankAccountName { get; set; }
    public string Note { get; set; }
    public DateTime RequestedDate { get; set; }
    public DateTime ProcessedDate { get; set; }
}