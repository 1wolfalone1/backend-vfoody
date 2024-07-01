using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.ShopDashboard.ShopDashboardOverview;

public class ShopDashboardOverviewQuery : IQuery<Result>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; } = DateTime.Now;
}