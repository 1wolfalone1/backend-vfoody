using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Product.Commands;
using VFoody.Application.UseCases.Product.Queries;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class ProductController : BaseApiController
{
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public ProductController(IMapper mapper, ICurrentPrincipalService currentPrincipalService)
    {
        _mapper = mapper;
        _currentPrincipalService = currentPrincipalService;
    }

    [HttpGet("customer/product/top")]
    public async  Task<IActionResult> GetTopProduct(int pageIndex = 1, int pageSize = 20)
    {
        return this.HandleResult(await this.Mediator.Send(new GetTopProductQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpGet("customer/product/recent")]
    public async Task<IActionResult> GetRecentOrderedProductQuery(int pageIndex = 1, int pageSize = 20)
    {
        string email = _currentPrincipalService.CurrentPrincipal;
        return this.HandleResult(await this.Mediator.Send(new GetRecentOrderedProductQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Email = email
        }));
    }

    [HttpPost("shop/product/create")]
    public async  Task<IActionResult> CreateProduct([FromForm] CreateProductRequest createProductRequest)
    {
        return HandleResult(await Mediator.Send(new CreateProductCommand
        {
            CreateProductRequest = createProductRequest
        }));
    }
}