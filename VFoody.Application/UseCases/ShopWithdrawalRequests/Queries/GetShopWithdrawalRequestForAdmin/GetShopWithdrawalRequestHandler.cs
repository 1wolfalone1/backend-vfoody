using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalRequestForAdmin;

public class GetShopWithdrawalRequestHandler : IQueryHandler<GetShopWithdrawalRequestQuery, Result>
{
    public Task<Result<Result>> Handle(GetShopWithdrawalRequestQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}