using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.CreateShopPromotion;

public class CreateShopPromotionCommand : ICommand<Result>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int UsageLimit { get; set; }
    public float AmountRate { get; set; }
    public float MinimumOrderValue { get; set; }
    public float MaximumApplyValue { get; set; }
    public float AmountValue { get; set; }
    public PromotionApplyTypes ApplyType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}