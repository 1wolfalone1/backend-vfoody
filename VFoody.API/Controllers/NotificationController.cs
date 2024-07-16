using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Notifications.Queries;
using VFoody.Application.UseCases.Notifications.Queries.ShopOwnerNotification;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class NotificationController : BaseApiController
{
    [HttpGet("customer/notification")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetAllCustomerNotification([FromQuery] GetCustomerNotificationQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpGet("shop/notification")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetAllShopNotification([FromQuery] GetShopOwnerNotificationQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }
}