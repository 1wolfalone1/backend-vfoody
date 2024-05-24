using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Accounts.Queries;
using VFoody.Application.UseCases.Roles.Commands.CreateRole;
using VFoody.Application.UseCases.Roles.Commands.UpdateRole;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.API.Controllers;

public class TestController : BaseApiController
{
    [HttpGet("hehe")]
    [Authorize(Roles = IdentityConst.CustomerClaimName)]
    public async Task<IActionResult> GetAccount()
    {
        return this.HandleResult(await this.Mediator.Send(new GetAllAccountQuery()));
    }

    [HttpPost("role")]
    [Authorize(Roles = "Shop")]
    public async Task<IActionResult> CreateRole([FromBody] string name)
    { 
        return this.HandleResult(await this.Mediator.Send(new CreateRoleCommand { Name = name}));
    }

    [HttpPut("role")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRole role)
    {
        return this.HandleResult(await this.Mediator.Send(new UpdateRoleCommand { Role = role }));
    }

    [HttpGet("test")]
    public IActionResult TestAccount()
    {
        return this.HandleResult(Result.Success(Unit.Value));
    }
}
