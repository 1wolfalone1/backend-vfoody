using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Dashboard.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.ChartRevenue;

public class GetChartRevenueAdminDashboardHandler : IQueryHandler<GetChartRevenueAdminDashboardQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetChartRevenueAdminDashboardHandler> _logger;

    public GetChartRevenueAdminDashboardHandler(IDapperService dapperService, ILogger<GetChartRevenueAdminDashboardHandler> logger)
    {
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetChartRevenueAdminDashboardQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var revenueChart = await this._dapperService.SingleOrDefaultAsync<RevenueChartResponse>(
                QueryName.SelectDashboardChartRevenue,
                new
                {
                    DateOfYear = request.DateOfYear
                }).ConfigureAwait(false);
            return Result.Success(revenueChart);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Dữ liệu không khả dụng"));
        }
    }
}