using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Category.Queries;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
[Authorize(Roles = IdentityConst.CustomerClaimName)]
public class CategoryController : BaseApiController
{
    [HttpGet("category")]
    public async Task<IActionResult> GetAllCategory()
    {
        return this.HandleResult(await this.Mediator.Send(new GetAllCategoryQuery()));
    }
}