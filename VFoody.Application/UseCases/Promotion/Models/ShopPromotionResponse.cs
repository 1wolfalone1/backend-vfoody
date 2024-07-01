using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.Promotion.Models;

public class ShopPromotionResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int NumberOfUsed { get; set; }
    public int UsageLimit { get; set; }
    public float AmountRate { get; set; }
    public float MinimumOrderValue { get; set; }
    public float MaximumApplyValue { get; set; }
    public float AmountValue { get; set; }
    public PromotionApplyTypes ApplyType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}