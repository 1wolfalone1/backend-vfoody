using MediatR;
using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Accounts.Queries;
using VFoody.Application.UseCases.Roles.Commands.CreateRole;
using VFoody.Application.UseCases.Roles.Commands.UpdateRole;
using VFoody.Domain.Entities;

namespace VFoody.API.Controllers;

public class TestController : BaseApiController
{
    [HttpGet("hehe")]
    public async Task<IActionResult> GetAccount()
    {
        return this.HandleResult(await this.Mediator.Send(new GetAllAccountQuery()));
    }

    [HttpPost("role")]
    public async Task<IActionResult> CreateRole([FromBody] string name)
    { 
        return this.HandleResult(await this.Mediator.Send(new CreateRoleCommand { Name = name}));
    }

    [HttpPut("role")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRole role)
    {
        return this.HandleResult(await this.Mediator.Send(new UpdateRoleCommand { Role = role }));
    }
}
