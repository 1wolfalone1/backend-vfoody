using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Dashboard.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.ShopDashboard.ShopDashboardOverview;

public class ShopDashboardOverviewHandler : IQueryHandler<ShopDashboardOverviewQuery, Result>
{
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IDapperService _dapperService;
    private readonly ILogger<ShopDashboardOverviewHandler> _logger;
    private readonly IShopRepository _shopRepository;

    public ShopDashboardOverviewHandler(ICurrentPrincipalService currentPrincipalService, IDapperService dapperService, ILogger<ShopDashboardOverviewHandler> logger, IShopRepository shopRepository)
    {
        _currentPrincipalService = currentPrincipalService;
        _dapperService = dapperService;
        _logger = logger;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(ShopDashboardOverviewQuery request, CancellationToken cancellationToken)
    {
        var shop = await _shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId.Value);
        try
        {
            var dayCompareRate = 0;
            if (request.DateFrom != default)
            {
                dayCompareRate = (request.DateTo - request.DateFrom).Days;
            }

            var listCurrently = await this._dapperService.SingleOrDefaultAsync<ShopOverviewResponse>(
                QueryName.SelectDashboardOverviewForShop,
                new
                {
                    DateFrom = request.DateFrom == default ? DateTime.Now.AddDays(-30) : request.DateFrom,
                    DateTo = request.DateTo,
                    ShopId = shop.Id
                }).ConfigureAwait(false);

            var listCurrentlyPrevious = await this._dapperService.SingleOrDefaultAsync<ShopOverviewResponse>(
                QueryName.SelectDashboardOverviewForShop,
                new
                {
                    DateFrom = request.DateFrom == default
                        ? DateTime.Now.AddDays(-60)
                        : request.DateFrom.AddDays(-dayCompareRate),
                    DateTo = request.DateFrom == default ? DateTime.Now.AddDays(-30) : request.DateFrom,
                    ShopId = shop.Id
                }).ConfigureAwait(false);
            
            listCurrently.CalTotalCustomerRate(listCurrentlyPrevious.TotalCustomerOrder);
            listCurrently.CalTotalRevenueRate(listCurrentlyPrevious.TotalRevenue);
            listCurrently.CalTotalOrderCancelRate(listCurrentlyPrevious.TotalOrderCancel);
            listCurrently.CalTotalSuccessOrderRate(listCurrentlyPrevious.TotalSuccessOrder);

            return Result.Success(listCurrently);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw;
        }
    }
}