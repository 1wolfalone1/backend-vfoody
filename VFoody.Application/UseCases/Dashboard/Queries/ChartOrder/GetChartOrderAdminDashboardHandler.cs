using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Dashboard.Models;
using VFoody.Application.UseCases.Dashboard.Queries.ChartRevenue;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.ChartOrder;

public class GetChartOrderAdminDashboardHandler : IQueryHandler<GetChartOrderAdminDashboardQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetChartRevenueAdminDashboardHandler> _logger;

    public GetChartOrderAdminDashboardHandler(IDapperService dapperService, ILogger<GetChartRevenueAdminDashboardHandler> logger)
    {
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetChartOrderAdminDashboardQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get Last Seven Day Order From Date to
            var lastSevenDay = await this.GetDayChartOrder(request.DateTo.AddDays(-7), request.DateTo)
                .ConfigureAwait(false);

            // Get List Week
            var week = await this.GetWeekOfChartOrder(request.DateFrom, request.DateTo).ConfigureAwait(false);

            // Get Month week
            var month = await this.GetMonthOfChartOrder(request.DateFrom, request.DateTo).ConfigureAwait(false);

            return Result.Success(new
            {
                Day = lastSevenDay,
                Week = week,
                Month = month
            });
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Dữ liệu không khả dụng"));
        }
    }

    private async Task<DayChartOrderResponse> GetDayChartOrder(DateTime dateFrom, DateTime dateTo)
    {
        return await this._dapperService.SingleOrDefaultAsync<DayChartOrderResponse>(
            QueryName.SelectDashboardChartOrder, new
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
            }).ConfigureAwait(false);
    }

    private async Task<List<DayChartOrderResponse>> GetWeekOfChartOrder(DateTime dateFrom, DateTime dateTo)
    {
        List<DayChartOrderResponse> result = new List<DayChartOrderResponse>();
        for (int i = 6; i >= 0; i--)
        {
            result.Add(await this.GetDayChartOrder(dateTo.AddDays(-i), dateTo.AddDays(-i)));
        }

        return result;
    }

    private async Task<List<List<DayChartOrderResponse>>> GetMonthOfChartOrder(DateTime dateFrom, DateTime dateTo)
    {
        List<List<DayChartOrderResponse>> result = new List<List<DayChartOrderResponse>>();
        for (int i = 28; i >= 0; i-=7)
        {
            result.Add(await this.GetWeekOfChartOrder(dateFrom, dateTo.AddDays(-i)));
        }

        return result;
    }
}