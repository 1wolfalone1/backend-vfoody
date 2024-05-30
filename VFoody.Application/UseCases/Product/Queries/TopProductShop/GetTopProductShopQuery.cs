using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.TopProductShop;

public class GetTopProductShopQuery : PaginationRequest, IQuery<Result>
{
    public int ShopId { get; set; }
}