using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ProductOfShopOwner;

public class GetProductShopOwnerQuery : PaginationRequest, IQuery<Result>
{
    public int? ShopId { get; set; }
    public int? Status { get; set; }
}