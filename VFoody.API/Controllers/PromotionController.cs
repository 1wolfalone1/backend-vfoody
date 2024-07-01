using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Promotion.Commands.CreatePromotion;
using VFoody.Application.UseCases.Promotion.Commands.CreateShopPromotion;
using VFoody.Application.UseCases.Promotion.Commands.UpdatePromotionInfo;
using VFoody.Application.UseCases.Promotion.Commands.UpdateShopPromotion;
using VFoody.Application.UseCases.Promotion.Commands.UpdateShopPromotionStatus;
using VFoody.Application.UseCases.Promotion.Commands.UploadImageForPlatformPromotion;
using VFoody.Application.UseCases.Promotion.Queries.All;
using VFoody.Application.UseCases.Promotion.Queries.AllForAdmin;
using VFoody.Application.UseCases.Promotion.Queries.AllPromotionOfShopOwner;
using VFoody.Application.UseCases.Promotion.Queries.Customer;
using VFoody.Application.UseCases.Promotion.Queries.DetailPromotionOfShopOwner;
using VFoody.Application.UseCases.Promotion.Queries.Platform;
using VFoody.Application.UseCases.Promotion.Queries.Shop;
using VFoody.Domain.Enums;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
[Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
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

    [HttpGet("customer/promotion/all")]
    public async Task<IActionResult> GetAllPlatformPromotion([FromQuery] GetAllPromotionForCustomerQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpPost("admin/promotion")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePlatformPromotion([FromBody] CreatePromotionRequest promotion)
    {
        return this.HandleResult(await this.Mediator.Send(new CreatePromotionCommand{CreatePromotion = promotion}));
    }

    [HttpGet("admin/promotion")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPromotionWithCondition([FromQuery] GetAllPromotionForAdminPageQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpPost("admin/promotion/upload")]
    [AllowAnonymous]
    public async Task<IActionResult> UploadImageForPlatformPromotion(IFormFile image)
    {
        return this.HandleResult(await this.Mediator.Send(new UploadBannerPlatformPromotionCommand()
        {
            BannerImage = image
        }));
    }

    [HttpPut("admin/promotion/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateAllPromotionInfo([FromBody] UpdatePromotionInfoRequest promotion, int id)
    {
        promotion.Id = id;
        return this.HandleResult(await this.Mediator.Send(new UpdatePromotionInfoCommand()
        {
            Promotion = promotion
        }));
    }

    [HttpPost("shop-owner/promotion/create")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> CreateShopPromotion([FromBody] CreateShopPromotionCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPut("shop-owner/promotion/update")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> UpdateShopPromotion([FromBody] UpdateShopPromotionCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPut("shop-owner/promotion/status/update")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> UpdateShopPromotionStatus([FromBody] UpdateShopPromotionStatusCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("shop-owner/promotion/all")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> GetAllPromotion([FromBody] GetAllPromotionShopQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }

    [HttpGet("shop-owner/promotion/detail")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> GetAllPromotion(int id)
    {
        return HandleResult(await Mediator.Send(new GetPromotionShopDetailQuery{Id = id}));
    }
}