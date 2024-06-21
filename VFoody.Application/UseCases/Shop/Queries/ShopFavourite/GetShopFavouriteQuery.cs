using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopFavourite;

public class GetShopFavouriteQuery : PaginationRequest, IQuery<Result>
{
}