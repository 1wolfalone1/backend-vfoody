using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalHistory;

public class GetShopWithdrawalHistoryQuery : PaginationRequest, IQuery<Result>
{
    public int Status { get; set; }
}