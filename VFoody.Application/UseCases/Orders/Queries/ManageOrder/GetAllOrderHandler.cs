using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.ManageOrder;

public class GetAllOrderHandler : IQueryHandler<GetAllOrderQuery, Result>
{

    private readonly IDapperService _dapperService;
    private readonly ILogger<GetAllOrderHandler> _logger;

    public GetAllOrderHandler(IDapperService dapperService, ILogger<GetAllOrderHandler> logger)
    {
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var parameterCount = new
            {
                ExcludedAccountStatus = (int) AccountStatus.Delete,
                ExcludedShopStatus = (int) ShopStatus.Delete
            };

            var parameterGetAll = new
            {
                ExcludedAccountStatus = (int) AccountStatus.Delete,
                ExcludedShopStatus = (int) ShopStatus.Delete,
                Offset = (request.PageIndex - 1) * request.PageSize,
                PageSize = request.PageSize
            };

            var totalOrders = await _dapperService.SelectAsync<int>(QueryName.CountAllOrder, parameterCount);

            var orders = await _dapperService.SelectAsync<ManageOrderResponse>(
                    QueryName.SelectAllOrder, parameterGetAll)
                .ConfigureAwait(false);


            var result = new PaginationResponse<ManageOrderResponse>(orders.ToList(), request.PageIndex, request.PageSize,
                totalOrders.First());
            return Result.Success(result);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal error"));
        }
    }
}