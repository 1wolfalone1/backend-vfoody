using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.ShopBalanceHistories.Queries.GetShopBalanceHistory;

namespace VFoody.API.Controllers;

[ApiController]
[Route("/api/v1/")]
public class ShopBalanceHistoryController : BaseApiController
{
    [HttpGet("shop/balance/history")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetHistoryShopBalance([FromQuery] GetShopBalanceHistoryQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }
}