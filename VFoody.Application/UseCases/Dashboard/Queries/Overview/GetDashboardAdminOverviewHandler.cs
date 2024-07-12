using FluentValidation;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Dashboard.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.Overview;

public class GetDashboardAdminOverviewHandler : IQueryHandler<GetDashboardAdminOverviewQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetDashboardAdminOverviewHandler> _logger;

    public GetDashboardAdminOverviewHandler(IDapperService dapperService)
    {
        _dapperService = dapperService;
    }

    public async Task<Result<Result>> Handle(GetDashboardAdminOverviewQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var dayCompareRate = 0;
            if (request.DateFrom != default)
            {
                dayCompareRate = (request.DateTo - request.DateFrom).Days + 1;
            }

            var currentOverview = await this._dapperService.SingleOrDefaultAsync<OverviewResponse>(
                QueryName.SelectDashboardOverview, new
                {
                    DateFrom = request.DateFrom,
                    DateTo = request.DateTo
                }).ConfigureAwait(false);
            
            if(request.DateFrom != default && dayCompareRate <= 365){
                var previousOverview = await this._dapperService.SingleOrDefaultAsync<OverviewResponse>(
                    QueryName.SelectDashboardOverview, new
                    {
                        DateFrom = request.DateFrom.AddDays(-dayCompareRate),
                        DateTo = request.DateFrom
                    }).ConfigureAwait(false);
            
                currentOverview.CalTotalOrderRate(previousOverview.TotalOrder);
                currentOverview.CalTotalRevenueRate(previousOverview.TotalRevenue);
                currentOverview.CalTotalTradingRate(previousOverview.TotalTrading);
                currentOverview.CalTotalUserRate(previousOverview.TotalUser);
                currentOverview.DayCompareRate = dayCompareRate;
                return Result.Success(currentOverview);
            }
            else
            {
                var currentMonthOverview = await this._dapperService.SingleOrDefaultAsync<OverviewResponse>(
                    QueryName.SelectDashboardOverview, new
                    {
                        DateFrom = request.DateTo.AddDays(-30),
                        DateTo = request.DateTo
                    }).ConfigureAwait(false);
                
                var lastMonthOverview = await this._dapperService.SingleOrDefaultAsync<OverviewResponse>(
                    QueryName.SelectDashboardOverview, new
                    {
                        DateFrom = request.DateTo.AddDays(-60),
                        DateTo = request.DateTo.AddDays(-30)
                    }).ConfigureAwait(false);
                
                currentMonthOverview.CalTotalOrderRate(lastMonthOverview.TotalOrder);
                currentMonthOverview.CalTotalRevenueRate(lastMonthOverview.TotalRevenue);
                currentMonthOverview.CalTotalTradingRate(lastMonthOverview.TotalTrading);
                currentMonthOverview.CalTotalUserRate(lastMonthOverview.TotalUser);
                
                currentMonthOverview.DayCompareRate = 30;
                return Result.Success(currentMonthOverview);
            }
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Dữ liệu không khả dụng"));
        }
    }
}
