using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Dashboard.Queries.Overview;

namespace VFoody.API.Controllers;

[ApiController]
[Route("/api/v1/admin")]
public class DashboardController : BaseApiController
{
    [HttpGet("dashboard/overview")]
    public async Task<IActionResult> GetOverviewAdminDashboard([FromQuery] GetDashboardAdminOverviewQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }
}