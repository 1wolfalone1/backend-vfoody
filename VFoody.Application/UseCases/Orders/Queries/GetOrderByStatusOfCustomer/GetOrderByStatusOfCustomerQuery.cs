using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.GetOrderByStatusOfCustomer;

public class GetOrderByStatusOfCustomerQuery : PaginationRequest, IQuery<Result>
{
    public int[] Status { get; set; }
    public int AccountId { get; set; }
}