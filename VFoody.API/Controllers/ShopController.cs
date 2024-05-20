using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Commands.ReVerify;
using VFoody.Application.UseCases.Accounts.Commands.Verify;
using VFoody.Application.UseCases.Shop.Queries;

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
    public async  Task<IActionResult> GetTopShop(int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetTopShopQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }
}