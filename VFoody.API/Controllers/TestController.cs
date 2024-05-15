using MediatR;
using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Accounts.Queries;
using VFoody.Domain.Entities;

namespace VFoody.API.Controllers;

public class TestController : BaseApiController
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/hehe")]
    public async Task<ActionResult<List<Account>>> GetAccount()
    {
        return await this.Mediator.Send(new GetAllAccounQuery());
    }
}
