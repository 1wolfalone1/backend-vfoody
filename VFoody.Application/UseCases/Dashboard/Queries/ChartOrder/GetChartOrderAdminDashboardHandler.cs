using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Dashboard.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.ChartOrder;

public class GetChartOrderAdminDashboardHandler : IQueryHandler<GetChartOrderAdminDashboardQuery, Result>
{
    private readonly IDapperService _dapperService;

    public GetChartOrderAdminDashboardHandler(IDapperService dapperService)
    {
        _dapperService = dapperService;
    }

    public async Task<Result<Result>> Handle(GetChartOrderAdminDashboardQuery request, CancellationToken cancellationToken)
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
            LastSevenDay = lastSevenDay,
            Week = week,
            Month = month
        });
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