using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Firebases.Commands.CreateUser;
using VFoody.Application.UseCases.Firebases.Queries.GetFirebaseCustomToken;

namespace VFoody.API.Controllers;

[Route("api/v1")]
public class FirebaseController : BaseApiController
{
    [HttpGet("firebase/custom-token")]
    [Authorize]
    public async Task<IActionResult> GetCustomTokenInFirebase()
    {
        return this.HandleResult(await this.Mediator.Send(new GetFirebaseCustomTokenQuery()));
    }

    [HttpPost("firebase/user")]
    public async Task<IActionResult> CreateFirebaseUser([FromBody] CreateUserCommand command)
    {
        return this.HandleResult(await this.Mediator.Send(command));
    }
}