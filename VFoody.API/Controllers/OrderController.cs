using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Orders.Commands.CreateOrders;
using VFoody.Application.UseCases.Orders.Commands.CustomerCancels;
using VFoody.Application.UseCases.Orders.Queries.GetOrderByStatusOfCustomer;
using VFoody.Application.UseCases.Orders.Queries.ManageOrder;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class OrderController : BaseApiController
{
    [HttpPut("customer/order/{id}/cancel")]
    [Authorize(Roles = IdentityConst.CustomerClaimName)]
    public async Task<IActionResult> CancelOrderAsync(int id)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerCancelCommand
        {
            Id = id
        }));
    }
    
    [HttpGet("admin/order/all")]
    // [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetAllOrder(int pageIndex, int pageSize)
    {
        return HandleResult(await Mediator.Send(new GetAllOrderQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }
    
    [HttpPost("customer/order")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CustomerCreateOrderCommand command)
    {
        return this.HandleResult(await this.Mediator.Send(command));

    }

    [HttpGet("customer/order/history")]
    public async Task<IActionResult> GetListCustomerOrderHistory([FromQuery] GetOrderByStatusOfCustomerQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }
}