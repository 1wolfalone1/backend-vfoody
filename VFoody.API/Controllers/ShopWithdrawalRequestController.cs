using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.AdminApproveShopWithdrawal;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.AdminRejectShopWithdrawal;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.ShopWithdrawalRequests;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalHistory;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalRequestForAdmin;

namespace VFoody.API.Controllers;

[ApiController]
[Route("/api/v1/")]
public class ShopWithdrawalRequestController : BaseApiController
{
    [HttpPost("shop/withdrawal")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> ShopWithdrawalRequest(ShopWithdrawalCommandRequest requestModel)
    {
        return this.HandleResult(await this.Mediator.Send(new ShopWithdrawalRequestCommand()
        {
            CommandRequestModel = requestModel
        }));
    }
    
    [HttpGet("shop/withdrawal/history")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> ShopWithdrawalRequestHistory([FromQuery]GetShopWithdrawalHistoryQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpGet("admin/shop/withdrawal/request")]
    [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async Task<IActionResult> GetShopWithdrawalRequest([FromQuery] GetShopWithdrawalRequestQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpPut("admin/shop/{shopId}/withdrawal/approve")]
    [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async Task<IActionResult> AdminApprovedShopWithdrawalRequest([FromBody] AdminApproveShopWithdrawalCommand command)
    {
        return this.HandleResult(await this.Mediator.Send(command));
    }
    
    [HttpPut("admin/shop/{shopId}/withdrawal/reject")]
    [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async Task<IActionResult> AdminRejectShopWithdrawalRequest([FromBody] AdminRejectShopWithdrawalCommand command)
    {
        return this.HandleResult(await this.Mediator.Send(command));
    }
}