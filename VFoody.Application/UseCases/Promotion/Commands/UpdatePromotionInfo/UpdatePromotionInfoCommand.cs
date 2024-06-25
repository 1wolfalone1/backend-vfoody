
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.UpdatePromotionInfo;

public class UpdatePromotionInfoCommand : ICommand<Result>
{
    public UpdatePromotionInfoRequest Promotion { get; set; }
}