using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.UpdateShopPromotionStatus;

public class UpdateShopPromotionStatusCommand : ICommand<Result>
{
    public int Id { get; set; }
    public PromotionStatus Status { get; set; }
}