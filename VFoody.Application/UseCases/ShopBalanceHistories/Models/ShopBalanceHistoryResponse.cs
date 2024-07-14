namespace VFoody.Application.UseCases.ShopBalanceHistories.Models;

public class ShopBalanceHistoryResponse
{
    public int Id { get; set; }
    public float ChangeAmount { get; set; }
    public float BalanceBeforeChange { get; set; }
    public float BalanceAfterChange { get; set; }
    public int TransactionType { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
}