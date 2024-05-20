using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Accounts.Commands;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class AccountController : BaseApiController
{
    private readonly IMapper _mapper;

    public AccountController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost("customer/login")]
    public async  Task<IActionResult> Login(AccountLoginRequest loginRequest)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerLoginCommand
        {
            AccountLogin = loginRequest
        }));
    }

    [HttpPost("customer/register")]
    public async  Task<IActionResult> Login([FromBody] CustomerRegisterRequest registerRequest)
    {
        var command = _mapper.Map<CustomerRegisterCommand>(registerRequest);
        return HandleResult(await Mediator.Send(command));
    }
}