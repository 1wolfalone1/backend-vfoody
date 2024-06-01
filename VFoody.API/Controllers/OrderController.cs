using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Orders.Commands.CustomerCancels;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
[Authorize(Roles = IdentityConst.CustomerClaimName)]
public class OrderController : BaseApiController
{
    [HttpPut("customer/order/{id}")]
    public async Task<IActionResult> CancelOrderAsync(int id)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerCancelCommand
        {
            Id = id
        }));
    }
}