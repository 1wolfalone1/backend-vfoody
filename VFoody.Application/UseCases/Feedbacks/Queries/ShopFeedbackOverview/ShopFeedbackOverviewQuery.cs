using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbackOverview;

public class ShopFeedbackOverviewQuery : IQuery<Result>
{
    public int ShopId { get; set; }
}