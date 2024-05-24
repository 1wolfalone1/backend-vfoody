using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Commands.ReVerify;
using VFoody.Application.UseCases.Accounts.Commands.Verify;
using VFoody.Application.UseCases.Accounts.Queries;

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
    public async  Task<IActionResult> Register([FromBody] CustomerRegisterRequest registerRequest)
    {
        var command = _mapper.Map<CustomerRegisterCommand>(registerRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/verify")]
    public async  Task<IActionResult> Verify([FromBody] AccountVerifyRequest accountVerifyRequest)
    {
        var command = _mapper.Map<AccountVerifyCommand>(accountVerifyRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/resend-code")]
    public async  Task<IActionResult> ResendCodeVerify([FromBody] AccountReVerifyRequest accountReVerifyRequest)
    {
        var command = _mapper.Map<AccountReVerifyCommand>(accountReVerifyRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/google/login")]
    public async Task<IActionResult> LoginGoogle([FromBody] AccountGoogleLoginRequest accessToken)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerLoginGoogleCommand
            { AccessToken = accessToken.AccessToken }));
    }

    [HttpGet("customer")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetCustomerInfor()
    {
        return this.HandleResult(await this.Mediator.Send(new GetCustomerInforQuery()));
    }
}