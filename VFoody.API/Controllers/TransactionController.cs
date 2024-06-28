using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Transactions.Commands.PaymentForOrder.PaymentCancel;
using VFoody.Application.UseCases.Transactions.Commands.PaymentForOrder.PaymentSuccess;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class TransactionController : BaseApiController
{
    [HttpGet("transaction/{orderId}/success")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> PaymentThirdPartGetPaymentOrderSuccess(int orderId, [FromQuery] PaymentSucessCommand command)
    {
        command.OrderId = orderId;
        return this.HandleResult(await this.Mediator.Send(command));
    }
    
    [HttpGet("transaction/{orderId}/cancel")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> PaymentThirdPartGetPaymentOrderCancel(int orderId, [FromQuery] PaymenCancelCommand command)
    {
        command.OrderId = orderId;
        return this.HandleResult(await this.Mediator.Send(command));
    }
    
}