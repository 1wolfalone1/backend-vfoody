using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.ChartOrder;

public class GetChartOrderAdminDashboardQuery : IQuery<Result>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; } = DateTime.Now;
}