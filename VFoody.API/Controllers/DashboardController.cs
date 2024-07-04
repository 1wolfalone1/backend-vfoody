using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Dashboard.Queries.ChartOrder;
using VFoody.Application.UseCases.Dashboard.Queries.ChartRevenue;
using VFoody.Application.UseCases.Dashboard.Queries.Overview;
using VFoody.Application.UseCases.Dashboard.Queries.ShopDashboard.ShopDashboardOverview;

namespace VFoody.API.Controllers;

[ApiController]
[Route("/api/v1/admin/dashboard")]
public class DashboardController : BaseApiController
{
    [HttpGet("overview")]
    [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetOverviewAdminDashboard([FromQuery] GetDashboardAdminOverviewQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpGet("chart/order")]
    [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetChartShowOrder([FromQuery] GetChartOrderAdminDashboardQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query).ConfigureAwait(false));
    }
    
    [HttpGet("chart/revenue")]
    [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetChartShowRevenue([FromQuery] GetChartRevenueAdminDashboardQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query).ConfigureAwait(false));
    }

    [HttpGet("/api/v1/shop/dashboard/overview")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetOverviewForShop([FromQuery] ShopDashboardOverviewQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }
}