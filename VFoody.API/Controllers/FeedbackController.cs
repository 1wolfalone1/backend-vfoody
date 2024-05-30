using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbacks;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
[Authorize(Roles = IdentityConst.CustomerClaimName)]
public class FeedbackController : BaseApiController
{
    [HttpGet("customer/shop/{id}/feedback")]
    public async Task<IActionResult> GetAllShopReview(int id, int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetAllShopFeedbackQuery
        {
            Id = id,
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }
}