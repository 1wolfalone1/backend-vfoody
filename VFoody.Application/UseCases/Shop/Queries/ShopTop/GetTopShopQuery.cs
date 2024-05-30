using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopTop;

public class GetTopShopQuery : PaginationRequest,IQuery<Result>
{
}
