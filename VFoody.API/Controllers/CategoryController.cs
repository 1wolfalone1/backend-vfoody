using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Category.Queries;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class CategoryController : BaseApiController
{
    [HttpGet("category")]
    public async Task<IActionResult> GetAllCategory()
    {
        return this.HandleResult(await this.Mediator.Send(new GetAllCategoryQuery()));
    }
}