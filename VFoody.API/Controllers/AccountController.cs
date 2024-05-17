using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Models;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class AccountController : BaseApiController
{
    [HttpPost("customer/login")]
    public async  Task<IActionResult> Login(AccountLoginRequest loginRequest)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerLoginCommand
        {
            AccountLogin = loginRequest
        }));
    }
}