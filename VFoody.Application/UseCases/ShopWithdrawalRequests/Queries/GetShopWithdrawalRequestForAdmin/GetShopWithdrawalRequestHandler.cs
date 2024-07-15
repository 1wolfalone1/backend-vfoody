using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalRequestForAdmin;

public class GetShopWithdrawalRequestHandler : IQueryHandler<GetShopWithdrawalRequestQuery, Result>
{
    private readonly IDapperService _dapperService;

    public GetShopWithdrawalRequestHandler(IDapperService dapperService)
    {
        _dapperService = dapperService;
    }

    public async Task<Result<Result>> Handle(GetShopWithdrawalRequestQuery request, CancellationToken cancellationToken)
    {
        var listShopWithdrawrequest = await this._dapperService.SelectAsync<ShopWithdrawalRequestResponse>(
            QueryName.SelectShopWithdrawalRequest,
            new
            {
                ShopId = request.ShopId,
                ShopName = request.ShopName,
                Status = request.Status,
                DateFrom = request.DateFrom?.ToString("yyyy-MM-dd"),
                DateTo = request.DateTo?.ToString("yyyy-MM-dd"),
                OrderBy = request.OrderBy,
                OrderMode = request.OrderMode,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            }).ConfigureAwait(false);
        
         var result = new PaginationResponse<ShopWithdrawalRequestResponse>(listShopWithdrawrequest.ToList(), request.PageIndex, request.PageSize,
             listShopWithdrawrequest.Count() > 0 ? listShopWithdrawrequest.First().TotalItems : 0);

         return Result.Success(result);
    }
}