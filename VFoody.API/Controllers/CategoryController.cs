using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Category.Queries;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class CategoryController : BaseApiController
{
    [HttpGet("category")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetAllCategory()
    {
        return this.HandleResult(await this.Mediator.Send(new GetAllCategoryQuery()));
    }
}