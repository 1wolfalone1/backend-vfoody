using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Feedbacks.Commands.CustomerCreateFeedback;
using VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbackOverview;
using VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbacks;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
[Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
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

    [HttpPost("customer/feedback/order/{orderId}")]
    public async Task<IActionResult> CreateFeedbackForOrder(int orderId, [FromForm] CustomerCreateFeedbackRequest requestModel)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerCreateFeedbackCommand()
        {
            OrderId = orderId,
            RequestModel = requestModel
        }));
    }

    [HttpGet("customer/shop/{shopId}/feedback/overview")]
    public async Task<IActionResult> GetSummaryFeedbackOfShop(int shopId)
    {
        return this.HandleResult(await this.Mediator.Send(new ShopFeedbackOverviewQuery()
        {
            ShopId = shopId
        }));
    }
}