using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.Promotion.Commands.UpdatePromotionInfo;

public class UpdatePromotionInfoRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public float AmountRate { get; set; }
    public float AmountValue { get; set; }
    public float MinimumOrderValue { get; set; }
    public float MaximumApplyValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public string Description { get; set; }
    public int? ShopId { get; set; }
    public int? AccountId { get; set; }
    public PromotionStatus Status { get; set; }
    public PromotionApplyTypes ApplyType { get; set; }
    public PromotionTypes PromotionType { get; set; }
}