namespace VFoody.Application.UseCases.Feedbacks.Models;

public class ShopFeedbackDTO
{
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public int FeedbackId { get; set; }
    public string Comment { get; set; }
    public string ProductOrders { get; set; }
    public DateTime CreatedDate { get; set; }
}