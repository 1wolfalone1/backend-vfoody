using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.GetOrderByStatusOfCustomer;

public class GetOrderByStatusOfCustomerHandler  : IQueryHandler<GetOrderByStatusOfCustomerQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<GetOrderByStatusOfCustomerHandler> _logger;

    public GetOrderByStatusOfCustomerHandler(IDapperService dapperService, ILogger<GetOrderByStatusOfCustomerHandler> logger, ICurrentPrincipalService currentPrincipalService)
    {
        _dapperService = dapperService;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
    }

    public async Task<Result<Result>> Handle(GetOrderByStatusOfCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (_currentPrincipalService.CurrentPrincipalId.Value != request.AccountId)
                throw new InvalidCastException("Tài khoản của bạn không chính xác với định danh");

            var listOrderHistory = await this._dapperService.SelectAsync<OrderHistoryResponse>(
                QueryName.SelectOrderHistoryForCustomer,
                new
                {
                    Status = request.Status,
                    AccountId = request.AccountId,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    ReviewMode = request.ReviewMode
                }).ConfigureAwait(false);
            var result = new PaginationResponse<OrderHistoryResponse>(listOrderHistory.ToList(), request.PageIndex, request.PageSize,
                listOrderHistory.Count() > 0 ? listOrderHistory.First().TotalItems : 0);
            return Result.Success(result);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw e;
        }
    }
}