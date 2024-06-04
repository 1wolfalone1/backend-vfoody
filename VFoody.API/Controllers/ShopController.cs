using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Shop.Queries.ListShop;
using VFoody.Application.UseCases.Shop.Queries.ShopInfo;
using VFoody.Application.UseCases.Shop.Queries.ShopSearching;
using VFoody.Application.UseCases.Shop.Queries.ShopTop;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
[Authorize(Roles = IdentityConst.CustomerClaimName)]
public class ShopController : BaseApiController
{
    private readonly IMapper _mapper;

    public ShopController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("customer/shop/top")]
    public async  Task<IActionResult> GetTopShop(int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetTopShopQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpGet("customer/shop/search")]
    public async Task<IActionResult> GetSearchingProductQuery(int pageIndex, int pageSize, string searchText = "", int orderType = 0, int currentBuildingId = 0)
    {
        return this.HandleResult(await this.Mediator.Send(new GetSearchingShopQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchText = searchText,
            OrderType = orderType,
            CurrentBuildingId = currentBuildingId,
        }));
    }

    [HttpGet("shop/info")]
    public async Task<IActionResult> GetShopInfo(int shopId)
    {
        return HandleResult(await Mediator.Send(new GetShopInfoQuery(shopId)));
    }
    
    [HttpGet("customer/shop")]
    public async Task<IActionResult> GetShopInfo([FromQuery] int[] ids)
    {
        return HandleResult(await Mediator.Send(new GetListShopQuery
        {
            shopIds = ids
        }));
    }
}