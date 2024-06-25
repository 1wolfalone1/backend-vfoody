using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.GetShopOrderByStatus;

public class GetShopOrderByStatusQuery : PaginationRequest, IQuery<Result>
{
    public OrderStatus Status { get; set; }
}