using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.ChartRevenue;

public class GetChartRevenueAdminDashboardQuery : IQuery<Result>
{
    public DateTime DateOfYear { get; set; } = DateTime.Now;
}