using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.ShopWithdrawalRequests;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalHistory;

namespace VFoody.API.Controllers;

[ApiController]
[Route("/api/v1/")]
public class ShopWithdrawalRequestController : BaseApiController
{
    [HttpPost("shop/withdrwawal")]
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
}