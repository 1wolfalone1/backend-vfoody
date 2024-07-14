using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopBalanceHistories.Queries.GetShopBalanceHistory;

public class GetShopBalanceHistoryQuery : PaginationRequest, IQuery<Result>
{
    public int TransactionType { get; set; }
}