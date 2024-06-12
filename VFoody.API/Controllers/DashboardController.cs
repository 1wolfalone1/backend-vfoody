using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Dashboard.Queries.ChartOrder;
using VFoody.Application.UseCases.Dashboard.Queries.Overview;

namespace VFoody.API.Controllers;

[ApiController]
[Route("/api/v1/admin/dashboard")]
public class DashboardController : BaseApiController
{
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverviewAdminDashboard([FromQuery] GetDashboardAdminOverviewQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpGet("chart/order")]
    public async Task<IActionResult> GetChartShowOrder([FromQuery] GetChartOrderAdminDashboardQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query).ConfigureAwait(false));
    }
}