using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Notifications.Queries;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class NotificationController : BaseApiController
{
    [HttpGet("customer/notification")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName}")]
    public async Task<IActionResult> GetAllCustomerNotification([FromQuery] GetCustomerNotificationQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }
}