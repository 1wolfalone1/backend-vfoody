using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ListShop;

public class GetListShopQuery : IQuery<Result>
{
    public int[] shopIds { get; set; }
}