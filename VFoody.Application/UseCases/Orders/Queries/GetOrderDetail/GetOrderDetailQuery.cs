using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.GetOrderDetail;

public class GetOrderDetailQuery : IQuery<Result>
{
    public int OrderId { get; set; }
    public int AccountId { get; set; }
}