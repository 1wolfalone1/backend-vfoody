using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Promotion.Queries.Customer;
using VFoody.Application.UseCases.Promotion.Queries.Platform;
using VFoody.Application.UseCases.Promotion.Queries.Shop;
using VFoody.Domain.Enums;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
[Authorize(Roles = IdentityConst.CustomerClaimName)]
public class PromotionController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ICurrentPrincipalService _currentPrincipalService;

    public PromotionController(IMapper mapper, ICurrentPrincipalService currentPrincipalService)
    {
        _mapper = mapper;
        _currentPrincipalService = currentPrincipalService;
    }

    [HttpGet("customer/promotion")]
    public async Task<IActionResult> GetCustomerPromotion(int pageIndex, int pageSize)
    {
        if (!_currentPrincipalService.CurrentPrincipalId.HasValue)
        {
            throw new Exception("401 unauthorized but set 500 for while!");
        }

        return this.HandleResult(await this.Mediator.Send(new GetCustomerPromotionListQuery
        {
            AccountId = _currentPrincipalService.CurrentPrincipalId.Value,
            Status = (int) PersonPromotionStatus.Active,
            Available = true,
            PageIndex = pageIndex,
            PageSize = pageSize,
        }));
    }

    [HttpGet("customer/promotion/shop/{id}")]
    public async Task<IActionResult> GetShopPromotion(int id, int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetShopPromotionListQuery
        {
            ShopId = id,
            Status = (int) ShopPromotionStatus.Active,
            Available = true,
            PageIndex = pageIndex,
            PageSize = pageSize,
        }));
    }

    [HttpGet("customer/promotion/platform")]
    public async Task<IActionResult> GetPlatformPromotion(int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetPlatformPromotionListQuery
        {
            Status = (int) PlatformPromotionStatus.Active,
            Available = true,
            PageIndex = pageIndex,
            PageSize = pageSize,
        }));
    }
}