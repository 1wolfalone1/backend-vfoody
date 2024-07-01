using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.DetailPromotionOfShopOwner;

public class GetPromotionShopDetailQuery : IQuery<Result>
{
    public int Id { get; set; }
}