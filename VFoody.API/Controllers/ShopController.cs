using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Shop.Commands.UpdateProfile.UpdateInfo;
using VFoody.Application.UseCases.Shop.Queries.ListShop;
using VFoody.Application.UseCases.Shop.Queries.ManageShop;
using VFoody.Application.UseCases.Shop.Queries.ShopDetail;
using VFoody.Application.UseCases.Shop.Queries.ShopFavourite;
using VFoody.Application.UseCases.Shop.Queries.ShopInfo;
using VFoody.Application.UseCases.Shop.Queries.ShopInfoForShop;
using VFoody.Application.UseCases.Shop.Queries.ShopSearching;
using VFoody.Application.UseCases.Shop.Queries.ShopTop;
using VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopBannerImage;
using VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopLogoImage;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class ShopController : BaseApiController
{
    private readonly IMapper _mapper;

    public ShopController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("customer/shop/top")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async  Task<IActionResult> GetTopShop(int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetTopShopQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpGet("customer/shop/search")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetSearchingProductQuery([FromQuery] GetSearchingShopQuery query)
    {
        return this.HandleResult(await this.Mediator.Send(query));
    }

    [HttpGet("shop/info")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetShopInfo(int shopId)
    {
        return HandleResult(await Mediator.Send(new GetShopInfoForCustomerQuery(shopId)));
    }
    
    [HttpGet("customer/shop")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetShopInfo([FromQuery] int[] ids)
    {
        return HandleResult(await Mediator.Send(new GetListShopQuery
        {
            shopIds = ids
        }));
    }

    [HttpGet("customer/shop/favourite")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetShopFavourite(int pageIndex, int pageSize)
    {
        return HandleResult(await Mediator.Send(new GetShopFavouriteQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpPost("admin/shop/all")]
    [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetAllShop([FromBody] GetAllShopQuery getAllShopQuery)
    {
        return this.HandleResult(await Mediator.Send(getAllShopQuery));
    }

    [HttpGet("admin/shop/detail")]
    [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetShopDetail(int shopId)
    {
        return HandleResult(await Mediator.Send(new GetShopDetailQuery(shopId)));
    }

    [HttpGet("shop/profile")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetShopProfile()
    {
        return this.HandleResult(await this.Mediator.Send(new GetShopProfileQuery()));
    }
    
    [HttpPut("shop/profile/banner")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> ShopUploadBannerImage(IFormFile bannerImage)
    {
        return this.HandleResult(await this.Mediator.Send(new UploadShopBannerImageCommand()
        {
            BannerImage = bannerImage
        }));
    }
    
    [HttpPut("shop/profile/logo")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> ShopUploadLogoImage(IFormFile logoImage)
    {
        return this.HandleResult(await this.Mediator.Send(new UploadShopLogoImageCommand()
        {
            LogoImage = logoImage
        }));
    }
    
    [HttpPut("shop/profile")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> ShopUpdateShopProfile([FromBody] UpdateInfoShopCommand command)
    {
        return this.HandleResult(await this.Mediator.Send(command));
    }
}