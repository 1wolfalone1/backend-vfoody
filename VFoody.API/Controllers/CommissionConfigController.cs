using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Commission.Commands.CreateCommission;
using VFoody.Application.UseCases.Commission.Queries.GetCommission;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class CommissionConfigController : BaseApiController
{
    [HttpGet("admin/commission")]
    // [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async  Task<IActionResult> GetCommissionRate()
    {
        return HandleResult(await Mediator.Send(new GetCommissionRateQuery()));
    }
    [HttpPut("admin/commission/update")]
    // [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async  Task<IActionResult> UpdateCommission(CreateCommissionCommand createCommissionCommand)
    {
        return HandleResult(await Mediator.Send(createCommissionCommand));
    }
}