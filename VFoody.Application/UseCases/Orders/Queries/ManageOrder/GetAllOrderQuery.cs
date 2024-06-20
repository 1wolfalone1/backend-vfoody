using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.ManageOrder;

public class GetAllOrderQuery : PaginationRequest, IQuery<Result>
{
}
