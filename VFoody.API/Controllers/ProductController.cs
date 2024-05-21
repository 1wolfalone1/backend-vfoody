using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Commands.ReVerify;
using VFoody.Application.UseCases.Accounts.Commands.Verify;
using VFoody.Application.UseCases.Product.Queries;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class ProductController : BaseApiController
{
    private readonly IMapper _mapper;

    public ProductController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("customer/product/top")]
    public async  Task<IActionResult> GetTopProduct(int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetTopProductQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpGet("customer/product/recent")]
    public async Task<IActionResult> GetRecentOrderedProductQuery(int pageIndex, int pageSize, string phone)
    {
        return this.HandleResult(await this.Mediator.Send(new GetRecentOrderedProductQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Phone = phone
        }));
    }
}